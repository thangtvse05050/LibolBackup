using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class SP_CATA_GET_MODIFIED_FIELDS_Result
    {
        public SP_CATA_GET_MODIFIED_FIELDS_Result()
        {

        }
        public string FieldCode { get; set; }
        public string VietFieldName { get; set; }
        public string FieldName { get; set; }
        public int BlockID { get; set; }
        public bool Repeatable { get; set; }
        public string ParentFieldCode { get; set; }
        public bool Mandatory { get; set; }
        public bool AlwaysMandatory { get; set; }
        public Nullable<int> DicID { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public bool Coded { get; set; }
        public int FieldTypeID { get; set; }
        public int Length { get; set; }
        public string Indicators { get; set; }
        public string VietIndicators { get; set; }
        public Nullable<int> AuthorityID { get; set; }
    }
}