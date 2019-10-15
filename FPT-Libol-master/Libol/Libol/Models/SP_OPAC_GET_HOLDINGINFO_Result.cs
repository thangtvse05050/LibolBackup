//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Libol.Models
{
    using System;
    
    public partial class SP_OPAC_GET_HOLDINGINFO_Result
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int LocationID { get; set; }
        public int LibID { get; set; }
        public int UseCount { get; set; }
        public string Volume { get; set; }
        public Nullable<System.DateTime> AcquiredDate { get; set; }
        public string CopyNumber { get; set; }
        public bool InUsed { get; set; }
        public Nullable<bool> InCirculation { get; set; }
        public Nullable<int> ILLID { get; set; }
        public Nullable<float> Price { get; set; }
        public string Shelf { get; set; }
        public Nullable<int> POID { get; set; }
        public Nullable<System.DateTime> DateLastUsed { get; set; }
        public string CallNumber { get; set; }
        public bool Acquired { get; set; }
        public string Note { get; set; }
        public Nullable<int> LoanTypeID { get; set; }
        public Nullable<int> AcquiredSourceID { get; set; }
        public string LockedReason { get; set; }
        public Nullable<bool> IsLost { get; set; }
        public Nullable<bool> IsConfusion { get; set; }
        public Nullable<bool> Availlable { get; set; }
        public bool OnHold { get; set; }
        public string Currency { get; set; }
        public Nullable<int> Reason { get; set; }
        public Nullable<float> Rate { get; set; }
        public string RecordNumber { get; set; }
        public Nullable<System.DateTime> ReceiptedDate { get; set; }
    }
}
