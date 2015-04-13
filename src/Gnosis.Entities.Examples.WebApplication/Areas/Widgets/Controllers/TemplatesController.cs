using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gnosis.Entities.Examples.WebApplication.Areas.Widgets.Models;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Widgets.Controllers
{
    public class TemplatesController : Controller
    {
        //
        // GET: /Widgets/Tempalte/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            WidgetCreateRequestModel model = new WidgetCreateRequestModel();
            
            return View(model);
        }
    }
}
