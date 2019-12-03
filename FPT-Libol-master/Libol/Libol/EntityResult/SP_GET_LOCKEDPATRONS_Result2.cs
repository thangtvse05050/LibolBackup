using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    using System;
    public class SP_GET_LOCKEDPATRONS_Result2
    {
        public string PatronCode { get; set; }
        public string StartedDate { get; set; }
        public string Note { get; set; }
        public string FullName { get; set; }
        public string FinishDate { get; set; }
        public int LockedDays { get; set; }
    }
}