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
        //private List<Product> ListCatogoriesProduct(int count)
        //{
        //    return db.Products.OrderByDescending(x => x.CatogoriesID).Take(count).ToList();

        //}
        //private List<Product> ListProducerProduct(int count)
        //{
        //    return db.Products.OrderByDescending(x => x.ProducerID).Take(count).ToList();

        //}
        public ActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1); //kiểm tra nếu giá trị page = null thì sẽ gán page = 1 bởi vì mặc định người dùng lần đầu vào trang thì họ sẽ ở trang 1.
            var newProduct = ListNewProduct(24);
            return View(newProduct.ToPagedList(pageNum,pageSize));
        }
        public ActionResult GearCatogories()
        {
            var cato = from ca in db.Catogories where ca.RootCatogories == 17 select ca;
            return PartialView(cato);
        }
        public ActionResult LaptopCatogories()
        {
            var cato = from ca in db.Catogories where ca.RootCatogories == 18 select ca;
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
            var procat = from pro in db.Products where pro.CatogoriesID==id select pro;
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
        public ActionResult SortByProducer(int id,int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            var prod = from nsx in db.Products where nsx.ProducerID == id select nsx; 
            //var prod= db.Products.Select(x=>x.ProducerID==id);
            return View(prod.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SearchingwName(string searchString)
        {
            //var product = from sp in db.Products select sp.Name;
            //var cat = db.Catogories.Where(x => x.CatogoriesName.Contains(searchString));
            //var producer = db.Producers.Where(x => x.Name.Contains(searchString));
            //var links = links.Where(l => l.First() == '/' || l.First() == '//').ToList();
            var product = db.Products.Where(x => x.Name.Contains(searchString));
            return View(product.ToList());
        }
        //public ActionResult SearchingwProducer(string searchString)
        //{
        //    var product = from p in db.Producers where p.Name.Contains(searchString) select p.Products.Where(X => X.ProducerID == p.ProducerID);
        //    ViewBag.Message = searchString.ToString();
        //    return View(product.ToList());
        //}
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
        public ActionResult News(int id )
        {
            var news = db.News.SingleOrDefault(x => x.NewsID == id);
            return View(news);
        }
        public ActionResult NewsPartial()
        {
            var news = from s in db.News select s;
            return PartialView(news);
        }
        public ActionResult RelatedProductCatogory(int id)
        {
            var product = db.Products.Where(x => x.CatogoriesID == id).Take(6);
            return PartialView(product);
        }
    }
}