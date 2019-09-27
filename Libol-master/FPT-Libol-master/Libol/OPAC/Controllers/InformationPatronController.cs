using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPAC.Controllers
{
    public class InformationPatronController : Controller
    {
        // GET: InformationPatron
        public ActionResult PatronPage()
        {
            return View();
        }

        public ActionResult PatronAfterLoginPage()
        {
            return View();
        }

        public ActionResult BookBorrowingPage()
        {
            return View();
        }
    }
}