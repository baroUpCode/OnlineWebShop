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
            AdminAccount u = (AdminAccount)Session["AdminAccount"];
            if (u != null)
            {
            var order = db.Orders.SingleOrDefault(x=>x.OrderID==id);
            List<Detail> pro = new List<Detail>();
            foreach (var item in order.Details)
            {
                var listor = db.Details.Single(x =>(x.ProductID == item.ProductID) &&( x.OrderID==id));
                pro.Add(listor);
            }
            ViewBag.ProductList = pro;
            return View(order);
            }
            else
                return RedirectToAction("Login", "Admin");
        }
        public ActionResult NewsDetail(int id)
        {
            AdminAccount u = (AdminAccount)Session["AdminAccount"];
            if (u != null)
            {
                var news = db.News.SingleOrDefault(x => x.NewsID == id);
                return View(news);
            }
            else
                return RedirectToAction("Login", "Admin");

        }
        public ActionResult CustomerDetail(int id) {
            AdminAccount u = (AdminAccount)Session["AdminAccount"];
            if (u != null)
            {
                var cus = db.Customers.SingleOrDefault(x => x.CustomerID == id);
                return View(cus);
            }
            else
                return RedirectToAction("Login", "Admin");

        }
    }
}