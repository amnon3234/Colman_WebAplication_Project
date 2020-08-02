using System.Web;
using System.Web.Mvc;

namespace Colman_MVC_WebApplication_Project
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
