using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Models;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Controllers
{
    public class TemplateController : Controller
    {
        public ActionResult BananaCreate()
        {
            BananaCreateRequestModel model = new BananaCreateRequestModel();

            return View(model);
        }

    }
}
