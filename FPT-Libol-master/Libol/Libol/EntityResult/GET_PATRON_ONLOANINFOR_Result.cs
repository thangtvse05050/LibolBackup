using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    using System;
    public partial class GET_PATRON_ONLOANINFOR_Result
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public Nullable<System.DateTime> CheckOutDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.Int16> RenewCount { get; set; }    
        public string Serial { get; set; }
        public string FullName { get; set; }
        public float? Price { get; set; }
        public string Currency { get; set; }
    }
}