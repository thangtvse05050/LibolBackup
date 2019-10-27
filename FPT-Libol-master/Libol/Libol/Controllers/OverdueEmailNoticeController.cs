using Libol.EntityResult;
using Libol.Models;
using Libol.SupportClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Libol.Controllers
{
    public class OverdueEmailNoticeController : Controller
    {

        private LibolEntities db = new LibolEntities();
        // GET: OverdueNotice
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendEmail(string toEmail)
        {
            string name = "";
            string messagealarm = "";
            string contenttable = "";
            string alarmtitle = "";

            // mail to all patron have due date tomorrow
            List<int> list1 = FPT_SP_GETINFOR_EMAIL("", -1, -1).Select(a => a.PatronID).Distinct().ToList();
            foreach(var i in list1)
            {
                name = "";
                alarmtitle = "";
                messagealarm = "";
                contenttable = "";
                List<FPT_SP_GETINFOR_EMAIL_Result> list = FPT_SP_GETINFOR_EMAIL("", -1, i);
                foreach (var content in list)
                {
                    name = content.Name;
                    alarmtitle = "Overdue Notice";
                    messagealarm = "have (has) due date tomorrow";
                    contenttable = contenttable + "<tr> <td> <span> "+new FormatHoldingTitle().OnFormatHoldingTitle(content.MainTitle)+ "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CopyNumber + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CheckInDate + "</span> </td> </tr>";
                }
                //Email
                var message = EmailTemplate("NoticeEmail", alarmtitle, name, messagealarm, contenttable);
                await SendEmailAsync(toEmail, "Library's Overdue notice", message);
            }

            // mail to all patron has borrowed book(s) are overdue 1 day
            List<int> list2 = FPT_SP_GETINFOR_EMAIL("", 1, -1).Select(a => a.PatronID).Distinct().ToList();
            foreach (var i in list2)
            {
                name = "";
                alarmtitle = "";
                messagealarm = "";
                contenttable = "";
                List<FPT_SP_GETINFOR_EMAIL_Result> list = FPT_SP_GETINFOR_EMAIL("", 1, i);
                foreach (var content in list)
                {
                    name = content.Name;
                    alarmtitle = "Overdue Notice 1(st)";
                    messagealarm = "are overdue 1 day";
                    contenttable = contenttable + "<tr> <td> <span> " + new FormatHoldingTitle().OnFormatHoldingTitle(content.MainTitle) + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CopyNumber + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CheckInDate + "</span> </td> </tr>";
                }
                //Email
                var message = EmailTemplate("NoticeEmail", alarmtitle, name, messagealarm, contenttable);
                await SendEmailAsync(toEmail, "Library's Overdue notice 1(st)", message);
            }

            // mail to all patron has borrowed book(s) are overdue 7 day
            List<int> list3 = FPT_SP_GETINFOR_EMAIL("", 7, -1).Select(a => a.PatronID).Distinct().ToList();
            foreach (var i in list3)
            {
                name = "";
                alarmtitle = "";
                messagealarm = "";
                contenttable = "";
                List<FPT_SP_GETINFOR_EMAIL_Result> list = FPT_SP_GETINFOR_EMAIL("", 7, i);
                foreach (var content in list)
                {
                    name = content.Name;
                    alarmtitle = "Overdue Notice 2(nd)";
                    messagealarm = "are overdue 7 days";
                    contenttable = contenttable + "<tr> <td> <span> " + new FormatHoldingTitle().OnFormatHoldingTitle(content.MainTitle) + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CopyNumber + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CheckInDate + "</span> </td> </tr>";
                }
                //Email
                var message = EmailTemplate("NoticeEmail", alarmtitle, name, messagealarm, contenttable);
                await SendEmailAsync(toEmail, "Library's Overdue notice 2(nd)", message);
            }

            // mail to all patron has borrowed book(s) are overdue 14 day
            List<int> list4 = FPT_SP_GETINFOR_EMAIL("", 14, -1).Select(a => a.PatronID).Distinct().ToList();
            foreach (var i in list4)
            {
                name = "";
                alarmtitle = "";
                messagealarm = "";
                contenttable = "";
                List<FPT_SP_GETINFOR_EMAIL_Result> list = FPT_SP_GETINFOR_EMAIL("", 14, i);
                foreach (var content in list)
                {
                    name = content.Name;
                    alarmtitle = "Overdue Notice 3(rd)";
                    messagealarm = "are overdue 14 days";
                    contenttable = contenttable + "<tr> <td> <span> " + new FormatHoldingTitle().OnFormatHoldingTitle(content.MainTitle) + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CopyNumber + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CheckInDate + "</span> </td> </tr>";
                }
                //Email
                var message = EmailTemplate("NoticeEmail", alarmtitle, name, messagealarm, contenttable);
                await SendEmailAsync(toEmail, "Library's Overdue notice 3(rd)", message);
            }

            // mail to all patron has borrowed book(s) are overdue 21 day
            List<int> list5 = FPT_SP_GETINFOR_EMAIL("", 21, -1).Select(a => a.PatronID).Distinct().ToList();
            foreach (var i in list5)
            {
                name = "";
                alarmtitle = "";
                messagealarm = "";
                contenttable = "";
                List<FPT_SP_GETINFOR_EMAIL_Result> list = FPT_SP_GETINFOR_EMAIL("", 21, i);
                foreach (var content in list)
                {
                    name = content.Name;
                    alarmtitle = "Overdue Notice 4(th)";
                    messagealarm = "are overdue 21 days";
                    contenttable = contenttable + "<tr> <td> <span> " + new FormatHoldingTitle().OnFormatHoldingTitle(content.MainTitle) + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CopyNumber + "</span> </td>";
                    contenttable = contenttable + "<td> <span> " + content.CheckInDate + "</span> </td> </tr>";
                }
                //Email
                var message = EmailTemplate("NoticeEmail", alarmtitle, name, messagealarm, contenttable);
                await SendEmailAsync(toEmail, "Library's Overdue notice 4(th)", message);
            }

            //End
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        
        public List<FPT_SP_GETINFOR_EMAIL_Result> FPT_SP_GETINFOR_EMAIL(string libIDs, int intTime, int PatronID)
        {
            List<FPT_SP_GETINFOR_EMAIL_Result> list;
            if(PatronID != -1)
            {
                list = db.Database.SqlQuery<FPT_SP_GETINFOR_EMAIL_Result>("FPT_SP_GETINFOR_EMAIL {0}, {1}",
                new object[] { libIDs, intTime }).Where(a => a.PatronID == PatronID).ToList();
            }
            else
            {
                list = db.Database.SqlQuery<FPT_SP_GETINFOR_EMAIL_Result>("FPT_SP_GETINFOR_EMAIL {0}, {1}",
                new object[] { libIDs, intTime }).ToList();
            }
            return list;
        }

        public string EmailTemplate(string template,string title, string name,string messagealarm, string content)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Content/EmailTemplate/") + template + ".cshtml");
            body = body.Replace("|Title-Placeholder|", title);
            body = body.Replace("|Name-Placeholder|", name);
            body = body.Replace("|Time-Placeholder|", messagealarm);
            body = body.Replace("|ContentTable-Placeholder|", content);
            return body;
        }

        public async static Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                //step1 set setting public to email account
                
                //step2 send mail
                //var _email = "trinhlv26031997@gmail.com";
                //var _pass = "Iphone1997";
                var _email = "thainhatdatdoanhtrinh@gmail.com";
                var _pass = "Project123";
                var _name = "FPT University Library";
                MailMessage myMessage = new MailMessage();
                myMessage.To.Add(email);
                myMessage.From = new MailAddress(_email, _name);
                myMessage.Subject = subject;
                myMessage.Body = message;
                myMessage.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _pass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.SendCompleted += (s, e) =>
                    {
                        smtp.Dispose();
                    };
                    await smtp.SendMailAsync(myMessage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}