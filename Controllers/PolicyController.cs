using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWeb.Controllers
{
    public class PolicyController : Controller
    {
        // GET: Policy
        public ActionResult ThuDoi()
        {
            return View();
        }
        public ActionResult BaoHanh()
        {
            return View();
        }
        public ActionResult GiaoHang()
        {
            return View();
        }
        public ActionResult QA()
        {
            return View();
        }
    }
}