using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;

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
        public ActionResult Products()
        {
            return View(db.Products.ToList());
        }
        public ActionResult EditProduct(int id)
        {
            Product pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            return View(pro);
        }
        [HttpPost]
        public ActionResult EditProduct(int id,FormCollection f)
        {
            Product pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            if (pro != null)
            {
                pro.Name = f["productName"];
                pro.ProductImages = f["productImages"].ToString();
                pro.Quantity = Convert.ToInt32(f["productQuantity"]);
                pro.Warranty = Convert.ToInt32(f["productWarranty"]);
                pro.Price = Convert.ToDecimal(f["productPrice"]);
                pro.CatogoriesID = Convert.ToInt32(f["productCatogories"]);
                pro.ProducerID = Convert.ToInt32(f["productProducer"]);
                db.Products.InsertOnSubmit(pro);
                db.SubmitChanges();
                return RedirectToAction("EditProduct", "Modules");
            }
            else
                return RedirectToAction("Product", "Modules");
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
            return View();
        }
        [HttpPost]
        public ActionResult InsertProduct(FormCollection f)
        {
            Product pro = new Product();
            pro.Name = f["proName"];
            pro.ProductImages = f["proImage"];
            pro.Quantity = Convert.ToInt32(f["proQuantity"]);
            pro.Warranty = Convert.ToInt32(f["proWarranty"]);
            pro.Description = f["proDes"];
            pro.CatogoriesID = Convert.ToInt32(f["proCat"]);
            pro.Price = Convert.ToDecimal(f["proPrice"]);
            db.Products.InsertOnSubmit(pro);
            db.SubmitChanges();
            return View();
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
            return View(cat);
        }
        [HttpPost]
        public ActionResult EditCatogories(int id, FormCollection f)
        {
            
            Catogory cat = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            cat.CatogoriesName = f["catName"];
            cat.RootCatogories = Convert.ToInt32(f["catRoot"]);
            db.Catogories.InsertOnSubmit(cat);
            db.SubmitChanges();
            return RedirectToAction("Catogories","Modules");
        }
        public ActionResult InsertCatogories()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertCatogories(FormCollection f)
        {
            Catogory cat = new Catogory();
            cat.CatogoriesName = f["catName"];
            cat.RootCatogories = Convert.ToInt32(f["catRoot"]);
            db.Catogories.InsertOnSubmit(cat);
            db.SubmitChanges();
            return RedirectToAction("InsertCatogories", "Modules");
        }
        public ActionResult DeleteCatogories(int id)
        {
            Catogory cat = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            db.Catogories.DeleteOnSubmit(cat);
            db.SubmitChanges();
            return RedirectToAction("Catogories", "Modules");
        }
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
                pro.Name = f["proName"];
                db.Producers.InsertOnSubmit(pro);
                db.SubmitChanges();
                return RedirectToAction("EditProducer", "Modules");
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
        /// <summary>
        /// Module News
        /// Edit-Delete-Insert 
        /// </summary>
        /// <returns></returns>
        public ActionResult News()
        {
            return View(db.News.ToList());
        }
        public ActionResult EditNews(int id)
        {
            New pro = db.News.SingleOrDefault(x => x.NewsID == id);
            return View(pro);
        }
        [HttpPost]
        public ActionResult EditNews(int id, FormCollection f)
        {
            New news = db.News.SingleOrDefault(x => x.NewsID == id);
            if (news != null)
            {
                news.Content = f["newContent"];
                news.Images = f["newsImage"].ToString();
                news.CatogoriesID = Convert.ToInt32(f["newCatID"]);
                db.News.InsertOnSubmit(news);
                db.SubmitChanges();
                return RedirectToAction("EditCustomer", "Modules");
            }
            else
                return RedirectToAction("Customer", "Modules");
        }
        public ActionResult DeleteNews(int id)
        {
            New news = db.News.SingleOrDefault(x => x.NewsID == id);
            db.News.DeleteOnSubmit(news);
            db.SubmitChanges();
            return RedirectToAction("Products", "Modules");

        }
        public ActionResult InsertNews()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertNews(FormCollection f)
        {
            New news = new New();
            news.Content = f["newContent"];
            news.Images = f["newsImage"].ToString();
            news.CatogoriesID = Convert.ToInt32(f["newCatID"]);
            db.News.InsertOnSubmit(news);
            db.SubmitChanges();
            return this.InsertNews();
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
                cus.FullName = f["cusName"];
                cus.Address = f["cusAddress"];
                cus.BirthDay = Convert.ToDateTime(f["cusBirthday"]);
                cus.Phone = Convert.ToInt32(f["cusPhone"]);
                db.Customers.InsertOnSubmit(cus);
                db.SubmitChanges();
                return RedirectToAction("Customer", "Modules");
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
            return View();
        }
        public ActionResult InsertCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertCustomer(FormCollection f)
        {
            Customer cus = new Customer();
            cus.FullName = f["cusName"];
            cus.Email = f["cusEmail"];
            cus.Phone = Convert.ToInt32(f["cusPhone"]);
            cus.Pass = f["cusPass"];
            cus.Province = f["cusProvince"];
            db.Customers.InsertOnSubmit(cus);
            db.SubmitChanges();
            return this.InsertCustomer();
        }
        /// <summary>
        /// Modules Sliders
        /// </summary>
        /// <returns></returns>
        public ActionResult Sliders()
        {
            return View(db.Sliders.ToList());
        }
        public ActionResult EditSlider(int id )
        {
            Slider sli = db.Sliders.SingleOrDefault(x => x.ImagesID == id);
            return View(sli);
        }
        [HttpPost]
        public ActionResult EditSlider(FormCollection f,int id)
        {
            Slider sli = db.Sliders.SingleOrDefault(x => x.ImagesID == id);
            sli.ImagesLink = f["imageLink"];
            sli.URL = f["imageURL"];
            db.Sliders.InsertOnSubmit(sli);
            db.SubmitChanges();
            return RedirectToAction("Slider", "Modules");
        }
        public ActionResult DeleteSlider(int id)
        {
            Slider sli = db.Sliders.SingleOrDefault(x=>x.ImagesID==id);
            db.Sliders.DeleteOnSubmit(sli);
            db.SubmitChanges();
            return View();
        }
        public ActionResult InsertSlider()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertSlider(FormCollection f)
        {
            Slider sli = new Slider();
            sli.ImagesLink = f["imageLink"];
            sli.URL = f["imageUrl"];
            db.Sliders.InsertOnSubmit(sli);
            db.SubmitChanges();
            return this.InsertSlider();
        }
    }
}