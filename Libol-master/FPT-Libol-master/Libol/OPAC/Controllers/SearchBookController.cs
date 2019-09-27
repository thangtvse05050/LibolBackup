using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPAC.Controllers
{
    public class SearchBookController : Controller
    {
        // GET: SearchBook
        public ActionResult DetailBook()
        {
            return View();
        }

        public ActionResult SearchBook()
        {
            return View();
        }

        public ActionResult AdvancedSearchBook()
        {
            return View();
        }
    }
}