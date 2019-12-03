using Libol.EntityResult;
using Libol.Models;
using Libol.SupportClass;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Libol.Controllers
{
	public class AcquisitionController : Controller
	{
		private LibolEntities db = new LibolEntities();
		ShelfBusiness shelfBusiness = new ShelfBusiness();
		AcquisitionBusiness ab = new AcquisitionBusiness();
		private static string statust = "notcheck";

		[AuthAttribute(ModuleID = 4, RightID = "32")]
		public ActionResult HoldingLocRemove()
		{
			ViewBag.Library = shelfBusiness.FPT_SP_HOLDING_LIBRARY_SELECT(0, 1, -1, (int)Session["UserID"], 1);
			ViewData["ListReason"] = db.SP_HOLDING_REMOVE_REASON_SEL(0).ToList();
			return View();
		}

		[HttpPost]
		public JsonResult OnchangeLibrary(int LibID)
		{
			List<SP_HOLDING_LOCATION_GET_INFO_Result> list = shelfBusiness.FPT_SP_HOLDING_LOCATION_GET_INFO(LibID, (int)Session["UserID"], 0, -1);
			return Json(list, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Liquidate(string CopyNumber, string DKCB, string Liquidate, string DateLiquidate, int Reason, string selectfile, int libID, int libID2)
        {
            List<string> contentOnLoan = null;
            List<string> contentExists = null;
            int? result = 0;
            int? result2 = 0;
            int? result3 = 0;
            int? result4 = 0;
            int IDCN = -1;
            if (CopyNumber != "" && CopyNumber != null)
            {
                if (db.FPT_CHECK_ITEM_COPYNUMBER_EXISTS(CopyNumber, libID).FirstOrDefault() == 0)
                {
                    ViewBag.Liquidate = "Mã tài liệu : " + CopyNumber + " không tồn tại";

                }
                else
                {
                    IDCN = db.ITEMs.Where(a => a.Code == CopyNumber).First().ID;
                    //if (db.CIR_LOAN.Where(a => a.ItemID == IDCN).Count() != 0)
                    //{
                    //    result = db.HOLDINGs.Where(a => a.ItemID == IDCN).Count();
                    //    result2 = db.CIR_LOAN.Where(a => a.ItemID == IDCN).Count();
                    //    result3 = db.FPT_COUNT_HOLDING_REMOVE(Copynumber).FirstOrDefault();
                    //    /* List<string>*/
                    //    content = db.CIR_LOAN.Where(a => a.ItemID == IDCN).Select(a => a.CopyNumber).ToList();
                    //    //result = string.Join(",", b.ToArray());
                    //    ViewBag.Liquidate = "Không thể Thanh Lý vì vẫn còn sách đang lưu thông";

                    //}
                    if (db.FPT_CHECK_LOAN_COPYNUMBER(IDCN, libID).FirstOrDefault() != 0)
                    {
                        result = db.FPT_CHECK_HOLDING_COPYNUMBER(IDCN, libID).FirstOrDefault();
                        result2 = db.FPT_CHECK_LOAN_COPYNUMBER(IDCN, libID).FirstOrDefault();
                        result3 = db.FPT_COUNT_HOLDING_REMOVE(IDCN, libID).FirstOrDefault();
                        /* List<string>*/
                        //content = db.CIR_LOAN.Where(a => a.ItemID == IDCN).Select(a => a.CopyNumber).ToList();
                        contentOnLoan = db.FPT_COPY_NUMBER_ONLOAN(IDCN, libID).ToList();
                        //result = string.Join(",", b.ToArray());
                        ViewBag.Liquidate = "Không thể Thanh Lý vì vẫn còn sách đang lưu thông";

                    }
                    else
                    {
                        string formatDKCB = "";
                        result = db.FPT_CHECK_HOLDING_COPYNUMBER(IDCN, libID).FirstOrDefault();
                        result2 = db.FPT_CHECK_LOAN_COPYNUMBER(IDCN, libID).FirstOrDefault();

                        contentOnLoan = db.SP_HOLDING_REMOVED_LIQUIDATE2(Liquidate, DateLiquidate, CopyNumber, formatDKCB, Reason, new ObjectParameter("intTotalItem", typeof(int)),
                            new ObjectParameter("intOnLoan", typeof(int)),
                            new ObjectParameter("intOnInventory", typeof(int)), libID).ToList();
                        result3 = db.FPT_COUNT_HOLDING_REMOVE(IDCN, libID).FirstOrDefault();
                        ViewBag.Liquidate = "Thanh lý thành công";
                    }
                }
            }
            else
            {
                if (CopyNumber == "" && DKCB == "")
                {
                    ViewBag.Liquidate = "Không thể thanh lý vì chưa nhập thông tin";
                }
                else
                {
                    List<string> list1 = db.FPT_GET_ALL_COPYNUMBER_BY_LIBID(libID2).ToList();


                    string formatDKCB = DKCB.Replace('\n', ',');
                    formatDKCB = formatDKCB.Replace("\t", "");
                    char[] splitchar = { ',' };
                    List<string> list2 = formatDKCB.Split(splitchar).ToList();
                    result = list2.Count();
                    contentExists = list2.Where(i => !list1.Any(e => i.Contains(e))).ToList();
                    result4 = contentExists.Count();
                    List<string> list3 = list2.Where(i => list1.Any(e => i.Contains(e))).ToList();
                    List<string> listOnLoan = db.FPT_GET_ALL_COPYNUMBER_ONLOAN_BY_LIBID(libID2).ToList();
                    contentOnLoan = list3.Where(i => listOnLoan.Any(e => i.Contains(e))).ToList();
                    result2 = contentOnLoan.Count();
                    List<string> finalList = list3.Where(i => !listOnLoan.Any(e => i.Contains(e))).ToList();
                    result3 = finalList.Count();
                    string finalDKCB = String.Join(",", finalList);
                    ViewBag.Liquidate = db.SP_HOLDING_REMOVED_LIQUIDATE2(Liquidate, DateLiquidate, CopyNumber, finalDKCB, Reason, new ObjectParameter("intTotalItem", typeof(int)),
                        new ObjectParameter("intOnLoan", typeof(int)),
                       new ObjectParameter("intOnInventory", typeof(int)), libID2).ToList();
                    ViewBag.Liquidate = "Thanh lý thành công";
                }

            }

            return Json(new Responses { Message = ViewBag.Liquidate, ContentOnLoan = contentOnLoan, ContentExists = contentExists, totalOnloan = result2, total = result, totalSuccess = result3, totalExists = result4 }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult LiquidateByExcel()
        {
            db.FPT_REFRESH_TABLE_DATA();
            List<CopyNumberFile> listCopyInFile = new List<CopyNumberFile>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileName = Path.GetFileName(file.FileName);
                if (String.IsNullOrEmpty(fileName))
                {
                    ViewBag.CopyNumberInFile = listCopyInFile;

                    return View();
                }
                var path = Path.Combine(Server.MapPath("~/CopyNumberExcel/"), fileName);
                file.SaveAs(path);

                FileInfo excel = new FileInfo(Server.MapPath("/CopyNumberExcel/" + fileName));
                using (var package = new ExcelPackage(excel))
                {
                    var workbook = package.Workbook;

                    //*** Sheet 1
                    var worksheet = workbook.Worksheets.FirstOrDefault();

                    //*** Retrieve to List                    
                    int totalRows = worksheet.Dimension.End.Row;
                    for (int u = 2; u <= totalRows; u++)
                    {
                        if (!String.IsNullOrEmpty(worksheet.Cells[u, 1].Text.ToString()))
                        {

                            listCopyInFile.Add(new CopyNumberFile
                            {
                                ID = Convert.ToInt32(worksheet.Cells[u, 1].Text.ToString()),
                                CopyNumber = worksheet.Cells[u, 4].Text.ToString(),
                                LiquidCode = worksheet.Cells[u, 5].Text.ToString(),
                                Reason = worksheet.Cells[u, 7].Text.ToString(),
                            });
                        }

                    }
                }

            }

            ViewBag.CopyNumberInFile = listCopyInFile;




            if (listCopyInFile != null)
            {
                foreach (CopyNumberFile p in listCopyInFile)
                {

                    CopyNumberInFile(p.ID, p.CopyNumber, p.LiquidCode, p.Reason);
                }
            }
            return View();
        }
        [HttpPost]

        public JsonResult CopyNumberInFile(int ID, string CopyNumber, string LiquidCode, string Reason)
        {
            db.FPT_LOAD_DATA_TO_DB(ID, CopyNumber, LiquidCode, Reason);
            ViewBag.Success = "Load thành công!";
            return Json(ViewBag.Success, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public JsonResult LiquidateExcel()
        {

            db.UPDATE_REMOVE_REASON_EXCEL();
            List<string> listCopy = db.HOLDING_TO_REMOVE_EXCEL.Select(a => a.CopyNumber).ToList();
            int? result = listCopy.Count();
            List<string> listHolding = db.HOLDINGs.Select(a => a.CopyNumber).ToList();
            List<string> contentExists = listCopy.Where(i => !listHolding.Any(e => i.Equals(e))).ToList();
            int? result4 = contentExists.Count();
            List<string> list3 = listCopy.Where(i => listHolding.Any(e => i.Equals(e))).ToList();
            List<string> listOnLoan = db.CIR_LOAN.Select(a => a.CopyNumber).ToList();
            List<string> contentOnLoan = list3.Where(i => listOnLoan.Any(e => i.Equals(e))).ToList();
            int? result2 = contentOnLoan.Count();
            List<string> finalList = list3.Where(i => !listOnLoan.Any(e => i.Equals(e))).ToList();
            int? result3 = finalList.Count();
            string finalDKCB = ";" + String.Join(";", finalList) + ";";
            //List<CopyNumberFile> listToLiquid= db.HOLDING_TO_REMOVE_EXCEL.Where(a=>a.CopyNumber== finalList)
            db.FPT_LIQUID_BY_EXCEL(finalDKCB);
            ViewBag.Liquidate = "Thanh lý thành công!";
            return Json(new Responses { Message = ViewBag.Liquidate, ContentOnLoan = contentOnLoan, ContentExists = contentExists, totalOnloan = result2, total = result, totalSuccess = result3, totalExists = result4 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddReason(string reason)
        {
            int id = db.HOLDING_REMOVE_REASON.Select(a => a.ID).Count();
            ViewBag.Success = db.FPT_ADD_REASON(id + 1, reason);
            ViewBag.Success = "Thêm thành công";

            return Json(ViewBag.Success, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
		public JsonResult SearchCode(string strCode, string strCN, string strTT, string ISBN)
		{
			List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> inforList = ab.SearchCode(strCode, strCN, strTT, ISBN);
			return Json(inforList, JsonRequestBehavior.AllowGet);
		}

		[AuthAttribute(ModuleID = 1, RightID = "15")]
		public PartialViewResult AddNewCatalogueDetail()
		{
			string Id = Request["ID"];
			ViewBag.SearchItemResult = null;
			if (!String.IsNullOrEmpty(Id))
			{
				List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> listContent = ab.GetContentByID(Id).ToList();
				if (listContent.Count == 0) return PartialView("SearchItemCode");
				ViewBag.SearchItemResult = listContent[1].Content;
				
			}
			else
			{
				return PartialView("SearchItemCode");
			}
			List<SelectListItem> inven = new List<SelectListItem>();
			foreach (var l in db.SP_ACQ_INVENTORY_GET(0, 0))
			{
				inven.Add(new SelectListItem { Text = l.Name, Value = l.ID.ToString() });
			}
			ViewData["inven"] = inven;
			List<SelectListItem> lib = new List<SelectListItem>();
			lib.Add(new SelectListItem { Text = "Hãy chọn thư viện", Value = "" });
			foreach (var l in db.SP_HOLDING_LIB_SEL((int)Session["UserID"]).ToList())
			{
				lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
			}
			ViewData["lib"] = lib;
			return PartialView("InventoryReportByItemID");
		}
		
		[HttpPost]
		public JsonResult SearchItem(string title, string copynumber, string author, string publisher, string year, string isbn)
		{
			List<SP_GET_TITLES_Result> data = null;
			string message = shelfBusiness.SearchItem(title.Trim(), copynumber.Trim(), author.Trim(), publisher.Trim(), year.Trim(), isbn.Trim(), ref data);
			return Json(new { Message = message, data = data }, JsonRequestBehavior.AllowGet);
		}

		// Open Location
		public ActionResult OpenLoc(string libID, string ckc)
		{
			List<SelectListItem> lib = new List<SelectListItem>();
			if (String.IsNullOrEmpty(libID))
			{
				lib.Add(new SelectListItem { Text = "Hãy chọn thư viện", Value = "0" });
			}
			else
			{
				lib.Add(new SelectListItem { Text = "Hãy chọn thư viện", Value = libID });
			}
			foreach (var l in ab.SP_HOLDING_LIBRARY_SELECT_LIST(-1, 1, -1, (int)Session["UserID"], 1).ToList())
			{
				lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
				if (libID == l.ID.ToString())
				{
					lib[0].Text = l.Code;
				}
			}

			ViewData["lib"] = lib;
			if (!String.IsNullOrEmpty(libID))
			{

				ViewBag.Result = ab.SP_HOLDING_LOCATION_GET_INFO_LIST(Convert.ToInt32(libID), (int)Session["UserID"], 0, 0).ToList();

			}
			return View();

		}

		[HttpPost]
		public JsonResult OpenLocation(string libID, string strLocID)
		{
			string strShelf = "";
			int intStatus = 1;
			strLocID = strLocID.Trim();
			strLocID = strLocID.Substring(0, strLocID.LastIndexOf(','));
			List<SP_HOLDING_LOCATION_UPD_STATUS_Result> listResult = new List<SP_HOLDING_LOCATION_UPD_STATUS_Result>();
			if (strLocID.Length > 0)
			{
				listResult = ab.SP_HOLDING_LOCATION_UPD_STATUS(strLocID, strShelf, intStatus);

			}

			return Json(listResult, JsonRequestBehavior.AllowGet);

		}

		// Close Location
		public ActionResult CloseLoc(string libID, string ckc)
		{
			List<SelectListItem> lib = new List<SelectListItem>();
			if (String.IsNullOrEmpty(libID))
			{
				lib.Add(new SelectListItem { Text = "Hãy chọn thư viện", Value = "0" });
			}
			else
			{
				lib.Add(new SelectListItem { Text = "Hãy chọn thư viện", Value = libID });
			}
			foreach (var l in ab.SP_HOLDING_LIBRARY_SELECT_LIST(-1, 1, -1, (int)Session["UserID"], 1).ToList())
			{
				lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
				if (libID == l.ID.ToString())
				{
					lib[0].Text = l.Code;
				}
			}

			ViewData["lib"] = lib;
			if (!String.IsNullOrEmpty(libID))
			{

				ViewBag.Result = ab.SP_HOLDING_LOCATION_GET_INFO_LIST(Convert.ToInt32(libID), (int)Session["UserID"], 0, 1).ToList();

			}
			return View();

		}
		[HttpPost]
		public JsonResult CloseLocation(string libID, string strLocID)
		{
			string strShelf = "";
			int intStatus = 0;
			strLocID = strLocID.Trim();
			strLocID = strLocID.Substring(0, strLocID.LastIndexOf(','));
			//string[] myList = strLocID.Split('');
			List<SP_HOLDING_LOCATION_UPD_STATUS_Result> listResult = new List<SP_HOLDING_LOCATION_UPD_STATUS_Result>();
			if (strLocID.Length > 0)
			{
				listResult = ab.SP_HOLDING_LOCATION_UPD_STATUS(strLocID, strShelf, intStatus);

			}

			//ViewData["listResult"] = listResult;
			return Json(listResult, JsonRequestBehavior.AllowGet);

		}

		//Create Inventory
		public ActionResult CreateInventory()
		{
			ViewBag.uName = (string)Session["FullName"];
			return View();
		}
		[HttpPost]
		public JsonResult CreateInven(string nameIn, string inDate, string inputer)
		{
			ViewBag.intResult = db.FPT_SP_ACQ_NEW_INVENTORY(nameIn, inDate, inputer);
			return Json(ViewBag.intResult, JsonRequestBehavior.AllowGet);
		}
		//Close Inventory
		public ActionResult CloseInventory()
		{
			ViewBag.uName = (string)Session["FullName"];
			List<SelectListItem> inven = new List<SelectListItem>();
			foreach (var l in db.SP_ACQ_INVENTORY_GET(0, 0))
			{
				inven.Add(new SelectListItem { Text = l.Name, Value = l.ID.ToString() });
			}
			ViewData["inven"] = inven;
			return View();
		}

		[HttpPost]
		public JsonResult CloseInven(string InvenID)
		{
			try
			{
				int inventoryID = 0;
				if (InvenID != "")
				{
					inventoryID = Convert.ToInt32(InvenID);
				}
				ViewBag.intResult = db.SP_ACQ_CLOSE_INVENTORY(inventoryID);
			}
			catch (Exception ex)
			{
				ViewBag.Message = ex;
			}

			return Json(ViewBag.intResult, JsonRequestBehavior.AllowGet);
		}
		//kiem ke
		public ActionResult InventoryReport()
		{
			List<SelectListItem> inven = new List<SelectListItem>();
			foreach (var l in db.SP_ACQ_INVENTORY_GET(0, 0))
			{
				inven.Add(new SelectListItem { Text = l.Name, Value = l.ID.ToString() });
			}
			ViewData["inven"] = inven;
			List<SelectListItem> lib = new List<SelectListItem>();
			lib.Add(new SelectListItem { Text = "Hãy chọn thư viện", Value = "" });
			foreach (var l in db.SP_HOLDING_LIB_SEL((int)Session["UserID"]).ToList())
			{
				lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
			}
			ViewData["lib"] = lib;
			return View();
		}


		public PartialViewResult GetInventoryReport(string strInventoryID01, string strLibID01, string strLocPrefix, string strLocID, string strDKCBID01, string strItemID)
		{

			strDKCBID01 = strDKCBID01.Trim();
			string[] myList = strDKCBID01.Split('\n');
			int countCN = myList.Length;
			int libid = 0, invenid = 0;
			if (strDKCBID01.Equals(" "))
			{
				strDKCBID01 = strDKCBID01.Trim();
			}
			if (strLibID01 != "")
			{
				libid = Convert.ToInt32(strLibID01);
			}
			if (strInventoryID01 != "")
			{
				invenid = Convert.ToInt32(strInventoryID01);
			}
			int cirCount = 0;
			int totalInLib = 0, totalReLib = 0;
			//get and set inventorytime
			int intInventoryTime = 0;
			foreach (var item in db.FPT_SP_ACQ_GETMAXID_HINT())
			{
				intInventoryTime = item.Value;
			}
			intInventoryTime = intInventoryTime + 1;
			//add table
			ViewBag.intResult = db.SP_ACQ_RUN_INVENTORY(0, libid, 1, invenid, "", intInventoryTime, 1, 0);
			//exe inventory
			List<FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_Result> listCountResult = ab.FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_LIST(libid, 0, null, 1);
			foreach (var item in listCountResult)
			{
				if (item.Type == "CountCir")
				{
					cirCount = Convert.ToInt32(item.VALUE);
				}

				if (item.Type == "SUMCOPY")
				{
					totalInLib = Convert.ToInt32(item.VALUE);
				}
			}
			totalReLib = countCN + cirCount;
			//ViewBag.totalInLibrary = totalInLib.ToString();
			//ViewBag.totalReLibrary = totalReLib.ToString();
			ViewBag.totalInLibrary = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 2, statust).Count.ToString();
			ViewBag.totalOnLoan = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 1, statust).Count.ToString();

			List<FPT_SP_INVENTORY_Result> listData = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 0, statust);

			List<string> listStr = myList.ToList();
			ViewBag.totalCheck = listStr.Count();
			List<string> tempLstr = myList.ToList();
			List<FPT_SP_INVENTORY_Result> listDataTemp = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 0, statust);
			List<string> strDuplicate = new List<string>();



			if (myList.Length != 0)
			{

				for (int j = 0; j < listData.Count; j++)
				{

					for (int i = 0; i < listStr.Count; i++)
					{
						if (listData[j].CopyNumber.Equals(listStr[i].Trim()))
						{
							tempLstr.Remove(listStr[i]);
							strDuplicate.Add(listStr[i].Trim());
							foreach (FPT_SP_INVENTORY_Result ob in listDataTemp)
							{
								if (ob.CopyNumber.Equals(listData[j].CopyNumber))
								{
									listDataTemp.Remove(ob);
									break;
								}

							}

						}

					}

				}
			}

			List<string> strNumDup = new List<string>();
			for (int i = 0; i < strDuplicate.Count - 1; i++)
			{
				for (int j = i + 1; j < strDuplicate.Count; j++)
				{
					if (strDuplicate[i].Trim().Equals(strDuplicate[j].Trim()))
					{
						if (!strNumDup.Contains(listStr[i].Trim()))
						{
							strNumDup.Add(strDuplicate[i]);
						}

					}
				}
			}
			ViewBag.totalDuplicate = strNumDup.Count();

			if (listDataTemp.Count > 0)
			{
				ViewBag.LackDataResult = listDataTemp;
				ViewBag.totalLack = listDataTemp.Count.ToString();
			}
			else
			{
				ViewBag.LackDataResult = null;
				ViewBag.totalLack = "0";
			}

			if (tempLstr.Count > 0)
			{
				ViewBag.ExcessDataResult = tempLstr;
				ViewBag.totalEX = tempLstr.Count.ToString();
			}
			else
			{
				ViewBag.ExcessDataResult = null;
				ViewBag.totalEX = "0";
			}

			return PartialView("GetInventoryReport");
		}
		public ActionResult InventoryReportByItemID()
		{
			List<SelectListItem> inven = new List<SelectListItem>();
			foreach (var l in db.SP_ACQ_INVENTORY_GET(0, 0))
			{
				inven.Add(new SelectListItem { Text = l.Name, Value = l.ID.ToString() });
			}
			ViewData["inven"] = inven;
			List<SelectListItem> lib = new List<SelectListItem>();
			lib.Add(new SelectListItem { Text = "Hãy chọn thư viện", Value = "" });
			foreach (var l in db.SP_HOLDING_LIB_SEL((int)Session["UserID"]).ToList())
			{
				lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
			}
			ViewData["lib"] = lib;
			ViewBag.SearchItemResult = null;
			return View();
		}

		public ActionResult SearchItemCode()
		{
			
			return View();
		}
		public PartialViewResult GetInventoryReportByItemID(string strInventoryID01, string strLibID01, string strLocPrefix, string strLocID, string strDKCBID01, string strItemID)
		{

			strDKCBID01 = strDKCBID01.Trim();
			string[] myList = strDKCBID01.Split('\n');
			int countCN = myList.Length;
			int libid = 0, invenid = 0;
			if (strDKCBID01.Equals(" "))
			{
				strDKCBID01 = strDKCBID01.Trim();
			}
			if (strLibID01 != "")
			{
				libid = Convert.ToInt32(strLibID01);
			}
			if (strInventoryID01 != "")
			{
				invenid = Convert.ToInt32(strInventoryID01);
			}
			int cirCount = 0;
			int totalInLib = 0, totalReLib = 0;
			//get and set inventorytime
			int intInventoryTime = 0;
			foreach (var item in db.FPT_SP_ACQ_GETMAXID_HINT())
			{
				intInventoryTime = item.Value;
			}
			intInventoryTime = intInventoryTime + 1;
			//add table
			ViewBag.intResult = db.SP_ACQ_RUN_INVENTORY(0, libid, 1, invenid, "", intInventoryTime, 1, 0);
			//exe inventory
			List<FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_Result> listCountResult = ab.FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_LIST(libid, 0, null, 1);
			foreach (var item in listCountResult)
			{
				if (item.Type == "CountCir")
				{
					cirCount = Convert.ToInt32(item.VALUE);
				}

				if (item.Type == "SUMCOPY")
				{
					totalInLib = Convert.ToInt32(item.VALUE);
				}
			}
			totalReLib = countCN + cirCount;
			//ViewBag.totalInLibrary = totalInLib.ToString();
			//ViewBag.totalReLibrary = totalReLib.ToString();
			ViewBag.totalInLibrary = ab.FPT_SP_INVENTORY(libid, "0", "", 2, strItemID).Count.ToString();
			ViewBag.totalOnLoan = ab.FPT_SP_INVENTORY(libid, "0", "", 1, strItemID).Count.ToString();

			List<FPT_SP_INVENTORY_Result> listData = ab.FPT_SP_INVENTORY(libid, "0", "", 0, strItemID);

			List<string> listStr = myList.ToList();
			ViewBag.totalCheck = listStr.Count();
			List<string> tempLstr = myList.ToList();
			List<FPT_SP_INVENTORY_Result> listDataTemp = ab.FPT_SP_INVENTORY(libid, "0", "", 0, strItemID);
			List<string> strDuplicate = new List<string>();



			if (myList.Length != 0 && listDataTemp.Count != 0)
			{

				for (int j = 0; j < listData.Count; j++)
				{

					for (int i = 0; i < listStr.Count; i++)
					{
						if (listData[j].CopyNumber.Equals(listStr[i].Trim()))
						{
							tempLstr.Remove(listStr[i]);
							strDuplicate.Add(listStr[i].Trim());
							foreach (FPT_SP_INVENTORY_Result ob in listDataTemp)
							{
								if (ob.CopyNumber.Equals(listData[j].CopyNumber))
								{
									listDataTemp.Remove(ob);
									break;
								}

							}

						}

					}

				}
			}

			List<string> strNumDup = new List<string>();
			for (int i = 0; i < strDuplicate.Count - 1; i++)
			{
				for (int j = i + 1; j < strDuplicate.Count; j++)
				{
					if (strDuplicate[i].Trim().Equals(strDuplicate[j].Trim()))
					{
						if (!strDuplicate.Contains(listStr[i].Trim()))
						{
							strNumDup.Add(strDuplicate[i]);
						}

					}
				}
			}
			ViewBag.totalDuplicate = strNumDup.Count();

			if (listDataTemp.Count > 0)
			{
				ViewBag.LackDataResult = listDataTemp;
				ViewBag.totalLack = listDataTemp.Count.ToString();
			}
			else
			{
				ViewBag.LackDataResult = null;
				ViewBag.totalLack = "0";
			}

			if (tempLstr.Count > 0)
			{
				ViewBag.ExcessDataResult = tempLstr;
				ViewBag.totalEX = tempLstr.Count.ToString();
			}
			else
			{
				ViewBag.ExcessDataResult = null;
				ViewBag.totalEX = "0";
			}

			return PartialView("GetInventoryReportByItemID");
		}
		public JsonResult GetLocationsPrefix(string id)
		{
			List<SelectListItem> LocPrefix = new List<SelectListItem>();
			LocPrefix.Add(new SelectListItem { Text = "Tất cả", Value = "0" });
			if (!string.IsNullOrEmpty(id))
			{
				foreach (var lp in db.FPT_CIR_GET_LOCLIBUSER_PREFIX_SEL((int)Session["UserID"], Int32.Parse(id)))
				{
					if (!lp.Contains("LV"))
					{
						LocPrefix.Add(new SelectListItem { Text = Regex.Replace(lp.ToString(), @"[^0-9a-zA-Z]+", ""), Value = lp.ToString() });
					}
				}
			}
			return Json(new SelectList(LocPrefix, "Value", "Text"));
		}


		//GET LOCATIONS BY LOCATION PREFIX, LIBRARY, USERID
		public JsonResult GetLocationsByPrefix(int id, string prefix)
		{
			List<SelectListItem> LocByPrefix = new List<SelectListItem>();
			LocByPrefix.Add(new SelectListItem { Text = "Tất cả", Value = "" });

			foreach (var lbp in db.FPT_CIR_GET_LOCFULLNAME_LIBUSER_SEL((int)Session["UserID"], id, prefix))
			{

				LocByPrefix.Add(new SelectListItem { Text = lbp.Symbol, Value = lbp.ID.ToString() });


			}
			if (id == 81)
			{
				foreach (var lbp in db.FPT_CIR_GET_LOCFULLNAME_LIBUSER_SEL((int)Session["UserID"], 20, prefix))
				{
					if (lbp.ID == 13 || lbp.ID == 15 || lbp.ID == 16 || lbp.ID == 27)
					{
						LocByPrefix.Add(new SelectListItem { Text = lbp.Symbol, Value = lbp.ID.ToString() });
					}
				}
				if (prefix.Equals("TK/"))
				{
					foreach (var lbp in db.FPT_CIR_GET_LOCFULLNAME_LIBUSER_SEL((int)Session["UserID"], id, "LV/"))
					{

						LocByPrefix.Add(new SelectListItem { Text = lbp.Symbol, Value = lbp.ID.ToString() });


					}
				}
			}

			return Json(new SelectList(LocByPrefix, "Value", "Text"));
		}

		public PartialViewResult RecordNotFound(string strInventoryID01, string strLibID01, string strLocPrefix, string strLocID, string strDKCBID01, string strItemID)
		{
			strDKCBID01 = strDKCBID01.Trim();
			string[] myList = strDKCBID01.Split('\n');
			int countCN = myList.Length;
			int libid = 0, invenid = 0;
			if (strDKCBID01.Equals(" "))
			{
				strDKCBID01 = strDKCBID01.Trim();
			}
			if (strLibID01 != "")
			{
				libid = Convert.ToInt32(strLibID01);
			}

			string modeCheck;
			if (strItemID == "")
			{
				modeCheck = statust;
			}
			else
			{
				modeCheck = strItemID;
			}

			List<FPT_SP_INVENTORY_Result> listData = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 0, modeCheck);

			List<string> listStr = myList.ToList();
			List<string> tempLstr = myList.ToList();
			List<FPT_SP_INVENTORY_Result> listDataTemp = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 0, modeCheck);
			if (myList.Length != 0)
			{

				for (int j = 0; j < listData.Count; j++)
				{

					for (int i = 0; i < listStr.Count; i++)
					{
						if (listData[j].CopyNumber.Equals(listStr[i].Trim()))
						{
							tempLstr.Remove(listStr[i]);
							listDataTemp.Remove(listData[j]);
						}
					}

				}


			}


			if (tempLstr.Count > 0)
			{
				ViewBag.ExcessDataResult = tempLstr;
				ViewBag.totalEX = tempLstr.Count.ToString();
			}
			else
			{
				ViewBag.ExcessDataResult = null;
				ViewBag.totalEX = "0";
			}
			return PartialView("RecordNotFound");
		}

		public PartialViewResult DuplicateCopyNumber(string strInventoryID01, string strLibID01, string strLocPrefix, string strLocID, string strDKCBID01, string strItemID)
		{
			strDKCBID01 = strDKCBID01.Trim();
			string[] myList = strDKCBID01.Split('\n');
			int countCN = myList.Length;
			int libid = 0, invenid = 0;
			if (strDKCBID01.Equals(" "))
			{
				strDKCBID01 = strDKCBID01.Trim();
			}
			if (strLibID01 != "")
			{
				libid = Convert.ToInt32(strLibID01);
			}
			string modeCheck;
			if (strItemID == "")
			{
				modeCheck = statust;
			}
			else
			{
				modeCheck = strItemID;
			}

			List<FPT_SP_INVENTORY_Result> listData = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 0, modeCheck);

			List<string> listStr = myList.ToList();
			List<string> tempLstr = new List<string>();
			List<FPT_SP_INVENTORY_Result> listDataTemp = ab.FPT_SP_INVENTORY(libid, strLocPrefix, strLocID, 0, modeCheck);
			List<DUPLICATE_INVENTORY> duplicates = new List<DUPLICATE_INVENTORY>();
			if (myList.Length != 0)
			{

				for (int j = 0; j < listData.Count; j++)
				{

					for (int i = 0; i < listStr.Count; i++)
					{
						if (listData[j].CopyNumber.Equals(listStr[i].Trim()))
						{
							tempLstr.Add(listStr[i].Trim());
						}
					}
				}
			}
			for (int i = 0; i < tempLstr.Count - 1; i++)
			{
				DUPLICATE_INVENTORY dUPLICATE_ = new DUPLICATE_INVENTORY();

				for (int j = i + 1; j < tempLstr.Count; j++)
				{
					if (tempLstr[i].Trim().Equals(tempLstr[j].Trim()))
					{

						dUPLICATE_.Copynumbername = tempLstr[i];
						dUPLICATE_.Duplicatetime++;
					}
				}

				if (dUPLICATE_.Duplicatetime > 0)
				{
					if (duplicates.Count == 0)
					{
						duplicates.Add(dUPLICATE_);
					}
					else
					{
						List<string> temp = new List<string>();
						foreach (DUPLICATE_INVENTORY dpc in duplicates)
						{
							temp.Add(dpc.Copynumbername);

						}
						if (!temp.Contains(dUPLICATE_.Copynumbername))
						{
							duplicates.Add(dUPLICATE_);

						}
					}


				}

			}

			if (duplicates.Count == 0)
			{
				ViewBag.Duplicates = null;
			}
			else
			{
				ViewBag.Duplicates = duplicates;
			}


			return PartialView("DuplicateCopyNumber");




		}
	}
    public class CopyNumberFile
    {
        public int ID { get; set; }
        public string CopyNumber { get; set; }
        public string LiquidCode { get; set; }
        public string Reason { get; set; }

    }
}