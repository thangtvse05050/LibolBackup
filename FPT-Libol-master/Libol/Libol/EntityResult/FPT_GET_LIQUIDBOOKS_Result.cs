using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    using System;
    public partial class FPT_GET_LIQUIDBOOKS_Result
    {
        public string Reason { get; set; }
        public string Content { get; set; }
        public Nullable<System.Int32> AcquiredSourceID { get; set; }
        public string CallNumber { get; set; }
        public string CopyNumber { get; set; }
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int LibID { get; set; }
        public string LiquidCode { get; set; }
        public int LoanType { get; set; }
        public int LocID { get; set; }
        public Nullable<System.Int32> POID { get; set; }
        public decimal Price { get; set; }
        public string Shelf { get; set; }
        public Nullable<System.Int32> UseCount { get; set; }
        public string Volumn { get; set; }
        public Nullable<System.DateTime> AcquiredDate { get; set; }
        public Nullable<System.DateTime> RemovedDate { get; set; }
        public Nullable<System.DateTime> DateLastUsed { get; set; }
        public string LibName { get; set; }        
        public string LocName { get; set; }
    }
}