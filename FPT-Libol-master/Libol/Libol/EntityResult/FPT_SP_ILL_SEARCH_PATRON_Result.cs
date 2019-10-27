using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class FPT_SP_ILL_SEARCH_PATRON_Result
    {
        public FPT_SP_ILL_SEARCH_PATRON_Result()
        {

        }
        public string Code { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> ValidDate { get; set; }
        public Nullable<System.DateTime> ExpiredDate { get; set; }
        public string FullName { get; set; }
        public string FullNameSimple { get; set; }

    }


}