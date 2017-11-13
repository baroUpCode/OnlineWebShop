using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;
using System.Collections;

namespace OnlineWebShop.Areas.Admin.Controllers
{
    public class DetailsController : Controller
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        // GET: Admin/Details
        public ActionResult OrderDetail(int id) {
            var order = db.Details.SingleOrDefault(x => x.OrderID == id);
            List<Product> pro = new List<Product>();
            var listor = db.Details.Where(x => x.OrderID == id).ToList();
            foreach (var i in listor)
            {
            pro.Add(db.Products.Single(x=>x.ProductID==i.ProductID));
            }
            ViewBag.ProductList = pro;
            return View(order);
        }
        public ActionResult NewsDetail(int id)
        {
            var news = db.News.SingleOrDefault(x => x.NewsID == id);
            return View(news);
        }
        public ActionResult CustomerDetail(int id) {
            var cus = db.Customers.SingleOrDefault(x => x.CustomerID==id);
            return View(cus);
        }
    }
}