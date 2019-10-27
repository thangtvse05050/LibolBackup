using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class Removed_Item
    {
        public int ID { get; set; }
        public string CopyNumber { get; set; }
        public int ItemID { get; set; }
        public int LibID { get; set; }
        public int LocationID { get; set; }
        public int LoanTypeID { get; set; }
        public string Shelf { get; set; }
        public decimal Price { get; set; }
        public int Reason { get; set; }
        public DateTime? AcquiredDate { get; set; }
        public DateTime? RemovedDate { get; set; }
        public string Volume { get; set; }
        public int UseCount { get; set; }
        public int? PoID { get; set; }
        public DateTime? DateLastUsed { get; set; }
        public string CallNumber { get; set; }
        public int AcquiredSourceID { get; set; }
        public string LiquidCode { get; set; }
        public string Content { get; set; }
        public int REASON_ID { get; set; }
        public string Reson_detail { get; set; }

        public string REASON_DETAIL { get; set; }
        public string LibName { get; set; }
        public string LocName { get; set; }
    }
}