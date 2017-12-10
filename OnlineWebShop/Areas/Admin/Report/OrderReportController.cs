using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineWebShop.Areas.Admin.Report
{
    public class OrderReportController : Controller
    {
        // GET: Admin/OrderReport
        public ActionResult OrderReport()
        {
            return View(new OrderReport());
        }
    }
}