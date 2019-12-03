using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    using System;
    public class GET_PATRON_RENEW_LOAN_INFOR_Result
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public string FullName { get; set; }
        public DateTime RenewDate { get; set; }
        public DateTime OverDueDateNew { get; set; }
        public DateTime OverDueDateOld { get; set; }
        public int? OverdueDays { get; set; }
        public decimal OverdueFine { get; set; }
        public float? Price { get; set; }
        public string Currency { get; set; }
    }
}