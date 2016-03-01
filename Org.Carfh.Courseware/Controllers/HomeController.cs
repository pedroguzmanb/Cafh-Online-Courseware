using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lang;
using Org.Cafh.Courseware.Models.Courseware;
using Org.Cafh.Courseware.Models.Repositories;

namespace Org.Cafh.Courseware.Controllers
{
    public class HomeController : Controller
    {

        

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION INDEX                                                                                                                     //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Directory()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      
    }
}