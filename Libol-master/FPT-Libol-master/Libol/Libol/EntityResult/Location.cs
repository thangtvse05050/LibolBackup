using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class Location
    {
        public string LOCNAME { get; set; }
        public string ID { get; set; }
        public string GroupID { get; set; }
        public string LibID { get; set; }
        public string Symbol { get; set; }
        public string Code { get; set; }

        public Total_Amount Total { get; set; }

        public Location(string LOCNAME, string ID, string GroupID,
            string LibID, string Symbol, string Code)
        {
            this.LOCNAME = LOCNAME;
            this.ID = ID;
            this.GroupID = GroupID;
            this.LibID = LibID;
            this.Symbol = Symbol;
            this.Code = Code;
        }
        public Location(string LOCNAME, string ID, string GroupID,
            string LibID, string Symbol, string Code, Total_Amount Total)
        {
            this.LOCNAME = LOCNAME;
            this.ID = ID;
            this.GroupID = GroupID;
            this.LibID = LibID;
            this.Symbol = Symbol;
            this.Code = Code;
            this.Total = Total;
        }

        public Location()
        {
        }
    }
}