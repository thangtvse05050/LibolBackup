using System;

namespace Libol.EntityResult
{
    public class SP_CIR_GET_RENEW_Result
    {
        public SP_CIR_GET_RENEW_Result() { }

        public string Content { get; set; }
        public int Renewals { get; set; }
        public int Renewalperiod { get; set; }
        public int TimeUnit { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public int ID { get; set; }
        public int LoanTypeID { get; set; }
        public byte LoanMode { get; set; }
        public string CopyNumber { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Note { get; set; }
        public Int16 RenewCount { get; set; }
        public int PatronID { get; set; }
        public int ItemID { get; set; }
        public Nullable<System.DateTime> RecallDate { get; set; }
        public int LocationID { get; set; }
        public string Serial { get; set; }
        public string strNote { get; set; }
        public string timeHold { get; set; }
        public DateTime Today { get; set; }
    }
}