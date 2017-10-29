using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;
using PagedList;
using System.IO;

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
         public ActionResult TableData()
        {
            var user = db.AdminAccounts;
            return PartialView(user);
        }
    public ActionResult Products(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.Products.OrderByDescending(x=>x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        public ActionResult EditProduct(int id)
        {
            
            Product pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            List<Producer> producer = db.Producers.ToList();
            SelectList producerList = new SelectList(producer, "ProducerID", "Name",pro.ProducerID);
            ViewBag.ProList = producerList;
            List<Catogory> cat = db.Catogories.ToList();
            SelectList catList = new SelectList(cat, "CatogoriesID", "CatogoriesName", pro.CatogoriesID);
            ViewBag.CatList = catList;
            return View(pro);
        }
        [HttpPost,ActionName("EditProduct")]
        [ValidateInput(false)]
        public ActionResult EditProduct(int id,HttpPostedFileBase productImages, FormCollection f)
        {
            Product pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            if (pro != null)
            {
                if (productImages == null)
                {
                    UpdateModel(pro);
                    db.SubmitChanges();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //lay duong dan cua anh
                        var fileName = Path.GetFileName(productImages.FileName);
                        //Luu anh tu duong dan sang ~assets/images
                        var path = Path.Combine(Server.MapPath("~/Assets/images"), fileName);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Mess = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            productImages.SaveAs(path);
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
                return RedirectToAction("Products", "Modules");
        }
    public ActionResult DeleteProduct(int id)
        {
            Product pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            db.Products.DeleteOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("Products", "Modules");

        }
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
        public ActionResult InsertProduct(FormCollection f,HttpPostedFileBase fileUpload)
        {
            Product pro = new Product();
            //if (fileUpload == null)
            //{
            //    ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
            //    return View();
            //}
            //else
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var fileName = Path.GetFileName(fileUpload.FileName);
            //        var path = Path.Combine(Server.MapPath("~/productImage"), fileName);
            //        if (System.IO.File.Exists(path))
            //            ViewBag.ThongBao = "Hình ảnh đã tồn tại";
            //        else
            //        {
            //            fileUpload.SaveAs(path);
            //        }
            //        pro.ProductImages = fileName;
            //    }
            //}
            pro.Name = f["productName"];
            pro.ProductImages = f["productImages"];
            pro.Quantity = Convert.ToInt32(f["productQuantity"]);
            pro.Warranty = Convert.ToInt32(f["productWarranty"]);
            pro.Description = f["productDescription"];
            pro.ProducerID = Convert.ToInt32(f["productProducer"]);
            pro.CatogoriesID = Convert.ToInt32(f["productCatogories"]);
            pro.Price = Convert.ToDecimal(f["productPrice"]);
            db.Products.InsertOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("Products","Modules");
        }
        /// <summary>
        /// Module Catogories
        /// Edit-Delete-Insert 
        /// </summary>
        /// <returns></returns>
        public ActionResult Catogories()
        {
            return View(db.Catogories.ToList());
        }
        public ActionResult EditCatogories(int id)
        {
           
            Catogory cat= db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            List<RootCatogory> root = db.RootCatogories.ToList();
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
            return RedirectToAction("Catogories","Modules");
        }
        public ActionResult InsertCatogories()
        {
            ViewBag.CatList = new SelectList(db.RootCatogories.OrderBy(x => x.RootCatogoryName), "RootCatogoryID", "RootCatogoryName");
            return View();
        }
        [HttpPost]
        public ActionResult InsertCatogories(FormCollection f)
        {
            Catogory cat = new Catogory();
            cat.CatogoriesName = f["catName"];
            cat.RootCatogoryID = Convert.ToInt32(f["catRoot"]);
            db.Catogories.InsertOnSubmit(cat);
            db.SubmitChanges();
            return RedirectToAction("Catogories", "Modules");
        }
        public ActionResult DeleteCatogories(int id)
        {
            Catogory cat = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            db.Catogories.DeleteOnSubmit(cat);
            db.SubmitChanges();
            return RedirectToAction("Catogories", "Modules");
        }
        //public ActionResult CatogoriesList( )
        //{
        //    List<Catogory> cat = db.Catogories.ToList();
        //    SelectList catList = new SelectList(cat, "CatogoriesID", "CatogoriesName");
        //    ViewBag.CatList = catList;
        //    return PartialView(ViewBag.CatList);
        //}
        /// <summary>
        /// Module Producer
        /// Edit-Delete-Insert 
        /// </summary>
        /// <returns></returns>
        public ActionResult Producer()
        {
            return View(db.Producers.ToList());
        }
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
        public ActionResult DeleteProducer(int id)
        {
            Producer pro = db.Producers.SingleOrDefault(x => x.ProducerID == id);
            db.Producers.DeleteOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("Products", "Modules");

        }
        public ActionResult InsertProducer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertProducer(FormCollection f)
        {
           
            Producer pro = new Producer();
            pro.Name = f["proName"];
            db.Producers.InsertOnSubmit(pro);
            db.SubmitChanges();
            return this.InsertProducer();
        }
        //public ActionResult ProducerList()
        //{
        //    List<Producer> producer = db.Producers.ToList();
        //    SelectList producerList = new SelectList(producer, "ProducerID", "Name");
        //    ViewBag.ProList = producerList;
        //    return PartialView(ViewBag.ProList);
        //}
        /// <summary>
        /// Module News
        /// Edit-Delete-Insert 
        /// </summary>
        /// <returns></returns>
        public ActionResult News(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.News.OrderByDescending(x => x.CreatedAt).ToList().ToPagedList(pageNum, pageSize));
        }
        public ActionResult EditNews(int id)
        {
            New ne = db.News.SingleOrDefault(x => x.NewsID == id);
            List<RootCatogory> root = db.RootCatogories.ToList();
            ViewBag.RootCat = new SelectList(root, "RootCatogoryID", "RootCatogoryName", ne.RootCatogoryID);
            return View(ne);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNews(int id, FormCollection f)
        {
            New news = db.News.SingleOrDefault(x => x.NewsID == id);
            if (news != null)
            {
                news.Title = f["newsTitle"];
                news.Content = f["newsContent"];
                news.Images = f["newsImage"].ToString();
                news.RootCatogoryID = Convert.ToInt32(f["newsRootCat"]);
                db.SubmitChanges();
                return RedirectToAction("News", "Modules");
            }
            else
                return RedirectToAction("News", "Modules");
        }
        public ActionResult DeleteNews(int id)
        {
            New news = db.News.SingleOrDefault(x => x.NewsID == id);
            db.News.DeleteOnSubmit(news);
            db.SubmitChanges();
            return RedirectToAction("News", "Modules");

        }
        public ActionResult InsertNews()
        {
            List<RootCatogory> root = db.RootCatogories.ToList();
            ViewBag.RootCat = new SelectList(root, "RootCatogoryID", "RootCatogoryName");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertNews(FormCollection f)
        {
            New news = new New();
            news.Content = f["newsContent"];
            news.Title = f["newsTitle"];
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
        public ActionResult Customers()
        {
            return View(db.Customers.ToList());
        }
        public ActionResult EditCustomer(int id )
        {
            Customer cus = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            return View(cus);
        }
        [HttpPost]
        public ActionResult EditCustomer(int id ,FormCollection f)
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
            public ActionResult DeleteCustomer(int id)
        {
            Customer cus = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            db.Customers.DeleteOnSubmit(cus);
            db.SubmitChanges();
            return RedirectToAction("Customers", "Modules");
        }
        //public ActionResult InsertCustomer()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult InsertCustomer(FormCollection f)
        //{
        //            Customer cus = new Customer();
        //            cus.FullName = f["cusName"];
        //            cus.Email = f["cusEmail"];
        //            cus.Phone = f["cusPhone"];
        //            cus.Pass = f["cusPass"];
        //            cus.Province = f["cusProvince"];
        //            db.Customers.InsertOnSubmit(cus);
        //            db.SubmitChanges();
        //            return RedirectToAction("Customers", "Modules");
        //}
        //public ActionResult InsertUser()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult InsertUser(FormCollection f)
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
        public ActionResult EditUser(int id )
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
            List<Permission> per = db.Permissions.ToList();
            SelectList perList = new SelectList(per, "PermissionID", "PermissionName");//Tạo một biến đối tượng kiểu SelectList
            //với tham số truyền vào là Danh sách thông tin Permission lấy được, lấy giá trị option là ID và tên hiển thị là PermissionName
            ViewBag.PermissionList = perList;
            return PartialView(ViewBag.PermissionList);
        }
        [HttpPost]
        public ActionResult EditUser(FormCollection f, int id)
        {
            List<AdminAccount> perid = new List<AdminAccount>();
            var u = db.AdminAccounts.FirstOrDefault(x => x.AdminID == id);
            u.FullName = f["nameUser"];
            u.PermissionID = Convert.ToByte(f["permissionUser"]);
            u.Email = f["email"];
            db.SubmitChanges();
            return RedirectToAction("Index","Admin");
        }

        public ActionResult DeleteUser(int id)
        {
            var u = db.AdminAccounts.SingleOrDefault(x => x.AdminID == id);
            db.AdminAccounts.DeleteOnSubmit(u);
            db.SubmitChanges();
            return RedirectToAction("Index", "Admin");
        }
    }
}