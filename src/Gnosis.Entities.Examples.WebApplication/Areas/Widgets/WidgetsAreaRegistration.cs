using System.Web.Mvc;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Widgets
{
    public class WidgetsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Widgets";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Widgets_default",
                "Widgets/{controller}/{action}/{id}",
                new { action = "Index", controller = "Templates", id = UrlParameter.Optional }
            );
        }
    }
}
