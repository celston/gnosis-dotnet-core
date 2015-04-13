using System;
using System.Web.Mvc;

using Gnosis.Entities.Examples.WebApplication.Areas.Login.Models;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Login.Controllers
{
    public class TemplatesController : Controller
    {
        public ActionResult Index()
        {
            LoginRequestModel model = new LoginRequestModel();
            
            return View(model);
        }

    }
}
