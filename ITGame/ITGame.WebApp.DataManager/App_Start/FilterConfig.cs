using System.Web;
using System.Web.Mvc;

namespace ITGame.WebApp.DataManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
