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
    
    public partial class SP_PAT_GET_PATRONGROUP_Result
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int InLibraryQuota { get; set; }
        public int LoanQuota { get; set; }
        public int HoldQuota { get; set; }
        public int HoldTurnTimeOut { get; set; }
        public int Priority { get; set; }
        public int ILLQuota { get; set; }
        public int AccessLevel { get; set; }
    }
}
