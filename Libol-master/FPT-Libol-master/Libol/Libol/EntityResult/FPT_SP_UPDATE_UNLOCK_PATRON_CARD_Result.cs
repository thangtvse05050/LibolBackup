using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    using System;
    public class FPT_SP_UPDATE_UNLOCK_PATRON_CARD_Result
    {
        public string strPatronCode { get; set; }
        public Nullable<int> intLockedDays { get; set; }
        public string strNote { get; set; }
    }
}