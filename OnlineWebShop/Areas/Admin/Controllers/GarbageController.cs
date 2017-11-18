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
        /// <summary>
        /// Xoa dữ liệu khách hàng khỏi danh sách khách hàng (set Deleted==2)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DeletedProduct(int? page)
        {
            var pro = db.Products.Where(x => x.Deleted == 2).OrderByDescending(x => x.ProductID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Phục hồi dữ liệu khách hàng (set Deleted ==1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RestoreProduct(int id)
        {
            var pro = db.Products.SingleOrDefault(x => x.ProductID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedProduct");
        }
        /// <summary>
        /// Xoa dữ liệu loại sản phẩm khỏi danh sách loại (set Deleted==2)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DeletedCatogories(int? page)
        {
            var pro = db.Catogories.Where(x => x.Deleted == 2).OrderByDescending(x => x.CatogoriesID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Phục hồi dữ liệu loại sản phẩm (set Deleted==2)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RestoreCatogories(int id)
        {
            var pro = db.Catogories.SingleOrDefault(x => x.CatogoriesID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedCatogories");
        }
        /// <summary>
        /// Xóa dữ liệu nhà sản xuất khỏi danh sách nhà sản xuất (set Deleted==2)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DeletedProducer(int? page)
        {
            var pro = db.Producers.Where(x => x.Deleted == 2).OrderByDescending(x => x.ProducerID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Phục hồi danh dữ liệu nhà sản xuất (set Deleted==1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RestoreProducer(int id)
        {
            var pro = db.Producers.SingleOrDefault(x => x.ProducerID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedProducer");
        }
        /// <summary>
        /// Xóa dữ liệu hóa đơn khỏi bảng danh sách hóa đơn (set Deleted==2)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DeletedOrder(int? page)
        {
            var pro = db.Orders.Where(x => x.Deleted == 2).OrderByDescending(x => x.OrderID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Phục hồi dữ liệu hóa đơn đã xóa (set Deleted==1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RestoreOrder(int id)
        {
            var pro = db.Orders.SingleOrDefault(x => x.OrderID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedOrder");
        }
        /// <summary>
        /// Xóa dữ liệu Tin tức khỏi danh sách tin túc (set Deleted==2)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DeletedNews(int? page)
        {
            var pro = db.News.Where(x => x.Deleted == 2).OrderByDescending(x => x.NewsID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Phục hồi dữ liệu Tin tức đã bị xóa (set Deleted==1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RestoreNews(int id)
        {
            var pro = db.News.SingleOrDefault(x => x.NewsID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedNews");
        }
        /// <summary>
        /// Xóa dữ liệu khách hàng khỏi bảng danh sách khách hàng (set Deleted==2)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult DeletedCustomers(int? page)
        {
            var pro = db.Customers.Where(x => x.Deleted == 2).OrderByDescending(x => x.CustomerID);
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(pro.ToPagedList(pageNum, pageSize));
        }
        /// <summary>
        /// Phục hồi dữ liệu khách hàng đã xóa (set Deleted==1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RestoreCustomers(int id)
        {
            var pro = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            pro.Deleted = 1;
            db.SubmitChanges();
            return RedirectToAction("DeletedCustomers");
        }
        /// <summary>
        /// Xóa hoàn toàn Hóa đơn khỏi cơ sở dữ liệu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Xóa hoàn toàn Khách hàng khỏi csdl
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ConfirmDeleteCustomer(int id)
        {
            var cus = db.Customers.SingleOrDefault(x => x.CustomerID == id);
            db.Customers.DeleteOnSubmit(cus);
            db.SubmitChanges();
            return RedirectToAction("DeletedCustomers", "Garbage");
        }
        public ActionResult ConfirmDeleteNews(int id)
        {
            var news = db.News.SingleOrDefault(x => x.NewsID == id);
            db.News.DeleteOnSubmit(news);
            db.SubmitChanges();
            return RedirectToAction("DeletedNews");
        }
        /// <summary>
        /// Xóa một Sản phẩm khỏi danh sách sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ConfirmDeleteProduct(int id)
        {
            return RedirectToAction("DeletedProduct");
        }

    }
}