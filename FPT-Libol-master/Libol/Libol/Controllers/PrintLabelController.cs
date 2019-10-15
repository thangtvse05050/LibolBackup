using Libol.EntityResult;
using Libol.Models;
using Libol.SupportClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Libol.Controllers
{
    public class PrintLabelController : Controller
    {
        private LibolEntities db = new LibolEntities();
        ShelfBusiness shelfBusiness = new ShelfBusiness();

        [AuthAttribute(ModuleID = 4, RightID = "31")]
        public ActionResult Index()
        {
            ViewBag.Library = shelfBusiness.FPT_SP_HOLDING_LIBRARY_SELECT(0, 1, -1, (int)Session["UserID"], 1);
            ViewBag.ItemTypes = db.SP_GET_ITEMTYPES().ToList();
            ViewBag.Template = db.SP_SYS_GET_TEMPLATE(0, 4).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult OnchangeLibrary(int LibID)
        {
            List<SP_HOLDING_LOCATION_GET_INFO_Result> list = shelfBusiness.FPT_SP_HOLDING_LOCATION_GET_INFO(LibID, (int)Session["UserID"], 0, -1);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AuthAttribute(ModuleID = 4, RightID = "0")]
        public ActionResult Print(int intSelMode,
            int intLibID,
            int intLocID,
            string strFromItemCode,
            string strToItemCode,
            string strFromCopyNumber,
            string strToCopyNumber,
            string strElse,
            int intItemType,
            int TemplateID,
            int ColPageNumber,
            int RowPageNumber,
            int Page)
        {
            if (Page < 1)
            {
                Page = 1;
            }
            var selectCopyNumber = db.HOLDINGs.Where(a => true);
            switch (intSelMode)
            {
                case 0:
                    selectCopyNumber = selectCopyNumber
                        .Where(a => db.ITEMs.Where(i => i.Code.CompareTo(strFromItemCode) >= 0 && i.Code.CompareTo(strToItemCode) <= 0).Select(i => i.ID).Contains(a.ItemID));
                    break;
                case 1:
                    selectCopyNumber = selectCopyNumber.Where(a => string.Compare(a.CopyNumber, strFromCopyNumber) >= 0 &&
                                                                   string.Compare(a.CopyNumber, strToCopyNumber) <= 0);
                    if (intLibID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LibID == intLibID);
                    }
                    if (intLocID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LocationID == intLocID);
                    }
                    if (intItemType > 0)
                    {
                        List<Int32> listItemID = new List<int>();
                        foreach (var i in db.ITEMs.Where(a => a.CAT_DIC_ITEM_TYPE.ID == intItemType).ToList())
                        {
                            listItemID.Add(i.ID);
                        }
                        selectCopyNumber = selectCopyNumber.Where(a => listItemID.Contains(a.ItemID)).OrderBy(a => a.ItemID);
                    }
                    break;
                case 2:
                    selectCopyNumber = selectCopyNumber.Where(a => strElse.Contains(a.CopyNumber));
                    if(intLibID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LibID == intLibID);
                    }
                    if (intLocID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LocationID == intLocID);
                    }
                    if (intItemType > 0)
                    {
                        List<Int32> listItemID = new List<int>();
                        foreach(var i in db.ITEMs.Where(a => a.CAT_DIC_ITEM_TYPE.ID == intItemType).ToList())
                        {
                            listItemID.Add(i.ID);
                        }
                        selectCopyNumber = selectCopyNumber.Where(a => listItemID.Contains(a.ItemID)).OrderBy(a => a.ItemID);
                    }
                    break;
            }

            string Data = "<table>";
            string Template = db.SYS_TEMPLATE.Where(a => a.ID == TemplateID).First().Content;
            double TotalItem = selectCopyNumber.Count();
            double TotalItemPerPage = RowPageNumber * ColPageNumber;
            double TotalPage = Math.Ceiling(TotalItem / TotalItemPerPage);
            ViewBag.CurrentPage = Page;
            ViewBag.TotalPage = TotalPage;
            var CurrentItemInPage = selectCopyNumber.OrderBy(a => a.ItemID).Skip((int)TotalItemPerPage * (Page-1)).Take((int)TotalItemPerPage).ToArray();
            int Count = 0;
            for(int i = 0; i < RowPageNumber; i++)
            {
                Data += "<tr>";
                for(int j = 0; j < ColPageNumber; j++)
                {                    
                    if (Count < CurrentItemInPage.Length)
                    {
                        // HARDCODE , MAYBE NEED CHANGE LATER
                        string CallNumber = CurrentItemInPage[Count].CallNumber;
                        string a = CallNumber.Split(' ')[0];
                        string b = CallNumber.Split(' ')[1];
                        Data += "<td>";
                        Data += Template.Replace("<$090$a$>", a).Replace("<$090$b$>", b);
                        Data += "</td>";
                        Count++;
                    }
                    
                }
                Data += "</tr>";
            }
            Data += "</table>";
            ViewBag.Data = Data;
            return View();
        }

        [HttpPost]        
        public JsonResult SearchItemCode(DataTableAjaxPostModel model,
            string txtTitle, string txtCopyNumber, string txtAuthor, string txtPublisher, string txtYear, string txtISBN)
        {
            var items = db.ITEMs;
            var search = items.Where(a => true);
            if (!String.IsNullOrEmpty(txtTitle))
            {
                search = search.Where(a => a.ITEM_TITLE.Where(t => t.Title.Contains(txtTitle) && t.ItemID == a.ID).Count()>0);
            }
            if (!String.IsNullOrEmpty(txtCopyNumber))
            {
                search = search.Where(a => db.HOLDINGs.Where(h => h.CopyNumber.Contains(txtCopyNumber)).Select(h => h.ItemID).Contains(a.ID));
            }
            if (!String.IsNullOrEmpty(txtAuthor))
            {
                search = search.Where(a => a.ITEM_AUTHOR.Where(au => au.CAT_DIC_AUTHOR.DisplayEntry.Contains(txtAuthor)).Count()>0);
            }
            if (!String.IsNullOrEmpty(txtPublisher))
            {
                search = search.Where(a => a.ITEM_PUBLISHER.Where(p => p.CAT_DIC_PUBLISHER.DisplayEntry.Contains(txtPublisher)).Count()>0);
            }
            if (!String.IsNullOrEmpty(txtISBN))
            {
                search = search.Where(a => db.CAT_DIC_NUMBER.Where(n => n.ItemID == a.ID && n.Number.Contains(txtISBN)).Select(n => n.ItemID).Equals(a.ID));
            }
            if (!String.IsNullOrEmpty(txtYear))
            {
                search = search.Where(a => db.CAT_DIC_YEAR.Where(y => y.Year.Equals(txtYear)).Select(n => n.ItemID).Contains(a.ID));
            }

            var sorting = search.OrderBy(a => a.ID);
            if (model.order[0].column == 0)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    
                }
                else
                {
                    
                }

            }
            else if (model.order[0].column == 1)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    
                }
                else
                {
                    
                }

            }
            var paging = sorting.Skip(model.start).Take(model.length).ToList();
            var result = new List<SearchItemCodeResult>(paging.Count);
            foreach (var s in paging)
            {
                result.Add(new SearchItemCodeResult
                {
                    Code = s.Code,
                    Title = new FormatHoldingTitle().OnFormatHoldingTitle( s.FIELD200S.Where(t => t.FieldCode.Equals("245")).First().Content),
                });
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = search.Count(),
                recordsFiltered = search.Count(),
                data = result
            });
        }
    }

    public class SearchItemCodeResult
    {
        public string Code { get; set; }
        public string Title { get; set; }
    }
}