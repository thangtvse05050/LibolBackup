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
    using System.Collections.Generic;
    
    public partial class MARC_AUTHORITY_INDICATOR
    {
        public int FieldID { get; set; }
        public byte Pos { get; set; }
        public string Val { get; set; }
        public string Desciption { get; set; }
        public string FieldCode { get; set; }
    
        public virtual MARC_AUTHORITY_FIELD MARC_AUTHORITY_FIELD { get; set; }
    }
}
