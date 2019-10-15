using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Libol.EntityResult;
using Libol.Models;
using Libol.SupportClass;
using System.Data;

namespace Libol.Controllers
{
    public class ShelfController : Controller
    {
        LibolEntities db = new LibolEntities();
        ShelfBusiness shelfBusiness = new ShelfBusiness();
        List<Library> libs = new List<Library>();
        List<LibraryLocation> lic_locs = new List<LibraryLocation>();

        [AuthAttribute(ModuleID = 4, RightID = "26")]
        public ActionResult Index()
        {

            //get list marc form
            ViewData["ListNBS"] = db.ACQ_ACQUIRE_SOURCE.OrderBy(d => d.ID).ToList();
            //Cấp thư mục
            ViewData["listKTL"] = db.CIR_LOAN_TYPE.ToList();
            ViewData["ListCurrency"] = db.ACQ_CURRENCY.OrderBy(d => d.CurrencyCode).ToList();
            ViewData["ListDeleteReason"] = db.SP_HOLDING_REMOVE_REASON_SEL(0).ToList();

            List<SP_HOLDING_LIBRARY_SELECT_Result> listLibsResult = shelfBusiness.FPT_SP_HOLDING_LIBRARY_SELECT(0, 1, -1, (int)Session["UserID"], 1);
            List<HOLDING_LIBRARY> libs = SP_HOLDING_LIBRARY_SELECT_Result.ConvertToHoldingLibrary(listLibsResult);
            ViewData["listLibs"] = libs;


            string code = Request.QueryString["Code"];


            if (!string.IsNullOrEmpty(code))
            {
                var item = db.ITEMs.Where(i => i.Code.Equals(code)).FirstOrDefault();
                if (item == null)
                {
                    ViewBag.AlertMessage = "Mã tài liệu không tồn tại";
                    return View();
                }
                ViewBag.content = getContentShelf(code);
                int itemID = item.ID;
                ViewBag.itemID = itemID;
            }

            return View();

        }

        [HttpPost]
        public JsonResult SelectHolding(int libID)
        {
            List<SP_HOLDING_LOCATION_GET_INFO_Result> listLocsResult = shelfBusiness.FPT_SP_HOLDING_LOCATION_GET_INFO(libID, (int)Session["UserID"], 0, -1);
            List<HOLDING_LOCATION> locs = SP_HOLDING_LOCATION_GET_INFO_Result.ConvertToHoldingLocation(listLocsResult);
            ViewData["listLocs"] = locs;
            return Json(locs, JsonRequestBehavior.AllowGet);
        }
        public string getContentShelf(string idMTL)
        {

            List<FPT_EDU_GET_SHELF_CONTENT_Result> listContentResult = db.FPT_EDU_GET_SHELF_CONTENT(idMTL).ToList();
            string contentOutput = "";
            string fieldCode = "";
            string field020 = "";
            string field022 = "";
            string field100 = "";
            string field110 = "";
            string field245 = "";
            string field250 = "";
            string field260 = "";
            string field300 = "";
            string field490 = "";
            string field520 = "";
            foreach (FPT_EDU_GET_SHELF_CONTENT_Result item in listContentResult)
            {
                
                fieldCode = item.FieldCode;
                if (fieldCode.Equals("020"))
                {
                    field020 = ". -" + shelfBusiness.GetContent(item.Content);
                }
                if (fieldCode.Equals("022"))
                {
                    field022 = "=" + shelfBusiness.GetContent(item.Content);
                }
                if (fieldCode.Equals("100"))
                {
                    field100 = shelfBusiness.GetContent(item.Content);
                }
                if (fieldCode.Equals("110"))
                {
                    field110 = shelfBusiness.GetContent(item.Content);
                }
                if (fieldCode.Equals("245"))
                {
                    field245 = item.Content;
                    if (field245.Contains("$a"))
                    {
                        field245 = field245.Replace("$a", ". -");
                    }
                    if (field245.Contains("$b"))
                    {
                        field245 = field245.Replace("$b", " ");
                    }
                    if (field245.Contains("$c"))
                    {  
                        field245 = field245.Replace("$c", " ");
                    }
                    if (field245.Contains("$n"))
                    {
                        field245 = field245.Replace("$n", " ");
                    }
                    if (field245.Contains("$p"))
                    {
                        field245 = field245.Replace("$p", " ");
                    }
                }
                if (fieldCode.Equals("250"))
                {
                    field250 = ". -" + shelfBusiness.GetContent(item.Content);
                }
                if (fieldCode.Equals("260"))
                {
                    field260 = item.Content;
                    if (field260.Contains("$a"))
                    {
                        field260 = field260.Replace("$a", ". -");
                    }
                    if (field260.Contains("$b"))
                    {
                        field260 = field260.Replace("$b", " ");
                    }
                    if (field260.Contains("$c"))
                    {
                        field260 = field260.Replace("$c", " ");
                    }
                }
                if (fieldCode.Equals("300"))
                {
                    field300 = item.Content;
                    if (field300.Contains("$a"))
                    {
                        field300 = field300.Replace("$a", ". -");
                    }
                    if (field300.Contains("$b"))
                    {
                        field300 = field300.Replace("$b", " ");
                    }
                    if (field300.Contains("$c"))
                    {
                        field300 = field300.Replace("$c", " ");
                    }
                    if (field300.Contains("$e"))
                    {
                        field300 = field300.Replace("$e", " ");
                    }
                }
                if (fieldCode.Equals("490"))
                {
                    field490 = ". -" + shelfBusiness.GetContent(item.Content);
                }
                if (fieldCode.Equals("520"))
                {
                    field520 = shelfBusiness.GetContent(item.Content);
                }
                contentOutput = field022  + field100 + field110  +"@"+ field245 + "@" + field250  + field260 + field300  + field490  + "@" + field520  + field020;
            }
            return contentOutput;
        }



        [HttpPost]
        public JsonResult GenCopyNumber(int locId)
        {
            string copyNumber = shelfBusiness.GenCopyNumber(locId);
            return Json(new Result()
            {
                Data = copyNumber
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertHolding(HOLDING holding, int numberOfCN,string recommendID)
        {
            string composite = "";
            string message = shelfBusiness.InsertHolding(holding, numberOfCN,recommendID,ref composite);
            return Json(new { Message = message, Composite = composite }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateHolding(HoldingTable holdingUpdate)
        {
            string message = shelfBusiness.UpdateHolding(holdingUpdate);
            return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteHolding(int[] CopynumberList,int DeleteReasonID)
        {

            if (CopynumberList == null || CopynumberList.Length <= 0)
            {
                return Json(new { Message = "Hãy chọn bản ghi" }, JsonRequestBehavior.AllowGet);
            }
            string holdingIDs = string.Join(",", CopynumberList);
            int result = db.SP_HOLDING_REMOVED_PROC(DeleteReasonID, holdingIDs);
            return Json(new { Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeStatus(List<HoldingStatus> statusList)
        {
            try
            {
                foreach (var item in statusList)
                {
                    db.SP_PROCESS_HOLDING("" + item.HoldingID, item.LibID, item.LocID, item.Shelf, item.Mode, item.IsNew);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new { Message = "Kiểm nhận và mở khóa thành công!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadTableHolding(string code)
        {
            string message ="";

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            var sortColumnInt = Request.Form.GetValues("order[0][column]");

            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


            //Paging Size (10,20,50,100)    
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int RecordsTotal = 0;
            // Getting all holding data    
            var item = db.ITEMs.Where(i => i.Code.Equals(code)).FirstOrDefault();
            int itemID = 0;
            if (item != null)
            {
                itemID = db.ITEMs.Where(i => i.Code.Equals(code)).Select(i => i.ID).FirstOrDefault();
            }
            else
            {
                message = "Mã tài liệu không tồn tại";
                return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, numberOfFreeCopies = 0,data= new HoldingTable(), compositeHolding = 0, numberRecord = 0, Message = message });
            }
            


            var holdings = new List<HOLDING>();
            holdings = db.HOLDINGs.Where(h => h.ItemID == itemID).OrderBy(h => h.ID).ToList();

            // thông tin tổng hợp về mã tài liệu
            int numberFreeCopies = holdings.Where(h => h.InUsed == false).Count();
            string compositeHoldingData = shelfBusiness.GenerateCompositeHoldings(itemID);
            if (Convert.ToInt32(draw) > 1)
            {
                compositeHoldingData = "";
            }
            
            int numberOfRecord = holdings.Count(); 

            //Sorting    
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                // sort theo library and location
                #region
                if (sortColumn.Equals("Library"))
                {
                    if (sortColumnDir.Equals("asc"))
                    {
                        holdings = db.HOLDINGs.Join(db.HOLDING_LIBRARY, holding => holding.LibID, lib => lib.ID, (holding, lib) => new { HOLDING = holding, HOLDING_LIBRARY = lib })
                             .Where(y => y.HOLDING.ItemID == itemID).OrderBy(o => o.HOLDING_LIBRARY.AccessEntry).ToList().Select(x => new HOLDING()
                             {
                                 ID = x.HOLDING.ID,
                                 LibID = x.HOLDING.LibID,
                                 LocationID = x.HOLDING.LocationID,
                                 AcquiredDate = x.HOLDING.AcquiredDate,
                                 AcquiredSourceID = x.HOLDING.AcquiredSourceID,
                                 CallNumber = x.HOLDING.CallNumber,
                                 CopyNumber = x.HOLDING.CopyNumber,
                                 Currency = x.HOLDING.Currency,
                                 Note = x.HOLDING.Note,
                                 Price = x.HOLDING.Price,
                                 ReceiptedDate = x.HOLDING.ReceiptedDate,
                                 RecordNumber = x.HOLDING.RecordNumber,
                                 Shelf = x.HOLDING.Shelf,
                                 Volume = x.HOLDING.Volume,
                                 ItemID = x.HOLDING.ItemID,
                                 UseCount = x.HOLDING.UseCount,
                                 InUsed = x.HOLDING.InUsed,
                                 InCirculation = x.HOLDING.InCirculation,
                                 ILLID = x.HOLDING.ILLID,
                                 POID = x.HOLDING.POID,
                                 DateLastUsed = x.HOLDING.DateLastUsed,
                                 LoanTypeID = x.HOLDING.LoanTypeID,
                                 LockedReason = x.HOLDING.LockedReason,
                                 IsLost = x.HOLDING.IsLost,
                                 IsConfusion = x.HOLDING.IsConfusion,
                                 Rate = x.HOLDING.Rate,
                                 Reason = x.HOLDING.Reason,
                                 OnHold = x.HOLDING.OnHold,
                                 Acquired = x.HOLDING.Acquired,
                                 Availlable = x.HOLDING.Availlable,

                             }).ToList();
                    }
                    else
                    {
                        holdings = db.HOLDINGs.Join(db.HOLDING_LIBRARY, holding => holding.LibID, lib => lib.ID, (holding, lib) => new { HOLDING = holding, HOLDING_LIBRARY = lib })
                            .Where(y => y.HOLDING.ItemID == itemID).OrderByDescending(o => o.HOLDING_LIBRARY.AccessEntry).ToList().Select(x => new HOLDING()
                            {
                                ID = x.HOLDING.ID,
                                LibID = x.HOLDING.LibID,
                                LocationID = x.HOLDING.LocationID,
                                AcquiredDate = x.HOLDING.AcquiredDate,
                                AcquiredSourceID = x.HOLDING.AcquiredSourceID,
                                CallNumber = x.HOLDING.CallNumber,
                                CopyNumber = x.HOLDING.CopyNumber,
                                Currency = x.HOLDING.Currency,
                                Note = x.HOLDING.Note,
                                Price = x.HOLDING.Price,
                                ReceiptedDate = x.HOLDING.ReceiptedDate,
                                RecordNumber = x.HOLDING.RecordNumber,
                                Shelf = x.HOLDING.Shelf,
                                Volume = x.HOLDING.Volume,
                                ItemID = x.HOLDING.ItemID,
                                UseCount = x.HOLDING.UseCount,
                                InUsed = x.HOLDING.InUsed,
                                InCirculation = x.HOLDING.InCirculation,
                                ILLID = x.HOLDING.ILLID,
                                POID = x.HOLDING.POID,
                                DateLastUsed = x.HOLDING.DateLastUsed,
                                LoanTypeID = x.HOLDING.LoanTypeID,
                                LockedReason = x.HOLDING.LockedReason,
                                IsLost = x.HOLDING.IsLost,
                                IsConfusion = x.HOLDING.IsConfusion,
                                Rate = x.HOLDING.Rate,
                                Reason = x.HOLDING.Reason,
                                OnHold = x.HOLDING.OnHold,
                                Acquired = x.HOLDING.Acquired,
                                Availlable = x.HOLDING.Availlable,
                            }).ToList();
                    }

                }
                else if (sortColumn.Equals("Location"))
                {

                    if (sortColumnDir.Equals("asc"))
                    {
                        holdings = db.HOLDINGs.Join(db.HOLDING_LOCATION, holding => holding.LocationID, loc => loc.ID, (holding, loc) => new { HOLDING = holding, HOLDING_LOCATION = loc })
                            .Where(y => y.HOLDING.ItemID == itemID).OrderBy(o => o.HOLDING_LOCATION.Symbol).ToList().Select(x => new HOLDING()
                            {
                                ID = x.HOLDING.ID,
                                LibID = x.HOLDING.LibID,
                                LocationID = x.HOLDING.LocationID,
                                AcquiredDate = x.HOLDING.AcquiredDate,
                                AcquiredSourceID = x.HOLDING.AcquiredSourceID,
                                CallNumber = x.HOLDING.CallNumber,
                                CopyNumber = x.HOLDING.CopyNumber,
                                Currency = x.HOLDING.Currency,
                                Note = x.HOLDING.Note,
                                Price = x.HOLDING.Price,
                                ReceiptedDate = x.HOLDING.ReceiptedDate,
                                RecordNumber = x.HOLDING.RecordNumber,
                                Shelf = x.HOLDING.Shelf,
                                Volume = x.HOLDING.Volume,
                                ItemID = x.HOLDING.ItemID,
                                UseCount = x.HOLDING.UseCount,
                                InUsed = x.HOLDING.InUsed,
                                InCirculation = x.HOLDING.InCirculation,
                                ILLID = x.HOLDING.ILLID,
                                POID = x.HOLDING.POID,
                                DateLastUsed = x.HOLDING.DateLastUsed,
                                LoanTypeID = x.HOLDING.LoanTypeID,
                                LockedReason = x.HOLDING.LockedReason,
                                IsLost = x.HOLDING.IsLost,
                                IsConfusion = x.HOLDING.IsConfusion,
                                Rate = x.HOLDING.Rate,
                                Reason = x.HOLDING.Reason,
                                OnHold = x.HOLDING.OnHold,
                                Acquired = x.HOLDING.Acquired,
                                Availlable = x.HOLDING.Availlable,
                            }).ToList();
                    }
                    else
                    {
                        holdings = db.HOLDINGs.Join(db.HOLDING_LOCATION, holding => holding.LocationID, loc => loc.ID, (holding, loc) => new { HOLDING = holding, HOLDING_LOCATION = loc })
                            .Where(y => y.HOLDING.ItemID == itemID).OrderByDescending(o => o.HOLDING_LOCATION.Symbol).ToList().Select(x => new HOLDING()
                            {
                                ID = x.HOLDING.ID,
                                LibID = x.HOLDING.LibID,
                                LocationID = x.HOLDING.LocationID,
                                AcquiredDate = x.HOLDING.AcquiredDate,
                                AcquiredSourceID = x.HOLDING.AcquiredSourceID,
                                CallNumber = x.HOLDING.CallNumber,
                                CopyNumber = x.HOLDING.CopyNumber,
                                Currency = x.HOLDING.Currency,
                                Note = x.HOLDING.Note,
                                Price = x.HOLDING.Price,
                                ReceiptedDate = x.HOLDING.ReceiptedDate,
                                RecordNumber = x.HOLDING.RecordNumber,
                                Shelf = x.HOLDING.Shelf,
                                Volume = x.HOLDING.Volume,
                                ItemID = x.HOLDING.ItemID,
                                UseCount = x.HOLDING.UseCount,
                                InUsed = x.HOLDING.InUsed,
                                InCirculation = x.HOLDING.InCirculation,
                                ILLID = x.HOLDING.ILLID,
                                POID = x.HOLDING.POID,
                                DateLastUsed = x.HOLDING.DateLastUsed,
                                LoanTypeID = x.HOLDING.LoanTypeID,
                                LockedReason = x.HOLDING.LockedReason,
                                IsLost = x.HOLDING.IsLost,
                                IsConfusion = x.HOLDING.IsConfusion,
                                Rate = x.HOLDING.Rate,
                                Reason = x.HOLDING.Reason,
                                OnHold = x.HOLDING.OnHold,
                                Acquired = x.HOLDING.Acquired,
                                Availlable = x.HOLDING.Availlable,
                            }).ToList();
                    }
                }
                else if (sortColumn.Equals("Status"))
                {
                    if (sortColumnDir.Equals("asc"))
                    {
                        holdings = db.HOLDINGs.Where(h => h.ItemID == itemID).OrderBy( o => o.Acquired ).ThenBy( o => o.InCirculation ).ThenBy(o=> o.InUsed).ToList();
                    }
                    else 
                    {
                        holdings = db.HOLDINGs.Where(h => h.ItemID == itemID).OrderByDescending(o => o.Acquired).ThenByDescending(o => o.InCirculation).ThenByDescending(o => o.InUsed).ToList();
                    }
                }
                #endregion  
                else
                    holdings = db.HOLDINGs.SqlQuery("Select * from HOLDING where ItemID=" + itemID + " order by " + sortColumn + " " + sortColumnDir).ToList();
            }
            
            //Search    
            if (!string.IsNullOrEmpty(searchValue))
            {
                holdings = holdings.Where(m => m.CopyNumber.ToLower().Contains(searchValue.Trim().ToLower())).ToList();
            }

            //total number of rows count     
            RecordsTotal = holdings.Count();
            string codeItemToPrint = code;
            //Paging     
            var data = holdings.Skip(skip).Take(pageSize).ToList();
            List<HoldingTable> holdingTables = new List<HoldingTable>();
            
            foreach (var holding in data)
            {
                holdingTables.Add(new HoldingTable()
                {
                    ID = holding.ID,
                    LibID = holding.LibID,
                    LocID = holding.LocationID,
                    Library = db.HOLDING_LIBRARY.Where(h => h.ID == holding.LibID).Select(h => h.AccessEntry).FirstOrDefault(),
                    Location = db.HOLDING_LOCATION.Where(h => h.ID == holding.LocationID).Select(h => h.Symbol).FirstOrDefault(),
                    AcquiredDate = holding.AcquiredDate.Value.ToString("dd/MM/yyyy"),
                    AcquiredSource = db.ACQ_ACQUIRE_SOURCE.Where(h => h.ID == holding.AcquiredSourceID).Select(h => h.Source).FirstOrDefault(),
                    CallNumber = holding.CallNumber,
                    CopyNumber = holding.CopyNumber,
                    Currency = holding.Currency,
                    Volume = holding.Volume,
                    Note = holding.Note,
                    Price = holding.Price,
                    ReceiptedDate = holding.ReceiptedDate != null ? holding.ReceiptedDate.Value.ToString("dd/MM/yyyy"): "" ,
                    RecordNumber = holding.RecordNumber,
                    Shelf = holding.Shelf,
                    Status = shelfBusiness.GetHoldingStatus(holding.InUsed, holding.InCirculation.Value, holding.Acquired),
                });
                
            }

            //Returning Json Data    
            return Json(new { draw = draw, recordsFiltered = RecordsTotal, recordsTotal = RecordsTotal, data = holdingTables, numberOfFreeCopies = numberFreeCopies, compositeHolding = compositeHoldingData, numberRecord= numberOfRecord, Message = message, codeItemToPrint = codeItemToPrint });
        }

        

        [HttpPost]
        public JsonResult SearchItem(string title,string copynumber, string author,string publisher,string year,string isbn)
        {
            List<SP_GET_TITLES_Result> data= null;
            string message = shelfBusiness.SearchItem(title.Trim(), copynumber.Trim(), author.Trim(), publisher.Trim(), year.Trim(), isbn.Trim(), ref data);
            return Json(new { Message = message, data = data }, JsonRequestBehavior.AllowGet);
        }


    }

    public class HoldingStatus
    {
        public int HoldingID { get; set; }
        public int LibID { get; set; }
        public int LocID { get; set; }
        public string Shelf { get; set; }
        public int Mode { get; set; }
        public int IsNew { get; set; }
    }

    public class HoldingTable
    {
        public int ID { get; set; }
        public int LibID { get; set; }
        public int LocID { get; set; }
        public string Library { get; set; }
        public string Location { get; set; }
        public string Shelf { get; set; }
        public string Volume { get; set; }
        public string CallNumber { get; set; }
        public string CopyNumber { get; set; }
        public float? Price { get; set; }
        public string Currency { get; set; }
        public string RecordNumber { get; set; }
        public string ReceiptedDate { get; set; }
        public string AcquiredDate { get; set; }
        public string AcquiredSource { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }


    }





}
