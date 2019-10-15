using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    using System;
    public class SP_GET_LOCKEDPATRONS_Result
    {
        public string PatronCode { get; set; }
        public DateTime StartedDate { get; set; }
        public string Note { get; set; }
        public string FullName { get; set; }
        public DateTime FinishDate { get; set; }
        public int LockedDays { get; set; }
        public string College { get; set; }
    }
}