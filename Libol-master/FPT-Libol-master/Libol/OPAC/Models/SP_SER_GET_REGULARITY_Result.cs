//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OPAC.Models
{
    using System;
    
    public partial class SP_SER_GET_REGULARITY_Result
    {
        public int ItemID { get; set; }
        public Nullable<int> AcqSourceID { get; set; }
        public int BindingMode { get; set; }
        public int ClaimCycle1 { get; set; }
        public int ClaimCycle2 { get; set; }
        public int ClaimCycle3 { get; set; }
        public Nullable<int> DeliveryTime { get; set; }
        public Nullable<bool> Ceased { get; set; }
        public string FreqCode { get; set; }
        public string Note { get; set; }
        public string SummaryHolding { get; set; }
        public Nullable<int> POID { get; set; }
        public Nullable<int> FreqMode { get; set; }
        public Nullable<System.DateTime> CeasedDate { get; set; }
        public Nullable<System.DateTime> BasedDate { get; set; }
        public int OnSubscription { get; set; }
        public int BindingRule { get; set; }
        public string Months { get; set; }
        public string ChangeNote { get; set; }
        public string DOWs { get; set; }
        public string Weeks { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public byte ResetRegularity { get; set; }
        public Nullable<int> ClassID { get; set; }
    }
}
