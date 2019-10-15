using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCrypt;
using Libol.Models;

namespace Libol.Controllers
{
    public class LoginController : Controller
    {
        private LibolEntities db = new LibolEntities();




        // GET: Login
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            string passEncrypt = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password, "pl");
            var checkUser = db.SP_SYS_USER_LOGIN(username, passEncrypt).ToList();
            if (checkUser.Count > 0)
            {
                int UserID = checkUser[0].ID;
                Session["UserID"] = UserID;
                GetPermission(UserID);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Notification"] = "Tên đăng nhập/mật khẩu không đúng!";
                return View();
            }

        }

        [HttpPost]
        public JsonResult SignInWithGoogle(string email)
        {
            SYS_USER_GOOGLE_ACCOUNT acc = db.SYS_USER_GOOGLE_ACCOUNT.Find(email);
            if (acc != null)
            {
                int UserID = acc.ID;
                Session["UserID"] = UserID;
                GetPermission(UserID);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json("EmailNotExist", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult Logout()
        {
            Session.Abandon();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public void GetPermission(int UserID)
        {
            Session["UserID"] = UserID;

            List<Int32> RightIDs = new List<Int32>();
            List<Int32> ModuleIDs = new List<Int32>();
            var userRight = db.FPT_SYS_USER_RIGHT_DETAIL.Where(a => a.UserID == UserID).ToList();
            foreach (var right in userRight)
            {
                RightIDs.Add(right.RightID);
            }
            var user = db.SYS_USER.Where(a => a.ID == UserID).First();
            Session["FullName"] = user.Name;
            if (user.CatModule)
            {
                ModuleIDs.Add(1);
            }
            if (user.PatModule)
            {
                ModuleIDs.Add(2);
            }
            if (user.CirModule)
            {
                ModuleIDs.Add(3);
            }
            if (user.AcqModule)
            {
                ModuleIDs.Add(4);
            }
            if (user.SerModule)
            {
                ModuleIDs.Add(5);
            }
            if (user.ILLModule)
            {
                ModuleIDs.Add(8);
            }
            if (user.DelModule)
            {
                ModuleIDs.Add(9);
            }
            if (user.AdmModule)
            {
                ModuleIDs.Add(6);
            }
            Session["RightIDs"] = RightIDs;
            Session["ModuleIDs"] = ModuleIDs;
        }
    }
}