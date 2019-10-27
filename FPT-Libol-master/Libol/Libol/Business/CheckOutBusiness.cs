using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Libol.EntityResult;

namespace Libol.Models
{
    public class CheckOutBusiness
    {
        LibolEntities db = new LibolEntities();
        public List<SP_GET_CURRENT_LOANINFOR_Result> SP_GET_CURRENT_LOANINFORs(string strTransactionIDs, string strType)
        {
            List<SP_GET_CURRENT_LOANINFOR_Result> list = db.Database.SqlQuery<SP_GET_CURRENT_LOANINFOR_Result>("SP_GET_CURRENT_LOANINFOR {0}, {1}",
                new object[] { strTransactionIDs, strType }).ToList();
            return list;
        }
    }
}