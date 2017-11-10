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
            var pro = db.Products.Where(x => x.Deleted == 2).OrderByDescending(x=>x.ProductID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum,pageSize));
        }
        public ActionResult RestoreProduct(int id)
        {
            var pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedProduct");
        }
    }
}