using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CallMedical.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HealthScreening()
        {
            return View();
        }

        public ActionResult FindDoctor()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TestError()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
