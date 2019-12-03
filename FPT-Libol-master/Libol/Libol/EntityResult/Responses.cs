using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class Responses
    {
        public string Message { get; set; }
        public List<string> ContentOnLoan { get; set; }
        public List<string> ContentExists { get; set; }
        public int? total { get; set; }
        public int? totalOnloan { get; set; }
        public int? totalSuccess { get; set; }
        public int? totalExists { get; set; }
    }
}