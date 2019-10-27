using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    public class SP_LOCK_PATRON_CARD_Result
    {
        public string strPatronCode { get; set; }
        public Nullable<int> intLockedDays { get; set; }
        public string strStartedDate { get; set; }
        public string strNote { get; set; }
    }
}