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
    
    public partial class SP_CATA_GET_FIELD_PROPERTIES_Result
    {
        public int ID { get; set; }
        public Nullable<int> AuthorityID { get; set; }
        public bool Mandatory { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public bool Coded { get; set; }
        public int Length { get; set; }
        public string Description { get; set; }
        public string Indicators { get; set; }
        public string VietIndicators { get; set; }
        public int FieldTypeID { get; set; }
        public bool Repeatable { get; set; }
        public Nullable<int> LinkTypeID { get; set; }
        public string FieldCode { get; set; }
        public int BlockID { get; set; }
        public string VietFieldName { get; set; }
        public string FieldName { get; set; }
        public Nullable<int> DicID { get; set; }
        public string ParentFieldCode { get; set; }
        public bool IsMARCField { get; set; }
        public Nullable<bool> NeedHelp { get; set; }
        public Nullable<int> ClassificationID { get; set; }
        public string FieldType { get; set; }
        public string FieldFunction { get; set; }
    }
}
