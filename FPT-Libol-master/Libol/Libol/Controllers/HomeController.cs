using Libol.SupportClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Libol.Controllers
{
    public class HomeController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [AuthAttribute(ModuleID = 0, RightID = "0")]
        public ActionResult Index()
        {
            return View();
        }
    }
}