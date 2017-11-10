using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;
using System.IdentityModel.Tokens;

namespace OnlineWebShop.Controllers
{
    public class UserController : Controller
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Lay thong tin tu View gan vao para customer,luu thong tin customer vao models , db.InsertOrSubmit(customer) -> cap nhat lai DB tu models db.SubmitChanges()
        /// </summary>
        /// <param name="form"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            if (DateTime.Parse(form["BirthDay"]) > DateTime.Now)
            {
                ViewBag.ErrorDate = "Ngày sinh không hợp lệ";
                ViewBag.Error = "Thông tin đăng ký không hợp lệ";
                return this.Register();
            }
            else
            {
                Customer customer = new Customer();
                var name = form["Name"];
                var birth = form["BirthDay"];
                var email = form["Email"];
                var phone = form["Phone"];
                var address = form["Address"];
                var pass = form["Pass"];
                customer.FullName = name;
                customer.Email = email;
                customer.Deleted = 1;
                customer.BirthDay = DateTime.Parse(birth);
                customer.Phone = phone;
                customer.Address = address;
                customer.Pass = pass;
                db.Customers.InsertOnSubmit(customer);// Luu thong tin vua duoc nhap tu view vao Models
                db.SubmitChanges();// Cap nhat lai DB tu Models
                ViewBag.Succcess = "Đăng ký thành công";
                return View();
            }
        }
        //public ActionResult Login()
        //{
        //    return PartialView();
        //}
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            var Email = form["email"];
            var Pass = form["pass"];
            //if (String.IsNullOrEmpty(Email))
            //{
            //    ViewData["Error1"] = "Vui lòng nhập Email";
            //}
            //else if (String.IsNullOrEmpty(Pass))
            //{
            //    ViewData["Error2"] = "Vui lòng nhập mật khẩu";
            //}
            //else
            //{
            Customer cus = db.Customers.SingleOrDefault(x => x.Email == Email && x.Pass == Pass);
            if (cus != null)
            {
                Session["AccountCustomer"] = cus;
                Session["AccountName"] = cus.Email;
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Register", "User");
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}