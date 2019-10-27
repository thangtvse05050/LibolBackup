using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPAC.Models;
using System.Net;
using System.Net.Mail;
using OPAC.Dao;

namespace OPAC.Controllers
{
    public class LoginController : Controller
    {
        private OpacEntities db = new OpacEntities();
        private PatronDao dao = new PatronDao();

        // GET: Login
        public ActionResult Login()
        {
            /* if (Session["ID"] != null)
            {
                return RedirectToAction("Home", "Home");
            } */
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var checkUser = db.SP_OPAC_CHECK_PATRON_CARD(username, password).FirstOrDefault();
            if (checkUser != null)
            {
                int userId = checkUser.ID;
                Session["ID"] = userId;
                Session["Info"] = checkUser;
                Session["OnHolding"] = checkUser.Code;

                return RedirectToAction("PatronAfterLoginPage", "InformationPatron");
            }
            else
            {
                ViewData["Notification"] = "Tên đăng nhập/mật khẩu không đúng!";
                return View();
            }

        }

        public ActionResult Logout()
        {
            Session.Remove("ID");
            Session.Remove("Info");
            Session.Remove("OnHolding");

            return RedirectToAction("Home", "Home");
        }

        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var fromAddress = new MailAddress("santintt197@gmail.com", "FPT Library");
                var toAddress = new MailAddress(email, "To Name");
                string newPassword = dao.RandomPassword();
                dao.UpdatePasswordByEmail(newPassword, email);
                const string fromPassword = "trongthang197";
                const string subject = "Đặt lại mật khẩu mới - Thư viện FPT";
                string body = "Mật khẩu mới của bạn là: <b>" + newPassword + "</b><br/><br/>" +
                              "Vui lòng đổi mật khẩu sau khi đăng nhập bằng mật khẩu trên.<br/><br/>" +
                              "Trân trọng!.<br/><br/>" +
                              "Thư viện trường đại học FPT.<br/><br/>" +
                              "---------------------<br/>" +
                              "<i>Đây là mail tự động, xin vui lòng không reply.</i>";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                })
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
            catch (FormatException)
            {
                TempData["MailError"] = "Địa chỉ email không đúng!";
                return RedirectToAction("Login");
            }

            TempData["ChangePassword"] = "Mật khẩu mới đã được reset!";
            return RedirectToAction("Login");
        }
    }
}