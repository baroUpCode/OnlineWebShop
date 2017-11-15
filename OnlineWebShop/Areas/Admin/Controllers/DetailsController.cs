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
            var order = db.Orders.SingleOrDefault(x=>x.OrderID==id);
            List<Product> pro = new List<Product>();
            foreach (var item in order.Details)
            {
                var listor = db.Products.Single(x => x.ProductID == item.ProductID);
                pro.Add(listor);
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