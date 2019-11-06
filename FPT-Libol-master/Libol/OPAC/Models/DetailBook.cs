using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPAC.Models
{
    public class DetailBook
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string CopyNumber { get; set; }
        public bool IsInUser { get; set; }
    }
}