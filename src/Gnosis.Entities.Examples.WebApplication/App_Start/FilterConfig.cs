using System.Web;
using System.Web.Mvc;

namespace Gnosis.Entities.Examples.WebApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}