using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Fruits/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
