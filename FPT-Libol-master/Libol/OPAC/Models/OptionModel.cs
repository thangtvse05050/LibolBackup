using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPAC.Models
{
    public class OptionModel
    {
        public List<SelectListItem> Option { get; set; }
        public string SearchingText { get; set; }
    }
}