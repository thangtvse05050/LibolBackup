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
    
    public partial class SP_ILL_GET_OR_DETAIL_Result
    {
        public string MediumDisplay { get; set; }
        public string dPaymentType { get; set; }
        public int ID { get; set; }
        public string RequestID { get; set; }
        public Nullable<int> ResponderID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<System.DateTime> NeedBeforeDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> RespondDate { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<System.DateTime> ShippedDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<System.DateTime> ReturnedDate { get; set; }
        public Nullable<System.DateTime> CheckedInDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> LocalDueDate { get; set; }
        public Nullable<System.DateTime> LocalCheckedInDate { get; set; }
        public Nullable<System.DateTime> LocalCheckedOutDate { get; set; }
        public Nullable<int> Renewals { get; set; }
        public Nullable<bool> Renewable { get; set; }
        public Nullable<int> LoanTypeID { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> PatronID { get; set; }
        public Nullable<bool> NoticePatron { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string CurrencyCode1 { get; set; }
        public Nullable<decimal> InsuredForCost { get; set; }
        public string CurrencyCode2 { get; set; }
        public Nullable<int> ReturnInsuranceCost { get; set; }
        public string CurrencyCode3 { get; set; }
        public Nullable<byte> ChargeableUnits { get; set; }
        public Nullable<decimal> MaxCost { get; set; }
        public string CurrencyCode { get; set; }
        public Nullable<int> Status { get; set; }
        public string Note { get; set; }
        public Nullable<byte> DeliveryLocID { get; set; }
        public Nullable<byte> BillingLocID { get; set; }
        public Nullable<bool> ReciprocalAgreement { get; set; }
        public Nullable<bool> WillPayFee { get; set; }
        public Nullable<bool> PaymentProvided { get; set; }
        public Nullable<byte> ServiceType { get; set; }
        public Nullable<byte> CopyrightCompliance { get; set; }
        public Nullable<byte> Priority { get; set; }
        public Nullable<byte> PaymentType { get; set; }
        public Nullable<int> ItemType { get; set; }
        public Nullable<byte> Medium { get; set; }
        public Nullable<byte> DelivMode { get; set; }
        public Nullable<int> EDelivModeID { get; set; }
        public Nullable<bool> Alert { get; set; }
        public string Title { get; set; }
        public string PatronName { get; set; }
        public string PatronCode { get; set; }
        public Nullable<int> PatronGroupID { get; set; }
        public Nullable<int> ID1 { get; set; }
        public Nullable<int> RequestType { get; set; }
        public string CallNumber { get; set; }
        public string Title1 { get; set; }
        public string Author { get; set; }
        public string PlaceOfPub { get; set; }
        public string Publisher { get; set; }
        public string SeriesTitleNumber { get; set; }
        public string VolumeIssue { get; set; }
        public string Edition { get; set; }
        public Nullable<System.DateTime> PubDate { get; set; }
        public Nullable<System.DateTime> ComponentPubDate { get; set; }
        public string ArticleAuthor { get; set; }
        public string ArticleTitle { get; set; }
        public string Pagination { get; set; }
        public string NationalBibNumber { get; set; }
        public string ISBN { get; set; }
        public string ISSN { get; set; }
        public Nullable<byte> ItemType1 { get; set; }
        public string SystemNumber { get; set; }
        public string OtherNumbers { get; set; }
        public string Verification { get; set; }
        public string LocalNote { get; set; }
        public string SponsoringBody { get; set; }
        public string LibrarySymbol { get; set; }
        public string EmailReplyAddress { get; set; }
        public string LibraryName { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<byte> EncodingScheme { get; set; }
        public string dCopyrightCompliance { get; set; }
        public string dServiceType { get; set; }
        public Nullable<System.DateTime> dRequestDate { get; set; }
        public Nullable<System.DateTime> dNeedBeforeDate { get; set; }
        public Nullable<System.DateTime> dExpiryDate { get; set; }
        public string PATRONGROUP { get; set; }
        public string PostDelivName { get; set; }
        public string PostDelivXAddr { get; set; }
        public string PostDelivBox { get; set; }
        public string PostDelivStreet { get; set; }
        public string PostDelivRegion { get; set; }
        public string PostDelivCity { get; set; }
        public string PostDelivCode { get; set; }
        public Nullable<int> PostCountryID { get; set; }
        public string PostCountry { get; set; }
        public string EDELIVMODE { get; set; }
        public string EDELIVTSADDR { get; set; }
        public string BillDelivName { get; set; }
        public string BillDelivXAddr { get; set; }
        public string BillDelivBox { get; set; }
        public string BillDelivStreet { get; set; }
        public string BillDelivRegion { get; set; }
        public string BillDelivCity { get; set; }
        public string BillDelivCode { get; set; }
        public Nullable<int> BillCountryID { get; set; }
        public string BillCountry { get; set; }
        public string ItemTypeName { get; set; }
        public string LoanType { get; set; }
    }
}
