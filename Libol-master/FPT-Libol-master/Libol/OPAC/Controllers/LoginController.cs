using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPAC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: login
        [HttpPost]
        public ActionResult Redirect()
        {
            return RedirectToAction("PatronAfterLoginPage", "InformationPatron");
        }
    }
}