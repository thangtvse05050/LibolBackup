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
    
    public partial class SP_HOLDING_LOC_SCHEMA_SEL_Result
    {
        public int ID { get; set; }
        public string Symbol { get; set; }
        public Nullable<int> LocID { get; set; }
        public string ImgURL { get; set; }
        public Nullable<int> ImgWidth { get; set; }
        public Nullable<int> ImgHeight { get; set; }
        public Nullable<int> TopCoor { get; set; }
        public Nullable<int> LeftCoor { get; set; }
        public Nullable<float> ImgWidthMetter { get; set; }
        public Nullable<float> ImgHeightMetter { get; set; }
        public byte[] ImgByte { get; set; }
        public string ShowImg { get; set; }
    }
}
