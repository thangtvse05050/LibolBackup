using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    public class FPT_ACQ_MONTH_STATISTIC_Result
    {
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> BooksTotal { get; set; }
        public Nullable<int> CopiesTotal { get; set; }
        public Nullable<double> MoneyTotal { get; set; }
    }
}