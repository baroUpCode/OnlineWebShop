using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;
using PagedList;

namespace OnlineWebShop.Areas.Admin.Controllers
{
    public class GarbageController : Controller
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        // GET: Admin/Garbage
        public ActionResult DeletedProduct(int? page)
        {
            var pro = db.Products.Where(x => x.Deleted == 2).OrderByDescending(x => x.ProductID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        public ActionResult RestoreProduct(int id)
        {
            var pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedProduct");
        }
        public ActionResult DeletedCatogories(int? page)
        {
            var pro = db.Catogories.Where(x => x.Deleted == 2).OrderByDescending(x => x.CatogoriesID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        public ActionResult RestoreCatogories(int id)
        {
            var pro = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedCatogories");
        }
        public ActionResult DeletedProducer(int? page)
        {
            var pro = db.Producers.Where(x => x.Deleted == 2).OrderByDescending(x => x.ProducerID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        public ActionResult RestoreProducer(int id)
        {
            var pro = db.Producers.SingleOrDefault(x => x.ProducerID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedProducer");
        }
        public ActionResult DeletedOrder(int? page)
        {
            var pro = db.Orders.Where(x => x.Deleted == 2).OrderByDescending(x => x.OrderID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        public ActionResult RestoreOrder(int id)
        {
            var pro = db.Orders.SingleOrDefault(x => x.OrderID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedOrder");
        }
        public ActionResult DeletedNews(int? page)
        {
            var pro = db.News.Where(x => x.Deleted == 2).OrderByDescending(x => x.NewsID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        public ActionResult RestoreNews(int id)
        {
            var pro = db.News.SingleOrDefault(x => x.NewsID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedNews");
        }
        public ActionResult DeletedCustomers(int? page)
        {
            var pro = db.Customers.Where(x => x.Deleted == 2).OrderByDescending(x => x.CustomerID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        public ActionResult RestoreCustomers(int id)
        {
            var pro = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedCustomers");
        }
        public ActionResult DeleteOrders(int id )
        {
            var order = db.Orders.SingleOrDefault(x => x.OrderID == id);
            var detail = db.Details.Where(x => x.OrderID == order.OrderID).ToList();
            foreach (var item in detail)
            {
                db.Details.DeleteOnSubmit(item);
            }
            db.Orders.DeleteOnSubmit(order);
            db.SubmitChanges();
            return RedirectToAction("Orders", "Modules");
        }
    }
}