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
    
    public partial class SP_SYS_LDAP_USER_LOGIN_Result
    {
        public int ID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<byte> Priority { get; set; }
        public bool AcqModule { get; set; }
        public bool SerModule { get; set; }
        public bool CirModule { get; set; }
        public bool PatModule { get; set; }
        public bool CatModule { get; set; }
        public bool AdmModule { get; set; }
        public bool ILLModule { get; set; }
        public bool DelModule { get; set; }
        public string Unit { get; set; }
        public string LocModule { get; set; }
        public string LDAPAdsPath { get; set; }
    }
}
