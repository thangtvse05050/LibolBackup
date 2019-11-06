using Libol.EntityResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    public class SearchPatronBusiness

    {
        LibolEntities db = new LibolEntities();
        public List<FPT_SP_ILL_SEARCH_PATRON_Result> FPT_SP_ILL_SEARCH_PATRONs(string strFullName, string strPatronCode)
        {
            List<FPT_SP_ILL_SEARCH_PATRON_Result> list = db.Database.SqlQuery<FPT_SP_ILL_SEARCH_PATRON_Result>("FPT_SP_ILL_SEARCH_PATRON {0}, {1}",
                new object[] { strFullName, strPatronCode}).ToList();
            return list;
        }
    }
}