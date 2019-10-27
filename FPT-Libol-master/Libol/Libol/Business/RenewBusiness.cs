using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Libol.EntityResult;

namespace Libol.Models
{
    public class RenewBusiness
    {
        LibolEntities db = new LibolEntities();
        public List<SP_CIR_GET_RENEW_Result> FPT_SP_CIR_GET_RENEW(int intUserID, Byte intType, string strCodeVal)
        {
            List<SP_CIR_GET_RENEW_Result> list = db.Database.SqlQuery<SP_CIR_GET_RENEW_Result>("FPT_SP_CIR_GET_RENEW {0}, {1}, {2}",
                new object[] { intUserID, intType, strCodeVal }).ToList();
            return list;
        }
    }
}