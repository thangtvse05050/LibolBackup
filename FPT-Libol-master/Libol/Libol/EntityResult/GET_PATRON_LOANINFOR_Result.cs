using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    using System;
    public partial class GET_PATRON_LOANINFOR_Result
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        public Nullable<System.DateTime> CheckInDate { get; set; }
        public int? RenewCount { get; set; }
        public string Serial { get; set; }
        public string FullName { get; set; }
        public int? OverdueDays { get; set; }
        public decimal OverdueFine { get; set; }
        public float? Price { get; set; }
        public string Currency { get; set; }
    }
}