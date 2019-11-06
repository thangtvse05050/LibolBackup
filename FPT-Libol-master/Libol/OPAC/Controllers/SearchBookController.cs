using OPAC.Dao;
using OPAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace OPAC.Controllers
{
    public class SearchBookController : Controller
    {
        private SearchDao dao = new SearchDao();

        // GET: DetailBook
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DetailBook(int itemID)
        {
            ViewBag.OnHoldingBook = dao.GetOnHoldingBook(itemID);
            ViewBag.TotalBook = dao.GetTotalBook(itemID);
            ViewBag.FreeBook = dao.GetFreeBook(itemID);
            ViewBag.InforCopyNumber = dao.GetInforCopyNumberList(itemID);
            ViewBag.RelatedTerm = dao.FPT_SP_OPAC_GET_RELATED_TERMS_LIST(itemID);
            ViewBag.BookTitle = dao.GetItemTitle(itemID);
            ViewBag.FullBookInfo = dao.GetFullInforBook(itemID);
            ViewBag.Summary = dao.GetSummary(itemID);

            try
            {
                var terms = dao.FPT_SP_OPAC_GET_RELATED_TERMS_LIST(itemID).Where(s => s.TermType.Equals("DDC"))
                    .FirstOrDefault();

                var ddc = terms.DisplayEntry;
                if (ddc.Contains("$a"))
                {
                    ddc = ddc.Replace("$a", "");
                }

                if (ddc.Contains("$b"))
                {
                    ddc = ddc.Replace("$b", " ");
                }

                ViewBag.DDC = ddc;
                ViewBag.OriginalDDC = terms.DisplayEntry;
            }
            catch (NullReferenceException)
            {
                ViewBag.DDC = "";
            }

            //TempData["itemID"] = itemID;
            //TempData["code"] = code;

            return View(dao.SP_CATA_GET_CONTENTS_OF_ITEMS_LIST(itemID, 0));
        }

        [HttpPost]
        public ActionResult GetKeySearch(OptionModel model, string selectOption)
        {
            try
            {
                if (model.SearchingText.Trim().Equals(""))
                {
                    ViewBag.EmptyKeword = "";
                    TempData["errorMessage"] = "Ô tìm kiếm không được để trống";
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    Session["key"] = model.SearchingText.Trim();
                    Session["option"] = selectOption;
                }
            }
            catch (NullReferenceException)
            {
                ViewBag.EmptyKeword = "";
                TempData["errorMessage"] = "Ô tìm kiếm không được để trống";
                return RedirectToAction("Home", "Home");
            }

            return RedirectToAction("SearchBook", new {page = 1});
        }

        public ActionResult SearchBy(string keyWord, int searchBy)
        {
            Session["key"] = keyWord.Trim();
            Session["searchBy"] = searchBy;
            return RedirectToAction("SearchBookByKeyWord", new {page = 1});
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchBook(int page)
        {
            string key = Session["key"].ToString();
            string option = Session["option"].ToString();
            int maxItemInOnePage = 30;
            ViewBag.NumberResult = dao.GetNumberResult(key, option);
            ViewBag.ItemInOnePage = maxItemInOnePage;
            Session["PageNo"] = page;

            return View(dao.GetSearchingBook(key, option, page, maxItemInOnePage));
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SearchBookByKeyWord(int page)
        {
            string keyWord = Session["key"].ToString();
            int searchBy = (int)Session["searchBy"];
            int maxItemInOnePage = 30;
            ViewBag.NumberResultKeyWord = dao.GetNumberResultByKeyWord(keyWord, page, maxItemInOnePage, searchBy);
            ViewBag.ItemInOnePage = maxItemInOnePage;
            Session["PageNo"] = page;

            return View(dao.GetSearchingBookByKeyWord(keyWord, page, maxItemInOnePage, searchBy));
        }

        public ActionResult AdvancedSearchBook()
        {
            ViewBag.DocumentList = dao.GetDocumentType();
            var libraryList = new List<SP_GET_LIBRARY_Result>();
            foreach (var item in dao.GetLibrary())
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                {
                    libraryList.Add(item);
                }
            }
            ViewBag.LibraryList = libraryList;
            return View();
        }

        public JsonResult GetLocationByLibId(int libraryId)
        {
            var list = dao.GetLocation(libraryId);
            var listLocation = new List<SelectListItem>();
            list.Insert(0, new Location()
            {   
                ID = 0,
                Status = false,
                SymbolAndCodeLoc = "--------------Chọn kho--------------"
            });
            foreach (var item in list)
            {
                listLocation.Add(new SelectListItem() { Text = item.SymbolAndCodeLoc, Value = item.ID.ToString() });
            }
            return Json(new SelectList(listLocation, "Value", "Text"));
        }
    }
}