using System.Web.Mvc;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Fruits
{
    public class FruitsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Fruits";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Fruits_default",
                "Fruits/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional }
            );
        }
    }
}
