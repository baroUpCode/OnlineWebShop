using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;
using PagedList;
using PagedList.Mvc;
namespace OnlineWebShop.Controllers
{
    public class HomeController : Controller
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();

        /// <summary>
        /// Lay ra 5 san pham moi nhat
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<Product> ListNewProduct(int count)// ham tra ve mot danh sach co thong tin cac san pham nhu ten, gia, ....
        {
            return db.Products.OrderByDescending(x => x.CreatedAt).Take(count).ToList();

        }
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1); //kiểm tra nếu giá trị page = null thì sẽ gán page = 1 bởi vì mặc định người dùng lần đầu vào trang thì họ sẽ ở trang 1.
            var newProduct = ListNewProduct(24);
            return View(newProduct.ToPagedList(pageNum, pageSize));
        }
        public ActionResult GearCatogories()
        {
            var cato = from ca in db.Catogories where ca.RootCatogoryID == 1 select ca;
            return PartialView(cato);
        }
        public ActionResult LaptopCatogories()
        {
            var cato = from ca in db.Catogories where ca.RootCatogoryID == 2 select ca;
            return PartialView(cato);
        }
        /// <summary>
        /// Hiển thị sản phẩm theo loại
        /// </summary>
        /// <returns></returns>
        public ActionResult SortByCatogories(int id, int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            var procat = (from pro in db.Products where pro.CatogoriesID == id select pro).ToList();
            return View(procat.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
            //DetailsModels details = new DetailsModels(id);
            var details = db.Products.SingleOrDefault(x => x.ProductID == id);
            //var details = from a in db.Products
            //             join b in db.Producers on a.ProducerID equals b.ProducerID
            //             select new
            //             {
            //                 productName = a.Name,
            //                 producerName = b.Name,
            //                 productImages=a.ProductImages,
            //                 productPrice=a.Price,
            //                 productWarranty=a.Warranty,
            //                 productStatus=a.Status
            //             };
            if (details == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(details);
        }
        public ActionResult ProducerName(int id)
        {
            var pro = db.Producers.SingleOrDefault(x => x.ProducerID == id);
            return PartialView(pro);
        }
        public ActionResult SortByProducer(int id, int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            var prod = (from nsx in db.Products where nsx.ProducerID == id select nsx).ToList();
            //var prod= db.Products.Select(x=>x.ProducerID==id);
            return View(prod.ToPagedList(pageNum, pageSize));
        }
        [HttpPost]
        public ActionResult SearchingwName(/*int? page,*/FormCollection f)
        {
            //int pageSize = 8;
            //int pageNum = (page ?? 1);
            var product = (from p in db.Products where p.Name.Contains(f["searchString"]) || p.Catogory.CatogoriesName.Contains(f["searchString"]) || p.Producer.Name.Contains(f["searchString"]) select p).ToList();
            ViewBag.Result = f["searchString"].ToString();
            return View(product/*.ToPagedList(pageNum, pageSize*/);
        }
        public ActionResult SearchingwName()
        {
            return View();
        }
   
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(FormCollection f)
        {

            return this.Contacted();
        }
        public ActionResult Contacted()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        /// <summary>
        /// Not yet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NewsDetail(int id )
        {
            var news = db.News.SingleOrDefault(x => x.NewsID == id);
            return View(news);
        }
        public ActionResult News(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            //var prod= db.Products.Select(x=>x.ProducerID==id);
            var news = db.News.OrderByDescending(x => x.CreatedAt).ToList();
            return View(news.ToPagedList(pageNum, pageSize));
        }
        public ActionResult NewsPartial()
        {
            var news = db.News.OrderByDescending(x=>x.CreatedAt).Take(3);
            return PartialView(news);
        }
        public ActionResult RelatedProductCatogory(int id)
        {
            var rand = new Random();
            var product = db.Products.Where(x => x.CatogoriesID == id).OrderByDescending(x=>x.CreatedAt).Take(4);
            return PartialView(product);
        }
        public ActionResult CatogoriesPartial()
        {
            var cat = db.Catogories;
            return PartialView(cat);
        }
    }
}