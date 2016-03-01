using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Org.Cafh.Courseware.Filters
{
   

    public class LocalizationAttribute : ActionFilterAttribute
    {
        private readonly string _defaultLanguage = "en";

        public LocalizationAttribute(string defaultLanguage)
        {
            _defaultLanguage = defaultLanguage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLanguage;
            if (lang == _defaultLanguage) return;
            try
            {
                Thread.CurrentThread.CurrentCulture =
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            catch (Exception e)
            {
                throw new NotSupportedException($"ERROR: Invalid language code '{lang}'.");
            }
        }
    }
}
