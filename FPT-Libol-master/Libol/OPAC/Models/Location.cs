using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPAC.Models
{
    public class Location
    {
        public int ID { get; set; }
        public int? LibID { get; set; }
        public string SymbolAndCodeLoc { get; set; }
        public bool Status { get; set; }
        public int? MaxNumber { get; set; }
    }
}