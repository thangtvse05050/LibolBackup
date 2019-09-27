using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class Library
    {
        public string ID { get; set; }
        public string LibName { get; set; }
        public string Code { get; set; }
        public Total_Amount Total { get; set; }

        public Library(string ID, string Code, string LibName)
        {
            this.ID = ID;
            this.LibName = LibName;
            this.Code = Code;
        }
        public Library(string ID, string Code, string LibName, Total_Amount Total)
        {
            this.ID = ID;
            this.LibName = LibName;
            this.Code = Code;
            this.Total = Total;
        }
    }
}