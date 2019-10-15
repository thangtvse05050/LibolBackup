using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Libol
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            //log the error!
            log.Info("");
            log.Info("------------------------------------------------------------------------------------------------------");            
            log.Error(ex.Message);
            log.Error(ex.StackTrace);
            log.Error(ex.TargetSite);

            System.IO.StreamWriter Writer = System.IO.File.AppendText(@"D:\home\error.log");
            Writer.WriteLine("Time: " + DateTime.Now.ToUniversalTime().AddHours(7).ToString("dd/MM/yyyy HH:mm:ss"));
            Writer.WriteLine(ex.Message);
            Writer.WriteLine(ex.StackTrace);
            Writer.WriteLine(ex.TargetSite.ToString());
            Writer.Close();
        }
    }
}
