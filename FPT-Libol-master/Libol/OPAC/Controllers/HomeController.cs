using OPAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPAC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Home()
        {
            var model = new OptionModel
            {
                Option = OptionList()
            };

            return View(model);
        }

        private List<SelectListItem> OptionList()
        {
            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mọi trường", Value = "0" },
                new SelectListItem { Text = "Nhan đề", Value = "1" },
                new SelectListItem { Text = "Tác giả", Value = "2" },
                new SelectListItem { Text = "Nhà xuất bản", Value = "3" },
                new SelectListItem { Text = "Chỉ số DDC", Value = "4" },
                new SelectListItem { Text = "Ngôn ngữ", Value = "5" },
                new SelectListItem { Text = "Từ khóa", Value = "6" },
                new SelectListItem { Text = "Môn học", Value = "7" }
            };

            return items;
        }
    }
}