using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPAC.Models
{
    public class LibraryLocationModel
    {
        public IEnumerable<SP_GET_LIBRARY_Result> Libraries { get; set; }
        public int LibraryId { get; set; }
        public int LocationId { get; set; }
    }
}