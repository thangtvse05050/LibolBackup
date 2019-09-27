using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class GET_CATALOGUE_FIELDS_Result
    {
        public GET_CATALOGUE_FIELDS_Result()
        {
            
        }

        public Nullable<int> AuthorityID { get; set; }
        public bool Mandatory { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public bool Coded { get; set; }
        public int Length { get; set; }
        public string Indicators { get; set; }
        public string VietIndicators { get; set; }
        public int FieldTypeID { get; set; }
        public bool Repeatable { get; set; }
        public string FieldCode { get; set; }
        public int BlockID { get; set; }
        public string VietFieldName { get; set; }
        public string FieldName { get; set; }
        public Nullable<int> DicID { get; set; }
        public string ParentFieldCode { get; set; }
        public Nullable<bool> NeedHelp { get; set; }
        public Nullable<int> ClassificationID { get; set; }

    }
}