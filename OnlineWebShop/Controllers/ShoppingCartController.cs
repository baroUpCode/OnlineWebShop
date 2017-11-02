using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;
using System.Web.SessionState;

namespace OnlineWebShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        // GET: ShoppingCart
        /// <summary>
        /// Hàm tạo mới một giỏ hàng trong một phiên sử dụng nếu chưa có , và trả về đối tượng List<ShoppingCarts>
        /// Nếu giỏ hàng đã được tạo thì lấy ra giỏ hàng , còn không thì tạo mới một đối tượng giỏ hàng và thêm vào Session
        /// </summary>
        /// <returns></returns>
        public List<ShoppingCartModels> GetShoppingCart()
        {
            //Dùng as thay vì ép kiểu để tạo mới giỏ hàng khi mà người dùng chưa tao, còn nếu ép kiểu sẽ bị lỗi vì giỏ hàng vẫn chưa tồn tại
            //Tạo mới một Session kiểu List<ShoppingCarts>, nếu trong Session chưa có đối tượng thì tạo mới đối tượng cho session và gán lại giá
            //trị đối tượng khởi tạo vào session
            List<ShoppingCartModels> cart = /*(List<ShoppingCarts>)*/Session["Cart"] as List<ShoppingCartModels>;
            if(cart==null)
            {
                cart = new List<ShoppingCartModels>();
                Session["Cart"]= cart; // Cart duoc luu vao Session
            }
            return cart;   
        }
        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        /// <param name="proID"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult InsertCart(int proID , string url)
        {
            //Lay ra session gio hang 
            List<ShoppingCartModels> cart = GetShoppingCart();
            //Tim thong tin san pham trong Cart theo ID(bien) truyen vao tu View
            //product bao gom cac thuoc tinh trong shoppingcarts, kiem tra xem san pham co ton tai trong gio hang chua 
            ShoppingCartModels product = cart.Find(x => x._productID == proID);
            if(product == null)// neu chua ton tai san pham trong session gio hang
            {
                product = new ShoppingCartModels(proID);// Khoi tao mot san pham kieu doi tuong ShoppingCarts va truyen vao cac bien(thuoc tinh)
                //cua san pham proID, proID se duoc ham trong ShoppingCarts xu ly va lay thong tin san pham co ID=proID
                cart.Add(product);
                return Redirect(url);
            }
            else
            {
                product._quantity++;// so luong san pham co id=proID tang 1 
                return Redirect(url);
            }
        }
 
        /// <summary>
        /// Xóa trực tiếp sản phẩm trong giỏ hàng 
        /// </summary>
        /// <param name="proID"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult DeleteProductCart(int proID)
        {
            List<ShoppingCartModels> cart = GetShoppingCart() ;
            ShoppingCartModels product = cart.SingleOrDefault(x => x._productID ==proID);
            if (product != null)
            {
                cart.RemoveAll(x => x._productID == proID);
                return RedirectToAction("Cart");
            }
            if(cart.Count==0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Cart");
        }
        /// <summary>
        /// Hàm tính tổng tiền của giỏ hàng
        /// </summary>
        /// <returns></returns>
        private double SumTotal()
        {
            double total = 0;
            List<ShoppingCartModels> cart = Session["Cart"] as List<ShoppingCartModels>;
            if(cart!=null)
            {
                total = cart.Sum(x => x._total);
            }
            return total;
        }
        /// <summary>
        /// Hàm tính tổng số lượng của giỏ hàng
        /// </summary>
        /// <returns></returns>
        public int SumAmount()
        {
            int amount = 0;
            List<ShoppingCartModels> cart = Session["Cart"] as List<ShoppingCartModels>;
            if (cart != null)
            {
                amount = cart.Sum(x => x._quantity);
            }
            //ViewBag.Quantity = SumQuantity();
            return amount;

        }
        /// <summary>
        /// Controller giỏ hàng - tạo giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Cart()
        {

            List<ShoppingCartModels> carts = GetShoppingCart();
            if (carts.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Amount = SumAmount();
            ViewBag.Total = SumTotal();
            return View(carts);
        }
        /// <summary>
        /// Hiển thị giỏ hàng mini ở nav-bar 
        /// </summary>
        /// <returns></returns>
        public ActionResult CartPartial()
        {
            ViewBag.Total = SumTotal();
            ViewBag.Amount = SumAmount();
            return PartialView();
        }
        public ActionResult DeleteCart()
        {
            List<ShoppingCartModels> carts = GetShoppingCart();
            if (carts.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                carts.Clear();
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult UpdateAmount(int id, FormCollection f)
        {
            List<ShoppingCartModels> carts = GetShoppingCart();
            ShoppingCartModels product = carts.SingleOrDefault(x => x._productID == id);
            if(product != null)
            {
                product._quantity = int.Parse(f["amount"].ToString());// số lượng sản phẩm = số lượng từ forrm truyền vào 
            }
            return RedirectToAction("Cart");
        }
        [HttpGet]
        public ActionResult OrderInform()
        {
            //Nếu ô thông tin bị bỏ trống thì không dược thanh toán 
            //if(Session["AccountCustomer"] !=null || Session["AccountCustomer"].ToString() != null)
            //{
            //    return View();
            //}
            //else
            //{
                
            //}
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ShoppingCartModels> cart = GetShoppingCart();
            ViewBag.Total = SumTotal();
            ViewBag.Amount = SumAmount();
            return View(cart);
        }
        /// <summary>
        /// can luu thong tin vao ShoppingCart, Order va Detals
        /// ShoppingCart -> customersID, Status
        /// Details -> proID,cartID,quantity,amount,unitprice
        /// order -> status =false(chua thanh toan) order.CartID=cart.cartID => not yet
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderInform(FormCollection f)
        {
            Order order = new Order();
            if (DateTime.Parse(f["deliveryDate"]) < DateTime.Now)
            {
                ViewData["ErrorDate"] = "Ngày giao không hợp lệ";
                return this.OrderInform();
            }
            else
            {
                List<ShoppingCartModels> listCart = GetShoppingCart();
                var total = listCart.Sum(x => x._total).ToString();
                Detail de = new Detail();

                if (Session["AccountCustomer"] != null)
                {
                    Customer cus = (Customer)Session["AccountCustomer"];
                    Reciever rec = new Reciever();
                    order.CustomerID = cus.CustomerID; // luu lai thong tin listgiohang vao db 
                    order.Status = 0;
                    order.Total = Int32.Parse(total);
                    order.DeliveryDate = DateTime.Parse(f["deliveryDate"]);
                    db.Orders.InsertOnSubmit(order);
                    db.SubmitChanges();
                    rec.RecieverName = cus.FullName;
                    rec.Address = cus.Address;
                    rec.Phone = Convert.ToInt32(cus.Phone);
                    rec.CustomerID = cus.CustomerID;
                    db.Recievers.InsertOnSubmit(rec);
                    db.SubmitChanges();
                }
                else
                {
                    Reciever rec = new Reciever();
                    rec.RecieverName = f["Name"];
                    rec.Address = f["Address"];
                    rec.Phone = Convert.ToInt32(f["Phone"]);
                    db.Recievers.InsertOnSubmit(rec);
                    db.SubmitChanges();
                    order.RecieverID = rec.RecieverID;
                    db.Orders.InsertOnSubmit(order);
                    order.DeliveryDate = DateTime.Parse(f["deliveryDate"]);
                    order.Total = Int32.Parse(total);
                    order.Status = 0;
                    db.Orders.InsertOnSubmit(order);
                    db.SubmitChanges();
                }
                //Duyệt các sản phẩm trong giỏ hàng.Thêm thông tin sản phẩm vào details
                foreach (var item in listCart)
                {
                    de.OrderID = order.OrderID;
                    de.ProductID = item._productID;
                    de.Quantity = item._quantity;
                    de.UnitPrice = item._unitPrice;
                    de.Total = item._total;
                    db.Details.InsertOnSubmit(de);
                }
                db.SubmitChanges();
                Session["Cart"] = null;
                return RedirectToAction("OrderSuccess", "ShoppingCart");
            }
        
            //var Name = f["name"];
            //var Address = f["address"];
            //var Phone = f["phone"];
            //if (String.IsNullOrEmpty(Name))
            //{
            //    ViewData["ErrorName"] = "Vui lòng nhập tên người nhận";
            //}
            //if (String.IsNullOrEmpty(Address))
            //{
            //    ViewData["ErrorAddress"] = "Vui lòng nhập địa chỉ nhận hàng";
            //}
            //if (String.IsNullOrEmpty(Phone))
            //{
            //    ViewData["ErrorPhone"] = "Vui lòng nhập số điện thoại người nhận";
            //}
            //else
            //{
            //    rec.RecieverName = Name;
            //    rec.Address = Address;
            //    rec.Phone = Int32.Parse(Phone);
            //    db.Recievers.InsertOnSubmit(rec);
            //    db.SubmitChanges();
            //}
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }
        //public ActionResult CustomerInform(int id)
        //{
        //    Customer cus = db.Customers.SingleOrDefault(x => x.CustomersID == id);
        //    Reciever rec = new Reciever();
        //    rec.RecieverName = cus.FullName;
        //    rec.Address = cus.Address;
        //    rec.Phone = Convert.ToInt32(cus.Phone);
        //    db.Recievers.InsertOnSubmit(rec);
        //    db.SubmitChanges();
        //    return this.OrderInform(rec);
        //}

    }
}