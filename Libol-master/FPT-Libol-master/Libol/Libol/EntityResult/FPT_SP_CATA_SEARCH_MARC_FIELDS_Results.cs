using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class FPT_SP_CATA_SEARCH_MARC_FIELDS_Results
    {
        public string FCURL1 { get; set; }
        public string OnClick { get; set; }
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


    }
}