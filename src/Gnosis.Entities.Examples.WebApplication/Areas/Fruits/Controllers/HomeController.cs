using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gnosis.Entities.Attributes;
using Gnosis.Entities.Examples.Fruits;
using Gnosis.Entities.Testing.Data;
using Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Models;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Fruits.Controllers
{
    public class HomeController : Controller
    {
        protected FruitManager manager;

        private Guid BasketId = Guid.Parse("780d93bd-1ce3-4bd1-90db-c1520ff4f95f");

        protected class MemoryFruitDataManager : MemoryEntityDataManager, IFruitDataManager
        {
            public Dictionary<string, int> GetEntityTypeCounts()
            {
                throw new NotImplementedException();
            }
        }
        
        public class Basket : Entity
        {
            [EntityField]
            public IEnumerable<Fruit> Fruit { get; protected set; }
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBanana(BananaCreateRequestModel request)
        {
            BananaCreateResponseModel response = new BananaCreateResponseModel();

            try
            {
                response.Revision = manager.CreateBanana(request);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            
            return Json(response);
        }

        public ActionResult List()
        {
            return Json(manager.LoadBasket<Basket>(BasketId));
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            manager = new FruitManager(new MemoryFruitDataManager());
        }

    }
}
