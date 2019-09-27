using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class FPT_SP_CATA_GET_DICINFOR_REFERENCES_Result
    {
        public int ID { get; set; }
        public string FieldCode { get; set; }
        public Nullable<int> DicID { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public int FieldTypeID { get; set; }
        public Nullable<int> LinkTypeID { get; set; }

        public FPT_SP_CATA_GET_DICINFOR_REFERENCES_Result()
        {
        }
    }
}