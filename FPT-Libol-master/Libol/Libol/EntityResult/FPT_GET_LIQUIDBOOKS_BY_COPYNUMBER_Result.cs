using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result
    {
        public string Reason { get; set; }
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public string LiquidCode { get; set; }
       // public int LocationID { get; set; }
        public decimal Price { get; set; }
        public Nullable<System.Int32> UseCount { get; set; }
        public Nullable<System.DateTime> RemovedDate { get; set; }

        public FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result()
        {
        }

        public FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result(string reason, string content, string copyNumber, string liquidCode, decimal price, int? useCount, DateTime? removedDate)
        {
            Reason = reason;
            Content = content;
            CopyNumber = copyNumber;
            LiquidCode = liquidCode;
            Price = price;
            UseCount = useCount;
            RemovedDate = removedDate;
        }
    }
}