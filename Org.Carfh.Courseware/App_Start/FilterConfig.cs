using System.Web;
using System.Web.Mvc;
using Org.Carfh.Courseware.Filters;

namespace Org.Carfh.Courseware
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());
            filters.Add(new LocalizationAttribute("en"), 0);
        }
    }
}
