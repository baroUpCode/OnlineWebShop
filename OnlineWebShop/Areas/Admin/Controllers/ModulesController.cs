using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;
using PagedList;
using System.IO;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using System.Web.Helpers;

namespace OnlineWebShop.Areas.Admin.Controllers
{
    public class ModulesController : Controller
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        // GET: Admin/Product
        /// <summary>
        /// Module Product
        /// Edit-Delete-Insert 
        /// </summary>
        /// <returns></returns>
        public ActionResult TableData(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.AdminAccounts.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        public ActionResult Products(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.Products.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Sửa sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditProduct(int id)
        {

            Product pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            List<SelectListItem> deleList = new List<SelectListItem>() {new SelectListItem() { Value = "1", Text = "Chưa xóa", Selected = true},
                                                                         new SelectListItem() { Value = "2", Text = "Đã xóa" }};

            ViewBag.deleteList = deleList;
            List<Producer> producer = db.Producers.ToList();
            SelectList producerList = new SelectList(producer, "ProducerID", "Name", pro.ProducerID);
            ViewBag.ProList = producerList;
            List<Catogory> cat = db.Catogories.ToList();
            SelectList catList = new SelectList(cat, "CatogoriesID", "CatogoriesName", pro.CatogoriesID);
            ViewBag.CatList = catList;
            //TempData -> lưu giá trị hình ảnh hiện tại của Product trước khi thay đổi và display ở View
            TempData["file"] = pro.ProductImages;
            //ViewBag.Error = TempData["e"] == null ? "" : TempData["e"].ToString();
            //ViewBag.Files = TempData["file"] == null ? new List<string>() : (List<string>)TempData["file"];
            return View(pro);
        }
        [HttpPost, ActionName("EditProduct")]
        [ValidateInput(false)]
        public ActionResult EditProduct(int id, FormCollection f, HttpPostedFileBase uploadFile)
        {

            Product pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            if (pro != null)
            {

                if (uploadFile == null)
                {
                    UpdateModel(pro);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //lay duong dan cua anh
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        //Luu anh tu duong dan sang ~assets/images
                        var pathstr = Path.Combine(Server.MapPath("~/Assets/images/"), fileName);
                        if (System.IO.File.Exists(pathstr))
                        {
                            ViewBag.Mess = "Hình ảnh đã tồn tại";
                            return this.EditProduct(id);
                        }
                        else
                        {
                            uploadFile.SaveAs(pathstr);
                        }
                        pro.ProductImages = fileName;
                        UpdateModel(pro);
                        db.SubmitChanges();
                    }
                }
                pro.Name = f["productName"];
                pro.Quantity = Convert.ToInt32(f["productQuantity"]);
                pro.Warranty = Convert.ToInt32(f["productWarranty"]);
                pro.Price = Convert.ToDecimal(f["productPrice"]);
                pro.CatogoriesID = Convert.ToInt32(f["productCatogories"]);
                pro.ProducerID = Convert.ToInt32(f["productProducer"]);
                pro.Description = f["productDescription"];
                db.SubmitChanges();
                return RedirectToAction("Products", "Modules");
            }
            else
                return RedirectToAction("EditProduct", "Modules");
        }
        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertProduct()
        {
            //List<Producer> producer = db.Producers.ToList();
            //SelectList producerList = new SelectList(producer, "ProducerID", "Name");
            ViewBag.ProList = new SelectList(db.Producers.OrderBy(x => x.Name), "ProducerID", "Name");
            //List<Catogory> cat = db.Catogories.ToList();
            //SelectList catList = new SelectList(cat, "CatogoriesID", "CatogoriesName");
            ViewBag.CatList = new SelectList(db.Catogories.OrderBy(x => x.CatogoriesName), "CatogoriesID", "CatogoriesName"); ;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertProduct(FormCollection f, HttpPostedFileBase uploadFile)
        {
            Product pro = new Product();
            if (uploadFile == null)
            {
                UpdateModel(pro);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //lay duong dan cua anh
                    var fileName = Path.GetFileName(uploadFile.FileName);
                    //Luu anh tu duong dan sang ~assets/images
                    var pathstr = Path.Combine(Server.MapPath("~/Assets/images/"), fileName);
                    uploadFile.SaveAs(pathstr);
                    //if (System.IO.File.Exists(pathstr))
                    //{
                    //    ViewBag.Mess = "Hình ảnh đã tồn tại";
                    //    return this.InsertProduct();
                    //}
                    //else
                    //{
                    //    uploadFile.SaveAs(pathstr);
                    //}
                    pro.ProductImages = fileName;
                    UpdateModel(pro);
                    db.SubmitChanges();
                }
            }
            pro.Name = f["productName"];
            pro.Status = true;
            pro.Deleted = 1;
            pro.Quantity = Convert.ToInt32(f["productQuantity"]);
            pro.Warranty = Convert.ToInt32(f["productWarranty"]);
            pro.Description = f["productDescription"];
            pro.ProducerID = Convert.ToInt32(f["productProducer"]);
            pro.CatogoriesID = Convert.ToInt32(f["productCatogories"]);
            pro.Price = Convert.ToDecimal(f["productPrice"]);
            db.Products.InsertOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("Products", "Modules");
        }
        /// <summary>
        /// Danh sách loại sản phẩm
        /// Edit-Delete-Insert 
        /// </summary>
        /// <returns></returns>
        public ActionResult Catogories(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.Catogories.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Sửa loại sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCatogories(int id)
        {

            Catogory cat = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            List<ParentCatogory> root = db.ParentCatogories.ToList();

            ViewBag.RootCat = new SelectList(root, "RootCatogoryID", "RootCatogoryName", cat.RootCatogoryID);
            return View(cat);
        }
        [HttpPost]
        public ActionResult EditCatogories(int id, FormCollection f)
        {

            Catogory cat = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            cat.CatogoriesName = f["catName"];
            cat.RootCatogoryID = Convert.ToInt32(f["catRoot"]);
            db.SubmitChanges();
            return RedirectToAction("Catogories", "Modules");
        }
        /// <summary>
        /// Thêm loại sản phẩm
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertCatogories()
        {
            ViewBag.CatList = new SelectList(db.ParentCatogories.OrderBy(x => x.RootCatogoryName), "RootCatogoryID", "RootCatogoryName");
            return View();
        }
        [HttpPost]
        public ActionResult InsertCatogories(FormCollection f,string url)
        {
            Catogory cat = new Catogory();
            cat.CatogoriesName = f["catName"];
            cat.Deleted = 1;
            cat.RootCatogoryID = Convert.ToInt32(f["catRoot"]);
            db.Catogories.InsertOnSubmit(cat);
            db.SubmitChanges();
            return RedirectToAction("InsertProduct","Modules");
        }
        /// <summary>
        /// Xóa loại sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCatogories(int id)
        {
            Catogory cat = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            cat.Deleted = 2;
            db.SubmitChanges();
            return RedirectToAction("Catogories", "Modules");
        }
        public ActionResult ConfirmDeleteCatogories(int id)
        {
            var cat = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            if (cat.Deleted == 2)
            {
                db.Catogories.DeleteOnSubmit(cat);
                db.SubmitChanges();
                return RedirectToAction("DeletedCatogories", "Garbage");
            }
            return RedirectToAction("DeletedCatogories", "Garbage");

        }
        //public ActionResult CatogoriesList( )
        //{
        //    List<Catogory> cat = db.Catogories.ToList();
        //    SelectList catList = new SelectList(cat, "CatogoriesID", "CatogoriesName");
        //    ViewBag.CatList = catList;
        //    return PartialView(ViewBag.CatList);
        //}
        /// <summary>
        /// Danh sách nhà sản xuất
        /// </summary>
        /// <returns></returns>
        public ActionResult Producer(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.Producers.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Sửa thông tin nhà sản xuất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditProducer(int id)
        {
            Producer pro = db.Producers.SingleOrDefault(x => x.ProducerID == id);
            return View(pro);
        }
        [HttpPost]
        public ActionResult EditProducer(int id, FormCollection f)
        {
            Producer pro = db.Producers.SingleOrDefault(x => x.ProducerID == id);
            if (pro != null)
            {
                pro.Name = f["producerName"];
                db.SubmitChanges();
                return RedirectToAction("Producer", "Modules");
            }
            else
                return RedirectToAction("Producer", "Modules");
        }
        /// <summary>
        /// Xóa nhà sản xuất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteProducer(int id)
        {
            Producer pro = db.Producers.SingleOrDefault(x => x.ProducerID == id);
            pro.Deleted = 2;
            db.SubmitChanges();
            return RedirectToAction("Products", "Modules");

        }
        /// <summary>
        /// Thêm nhà sản xuất
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertProducer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertProducer(FormCollection f)
        {

            Producer pro = new Producer();
            pro.Name = f["proName"];
            pro.Deleted = 1;
            db.Producers.InsertOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("InsertProduct","Modules");
        }
        /// <summary>
        /// Danh sách tin tức
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult News(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.News.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Sửa thông tin tin tức 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditNews(int id)
        {
            New ne = db.News.SingleOrDefault(x => x.NewsID == id);
            List<ParentCatogory> root = db.ParentCatogories.ToList();
            ViewBag.RootCat = new SelectList(root, "RootCatogoryID", "RootCatogoryName", ne.RootCatogoryID);
            return View(ne);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNews(int id, FormCollection f, HttpPostedFileBase uploadFile)
        {
            //
            New news = db.News.SingleOrDefault(x => x.NewsID == id);
            if (news != null)
            {
                if (uploadFile == null)
                {
                    UpdateModel(news);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //lay duong dan cua anh
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        //Luu anh tu duong dan sang ~assets/images
                        var pathstr = Path.Combine(Server.MapPath("~/Assets/images/"), fileName);
                        if (System.IO.File.Exists(pathstr))
                        {
                            ViewBag.Mess = "Hình ảnh đã tồn tại";
                            return this.EditNews(id);
                        }
                        else
                        {
                            uploadFile.SaveAs(pathstr);
                        }
                        news.Images = fileName;
                        UpdateModel(news);
                        db.SubmitChanges();
                    }
                }
                news.Title = f["newsTitle"];
                news.Content = f["newsContent"];
                news.RootCatogoryID = Convert.ToInt32(f["newsRootCat"]);
                db.SubmitChanges();
                return RedirectToAction("News", "Modules");

            }
            else
                return RedirectToAction("News", "Modules");
        }
        /// <summary>
        /// Xóa tin tức
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteNews(int id)
        {
            New news = db.News.SingleOrDefault(x => x.NewsID == id);
            news.Deleted = 2;
            db.SubmitChanges();
            return RedirectToAction("News", "Modules");

        }
        /// <summary>
        /// Thêm tin tức
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertNews()
        {
            List<ParentCatogory> root = db.ParentCatogories.ToList();
            ViewBag.RootCat = new SelectList(root, "RootCatogoryID", "RootCatogoryName");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertNews(FormCollection f, HttpPostedFileBase uploadFile)
        {
            New news = new New();
            if (uploadFile == null)
            {
                UpdateModel(news);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //lay duong dan cua anh
                    var fileName = Path.GetFileName(uploadFile.FileName);
                    //Luu anh tu duong dan sang ~assets/images
                    var pathstr = Path.Combine(Server.MapPath("~/Assets/images/"), fileName);
                    if (System.IO.File.Exists(pathstr))
                    {
                        ViewBag.Mess = "Hình ảnh đã tồn tại";
                        return this.InsertNews();
                    }
                    else
                    {
                        uploadFile.SaveAs(pathstr);
                    }
                    news.Images = fileName;
                    UpdateModel(news);
                    db.SubmitChanges();
                }
            }
            news.Content = f["newsContent"];
            news.Title = f["newsTitle"];
            news.Deleted = 1;
            news.Images = f["newsImage"].ToString();
            news.RootCatogoryID = Convert.ToInt32(f["newsRootCat"]);
            db.News.InsertOnSubmit(news);
            db.SubmitChanges();
            return RedirectToAction("News", "Modules");
        }
        /// <summary>
        /// Module Customers
        /// Edit-Delete-Insert 
        /// </summary>
        /// <returns></returns>
        public ActionResult Customers(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.Customers.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Sửa thông tin khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCustomer(int id)
        {
            Customer cus = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            return View(cus);
        }
        [HttpPost]
        public ActionResult EditCustomer(int id, FormCollection f)
        {
            Customer cus = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            if (cus != null)
            {
                cus.Address = f["Address"];
                cus.Phone = Convert.ToString(f["Phone"]);
                cus.Pass = f["Pass"];
                db.SubmitChanges();
                return RedirectToAction("Customers", "Modules");
            }
            else
                Response.StatusCode = 404;
            return RedirectToAction("Customers", "Modules");
        }
        /// <summary>
        /// Xóa thông tin khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCustomer(int id)
        {
            Customer cus = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            cus.Deleted = 2;
            db.SubmitChanges();
            return RedirectToAction("Customers", "Modules");
        }
        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertCustomer(FormCollection f)
        {
            Customer cus = new Customer();
            cus.FullName = f["Name"];
            cus.Deleted = 1;
            cus.Email = f["Email"];
            cus.Phone = f["Phone"];
            cus.Pass = f["cusPass"];
            cus.Address = f["Address"];
            if (DateTime.Parse(f["BirthDay"]) > DateTime.Now)
            {
                ViewBag.Mess = "Ngày sinh nhập vào không hợp lệ. Vui lòng nhập lại.";
                return this.InsertCustomer();
            }
            else
            {
                cus.BirthDay = DateTime.Parse(f["BirthDay"]);
            }
            cus.Pass = f["Pass"];
            db.Customers.InsertOnSubmit(cus);
            db.SubmitChanges();
            return RedirectToAction("Customers", "Modules");
        }
        /// <summary>
        /// Thêm mới Quản trị viên
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertAdmin(FormCollection f)
        {
            AdminAccount u = new AdminAccount();
            u.Email = f["email"];
            u.FullName = f["name"];
            u.PermissionID = Convert.ToByte(f["permissionUser"]);
            u.Pass = f["passUser"];
            db.AdminAccounts.InsertOnSubmit(u);
            db.SubmitChanges();
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult InsertAdmin(int id)
        {
            var admin = db.AdminAccounts.SingleOrDefault(x => x.AdminID == id);

            return View(admin);
        }
        /// <summary>
        /// Hàm lấy dữ liệu PermissionID từ DB để trả về HTML.DropDownLis
        /// </summary>
        /// <returns></returns>
        public ActionResult PermissionList()
        {
            //Tạo một danh sách đối tượng Permission
            List<Permission> per = db.Permissions.ToList();
            //Tạo một biến đối tượng kiểu SelectList với tham số truyền vào là Danh sách thông tin Permission lấy được, lấy giá trị(item value) option là ID và tên hiển thị(text display) là PermissionName
            SelectList perList = new SelectList(per, "PermissionID", "PermissionName");
            //Lưu biến vừa tạo vào ViewBag để Access Data ở View
            ViewBag.PermissionList = perList;
            return PartialView(ViewBag.PermissionList);
        }
        [HttpPost]
        public ActionResult EditAdmin(FormCollection f, int id)
        {
            List<AdminAccount> perid = new List<AdminAccount>();
            var u = db.AdminAccounts.FirstOrDefault(x => x.AdminID == id);
            u.FullName = f["nameUser"];
            u.PermissionID = Convert.ToByte(f["permissionUser"]);
            u.Email = f["email"];
            db.SubmitChanges();
            return RedirectToAction("Index", "Admin");
        }
        /// <summary>
        /// Xóa thông tin Quản trị viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteAdmin(int id)
        {
            var u = db.AdminAccounts.SingleOrDefault(x => x.AdminID == id);
            db.AdminAccounts.DeleteOnSubmit(u);
            db.SubmitChanges();
            return RedirectToAction("Index", "Admin");
        }
        /// <summary>
        /// Show-Sua Order
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Orders(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            return View(db.Orders.OrderByDescending(x => x.CreatedAt).ToPagedList(pageNum, pageSize));
        }
        public ActionResult SearchOrders(int? page, string searchString)
        {
            var product = db.Orders.Where(x => (x.Customer.FullName.Contains(searchString) || x.Reciever.Phone.ToString().Contains(searchString) || x.DeliveryDate.ToString().Contains(searchString)));
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.Search = searchString;
            return View(product.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Them hoa don : can chon san pham + tinh tong tien cua hoa don roi xuat ra 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public ActionResult InsertOrders()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult InsertOrders(FormCollection f)
        //{
        //    Order order = new Order();
        //    if (ModelState.IsValid)
        //    {
        //        order.Reciever.RecieverName = f["recName"];
        //        order.Reciever.Address = f["recAddress"];
        //        order.Reciever.Phone = Int32.Parse(f["recPhone"]);
        //        order.Status = Convert.ToByte(f["ordStatus"]);
        //        order.DeliveryDate = DateTime.Parse(f["deliveryDate"]);
        //        db.Orders.InsertOnSubmit(order);
        //        db.SubmitChanges();
        //    }
        //    return RedirectToAction("Orders", "Modules");
        //}

        public ActionResult EditOrders(int id)
        {
            //Tạo một danh sách đối tượng SelectListItem để lưu giá trị Item trong SelectList và trả về cho View bằng ViewBag
            var order = db.Orders.SingleOrDefault(x => x.OrderID == id);
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem() { Text = "Đang xử lý", Value = "0" });
            status.Add(new SelectListItem() { Text = "Đã giao", Value = "1" });
            ViewBag.OrderStatus = new SelectList(status, "Value", "Text", order.Status.Value);
            return View(order);
        }
        [HttpPost]
        public ActionResult EditOrders(int id, FormCollection f)
        {
            var order = db.Orders.SingleOrDefault(x => x.OrderID == id);
            if (ModelState.IsValid)
            {
                order.Reciever.RecieverName = f["recName"];
                order.Reciever.Address = f["recAddress"];
                order.Reciever.Phone = Int32.Parse(f["recPhone"]);
                order.Status = Convert.ToByte(f["ordStatus"]);
                if (DateTime.Parse(f["deliveryDate"]) < DateTime.Now)
                {
                    ViewBag.Mess = "Ngày giao không được nhỏ hơn ngày hiện tại";
                    return this.EditOrders(id);
                }
                else
                {
                    order.DeliveryDate = DateTime.Parse(f["deliveryDate"]);
                }
                db.SubmitChanges();
            }
            return RedirectToAction("Orders", "Modules");
        }
        /// <summary>
        /// Xóa tạm thông tin hóa đơn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteOrders(int id)
        {
            var order = db.Orders.SingleOrDefault(x => x.OrderID == id);
            order.Deleted = 2;
            db.SubmitChanges();
            return RedirectToAction("Orders", "Modules");
        }
        public ActionResult ConfirmDeleteOrders(int id)
        {
            var order = db.Orders.SingleOrDefault(x => x.OrderID == id);
            var detail = db.Details.Where(x => x.OrderID == order.OrderID).ToList();
            foreach (var item in detail)
            {
                db.Details.DeleteOnSubmit(item);
            }
            db.Orders.DeleteOnSubmit(order);
            db.SubmitChanges();
            return RedirectToAction("DeletedOrder", "Garbage");
        }
        /// <summary>
        /// Searching button
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult SearchProducts(int? page, string searchString)
        {
            var product = db.Products.Where(x => (x.Name.Contains(searchString) || x.Catogory.CatogoriesName.Contains(searchString) || x.Producer.Name.Contains(searchString)));
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.Search = searchString;
            return View(product.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SearchCatogories(int? page, string searchString)
        {
            var product = db.Catogories.Where(x => (x.CatogoriesName.Contains(searchString) || x.ParentCatogory.RootCatogoryName.Contains(searchString)));
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.Search = searchString;
            return View(product.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SearchCustomers(int? page, string searchString)
        {
            var product = db.Customers.Where(x => (x.FullName.Contains(searchString) || x.CustomerID.ToString() == searchString || x.Email.Contains(searchString) || x.Address.Contains(searchString) || x.Phone.Contains(searchString)));
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.Search = searchString;
            return View(product.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SearchNews(int? page, string searchString)
        {
            var product = db.News.Where(x => (x.NewsID.ToString() == searchString || x.Title.Contains(searchString) || x.ParentCatogory.RootCatogoryName.Contains(searchString) || x.Content.Contains(searchString)));
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.Search = searchString;
            return View(product.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SearchProducers(int? page, string searchString)
        {
            var product = db.Producers.Where(x => (x.Name.Contains(searchString) || x.ProducerID.ToString() == searchString));
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.Search = searchString;
            return View(product.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Chi tiết thông tin sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductDetail(int id)
        {
            var pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            return View(pro);
        }
        /// <summary>
        /// Xóa tạm thông tin sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteProduct(int id)
        {
            var product = db.Products.SingleOrDefault(x => x.ProductID == id);
            product.Deleted = 2;
            db.SubmitChanges();
            return RedirectToAction("Products", "Modules");
        }

        /// <summary>
        /// Tạo form View mẫu để người dùng biết được form xuất ra sẽ như thế nào, tự design giống với function ExportOrdersData 
        /// </summary>
        /// <returns></returns>
        //public ActionResult ExportOrdersToExcel()
        //{
        //    var order = db.Orders.OrderByDescending(x=>x.CreatedAt).ToList();
        //    var total = order.Sum(x => x.Total);
        //    ViewBag.Total = total;
        //    return View(order);
        //    // Step 1 - get the data from database
        //}
        /// <summary>
        /// Hàm xuất thông kê ra file excel với các thuộc tính tương ứng muốn xuất ra, ở đay (chọn thông tin muốn xuất và thêm vào webGrid)
        /// </summary>
        //public ActionResult ExportOrdersData() { 
        //    var data = db.Orders.ToList();
        //    var total = data.Sum(x => x.Total);
        //    //Khởi tạo một đối tượng WebGrid và truyền vào data để vẽ được các thuộc tính trong dât bằng đối tượng WebGrid
        //    //Vẫn sử dụng đối tượng WebGrid để show thông tin ở View 
        //    WebGrid webGrid = new WebGrid(data, canPage: true, rowsPerPage: 20);
        //    //var newR = new WebGridRow(webGrid,total,5);
        //    //webGrid.Rows.Add(newR);
        //    string gridData = webGrid.GetHtml(
        //        columns: webGrid.Columns(
        //     webGrid.Column(columnName: "OrderID", header: "Ma hoa don"),
        //     webGrid.Column(columnName: "CustomerID", header: "Ma khach hang"),
        //     webGrid.Column(columnName: "RecieverID", header: "Ma nguoi nhan"),
        //     webGrid.Column(columnName: "Status", header: "Trang thai xu lu "),
        //     webGrid.Column(columnName: "DeliveryDate", header: "Ngay giao hang"),
        //     webGrid.Column(columnName: "Total", header: "Tong tien")
        //     )).ToString();
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", "attachment; filename=OrdersReport.xls");
        //    Response.ContentType = "applicatiom/excel";
        //    Response.Write(gridData);
        //    Response.End();
            //other way
            //var gv = new GridView();
            //gv.DataSource = db.Orders.ToList();
            //gv.HeaderRow.Cells[0].Text = "Mã hóa đơn";
            //gv.HeaderRow.Cells[1].Text = "Mã khách hàng";
            //gv.HeaderRow.Cells[2].Text = "Mã người nhận";
            //gv.HeaderRow.Cells[3].Text = "Trạng thái xử lý";
            //gv.HeaderRow.Cells[4].Text = "Ngày giao hàng";
            //gv.HeaderRow.Cells[5].Text = "Tổng tiền";
            //gv.DataBind();
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=Order Report.xls");
            //Response.ContentType = "application/ms-excel";
            //Response.Charset = "";
            //StringWriter objStringWriter = new StringWriter();
            //HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            //gv.RenderControl(objHtmlTextWriter);
            //Response.Output.Write(objStringWriter.ToString());
            //Response.Flush();
        //    //Response.End();
        //    return View("ExportOrdersToExcel");
        //}
        ///// <summary>
        ///// Tạo bộ lọc xuất báo cáo theo tháng.
        ///// How to convert Binary Type to DateTime Type 
        ///// </summary>
        ///// <param name="month"></param>
        ///// <returns></returns>
        //public ActionResult ReportFilterByMonth(string month)
        //{
        //    var order = db.Orders.Where(x=>DateTime.Parse(x.CreatedAt.ToString()).Month.ToString() == month).ToList();
        //    var total = order.Sum(x => x.Total);
        //    ViewBag.ToTal = total;
        //    return View(order);
        //}
        ////Tạo một đối tượng ViewModel để lấy dữ liệu cần xuất báo cáo-> sau khi có ModelView thì viết một hàm trả về List<ModelView> và lưu trữ dữ iệu lại , viết một hàm GetOrderData để láy dữ liệu của Model khi đã được set và trả về một bảng gridview định dạng Exce
    }

    }