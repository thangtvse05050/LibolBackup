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
    
    public partial class FPT_SP_GET_HOLDING_BY_RECOMMENDID_Newest_Result
    {
        public string RECORDNUMBER { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> ReceiptedDate { get; set; }
        public Nullable<int> useCount { get; set; }
        public string ISBN { get; set; }
        public Nullable<int> InBookNum { get; set; }
        public string DKCB { get; set; }
        public Nullable<System.DateTime> ACQUIREDDATE { get; set; }
        public int LocationID { get; set; }
        public string RECOMMENDID { get; set; }
        public string Year { get; set; }
        public Nullable<float> Price { get; set; }
        public string Currency { get; set; }
        public string NXB { get; set; }
        public Nullable<double> FullPrice { get; set; }
        public int ItemID { get; set; }
    }
}
