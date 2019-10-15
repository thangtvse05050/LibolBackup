using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Libol.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        public void Log(string Path, string Error, string Message, string Track)
        {
            var ex = Server.GetLastError();
            //log the error!
            log.Info("");
            log.Info("");
            log.Info("------------------------------------------------------------------------------------------------------");
            log.Info("Time: " + DateTime.Now.ToUniversalTime().AddHours(7).ToString("dd/MM/yyyy HH:mm:ss"));
            log.Info(Path);
            log.Info(Error);
            log.Info(Message);
            log.Info(Track);

            System.IO.StreamWriter Writer = System.IO.File.AppendText(@"D:\home\error.log");
            Writer.WriteLine("");
            Writer.WriteLine("------------------------------------------------------------------------------------------------------");
            Writer.WriteLine("Time: " + DateTime.Now.ToUniversalTime().AddHours(7).ToString("dd/MM/yyyy HH:mm:ss"));
            Writer.WriteLine(Path);
            Writer.WriteLine(Error);
            Writer.WriteLine(Message);
            Writer.WriteLine(Track);
            Writer.Close();
        }
    }
}