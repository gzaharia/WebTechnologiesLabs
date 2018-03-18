using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Games()
        {
            return View();
        }

        public ActionResult Food()
        {
            return View();
        }

        public ActionResult Music()
        {
            return View();
        }

        public ActionResult Sport()
        {
            return View();
        }

        public ActionResult Technology()
        {
            return View();
        }
    }
}