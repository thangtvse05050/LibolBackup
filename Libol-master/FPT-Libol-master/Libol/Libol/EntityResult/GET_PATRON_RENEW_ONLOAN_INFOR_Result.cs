using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    using System;
    public partial class GET_PATRON_RENEW_ONLOAN_INFOR_Result
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> RenewDate { get; set; }
        public Nullable<System.DateTime> OverDueDateNew { get; set; }
        public Nullable<System.DateTime> OverDueDateOld { get; set; }
        public float? Price { get; set; }
        public string Currency { get; set; }
    }
}