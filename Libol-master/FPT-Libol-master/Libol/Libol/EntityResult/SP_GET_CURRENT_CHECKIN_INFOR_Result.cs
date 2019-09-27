using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class SP_GET_CURRENT_LOANINFOR_Result
    {
        public SP_GET_CURRENT_LOANINFOR_Result()
        {

        }
        public int Id { get; set; }
        public string Note { get; set; }
        public string CopyNumber { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string PatronCode { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Grade { get; set; }
        public string Class { get; set; }
        public string Faculty { get; set; }
        public string FullName { get; set; }
        public string Serial { get; set; }
    }
}