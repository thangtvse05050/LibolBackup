using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Libol.SupportClass
{
    public class AuthAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public int ModuleID { get; set; }
        public string RightID { get; set; }        

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["UserID"])))
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Index", "Login");
                filterContext.Result = new RedirectResult(url);
            }
            else
            {
                List<Int32> ModuleIDs = (List<Int32>)filterContext.HttpContext.Session["ModuleIDs"];
                List<Int32> RightIDs = (List<Int32>)filterContext.HttpContext.Session["RightIDs"];
                ModuleIDs.Add(0);
                RightIDs.Add(0);
                var Right = RightID.Split(',');
                bool Check = false;
                foreach(var r in RightID.Split(','))
                {
                    if (ModuleIDs.Contains(ModuleID) && RightIDs.Contains(Int32.Parse(r)))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)
                {
                    filterContext.Result = new ViewResult() { ViewName = "Permisssion" };
                }
            }
        }

    }
}