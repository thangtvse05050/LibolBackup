using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPAC.Models
{
    public class FullInforBook
    {
        public string DocumentType { get; set; }
        public string ISBN { get; set; }
        public string LanguageCode { get; set; }
        public string Publishing { get; set; }
        public string PublishingYear { get; set; }
        public string PhysicDescription { get; set; }
        public string Brief { get; set; }

    }
}