using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineWebShop.Models;

namespace OnlineWebShop.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        dbOnlineWebShopDataContext db = new dbOnlineWebShopDataContext();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            User u = (User)Session["AdminAccount"];
            if (u != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f) {
            var Email = f["email"];
            var pass = f["password"];
            if (String.IsNullOrEmpty(Email))
            {
                ViewData["LoiEmail"] = "Please enter your Email";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["LoiPass"] = "Please enter your Password";
            }
            else
            {
                User u = db.Users.SingleOrDefault(x => x.Email == Email && x.Pass == pass);
                if (u != null)
                {
                    Session["AdminAccount"] = u;
                    Session["AdminMail"] = u.Email;
                    Session["AdminName"] = u.FullName;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Message = "Tài khoản hoặc mật khẩu nhập sai";

            }
            return View();
        }
        public ActionResult AdminLogout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult UserProfile()
        { 

            return View();
        }
        [HttpPost]
        public ActionResult UserProfile(int id,FormCollection f)
        {
            User user = db.Users.SingleOrDefault(x => x.UsersID == id);
            user.FullName = f["userName"];
            
            return RedirectToAction("UserProfile", "Admin");
        }
      

    }
}