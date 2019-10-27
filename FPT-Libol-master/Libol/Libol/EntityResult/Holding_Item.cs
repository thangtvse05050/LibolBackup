using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class Holding_Item
    {
        public Int64 Seq { get; set; }
        public bool Acquired { get; set; }
        public int ID { get; set; }
        public int LibID { get; set; }
        public int LocationID { get; set; }
        public string Content { get; set; }
        public string Volume { get; set; }
        public DateTime? AcquiredDate { get; set; }
        public string CopyNumber { get; set; }
        public string CallNumber { get; set; }
        public string Shelf { get; set; }
        public bool InUsed { get; set; }
        public bool? InCirculation { get; set; }
        public string Note { get; set; }

        public DateTime? DateLastUsed { get; set; }
        public Single? Price { get; set; }
        public int UseCount { get; set; }
        public string LibName { get; set; }
        public string LocName { get; set; }
        public int? LoanTypeID { get; set; }
        public int ItemID { get; set; }
        public int? POID { get; set; }
        public int? AcquiredSourceID { get; set; }
    }
}