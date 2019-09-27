using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class FPT_SP_GETINFOR_EMAIL_Result
    {
        public int LOANID { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string CheckoutDate { get; set; }
        public string CheckInDate { get; set; }
        public string MainTitle { get; set; }
        public string CopyNumber { get; set; }
        public int PatronID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int OverdueDate { get; set; }
        public int OverdueDateIncludeWeek { get; set; }
        public System.Decimal Penati { get; set; }
        public string PatronCode { get; set; }
        public string Code { get; set; }
        public string ItemCode { get; set; }
        public int ID { get; set; }
        public int LibId { get; set; }
        public string LibCode { get; set; }
        public string LocCode { get; set; }
    }
}