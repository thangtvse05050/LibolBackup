using Libol.EntityResult;
using Libol.Models;
using Libol.SupportClass;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Libol.Controllers
{
    public class AcquisitionController : Controller
    {
        private LibolEntities db = new LibolEntities();
        ShelfBusiness shelfBusiness = new ShelfBusiness();
        AcquisitionBusiness ab = new AcquisitionBusiness();

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

        // Liquidate : thanh ly
        [HttpPost]
        public JsonResult Liquidate(string Copynumber, string DKCB, string Liquidate, string DateLiquidate, int Reason ,string selectfile)
        {
            int IDCN = -1;
            if (Copynumber != "" && Copynumber != null)
            {
                if (db.ITEMs.Where(a => a.Code == Copynumber).Count() == 0)
                {
                    ViewBag.Liquidate = "Mã tài liệu : " + Copynumber + " không tồn tại";
                }
                else
                {
                    IDCN = db.ITEMs.Where(a => a.Code == Copynumber).First().ID;
                    if (db.CIR_LOAN.Where(a => a.ItemID == IDCN).Count() != 0)
                    {
                        ViewBag.Liquidate = "Không thể Thanh Lý vì vẫn còn sách đang lưu thông";
                    }
                    else
                    {
                        string formatDKCB = "";
                        ViewBag.Liquidate = db.SP_HOLDING_REMOVED_LIQUIDATE(Liquidate, DateLiquidate, Copynumber, formatDKCB, Reason, new ObjectParameter("intTotalItem", typeof(int)),
                            new ObjectParameter("intOnLoan", typeof(int)),
                            new ObjectParameter("intOnInventory", typeof(int))).ToList();
                        ViewBag.Liquidate = "Thanh lý thành công";
                    }
                }
            }
            else
            {
                if (Copynumber == "" && DKCB== "")
                {
                    ViewBag.Liquidate = "Không thể thanh lý vì chưa nhập thông tin";
                }
                else
                {
                    string formatDKCB = DKCB.Replace('\n', ',');
                    formatDKCB = formatDKCB.Replace("\t", "");
                    ViewBag.Liquidate = db.SP_HOLDING_REMOVED_LIQUIDATE(Liquidate, DateLiquidate, Copynumber, formatDKCB, Reason, new ObjectParameter("intTotalItem", typeof(int)),
                        new ObjectParameter("intOnLoan", typeof(int)),
                        new ObjectParameter("intOnInventory", typeof(int))).ToList();
                    ViewBag.Liquidate = "Thanh lý thành công";
                }
                
            }
            
            return Json(ViewBag.Liquidate, JsonRequestBehavior.AllowGet);

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

        public PartialViewResult GetInventoryReport(string strInventoryID, string strLibID, string strDKCBID)
        {
            
            strDKCBID = strDKCBID.Trim();
            string[] myList = strDKCBID.Split('\n');
            int countCN = myList.Length;
            int libid = 0, invenid = 0;
            if(strDKCBID.Equals(" "))
            {
                strDKCBID = strDKCBID.Trim();
            }
            if (strLibID != "")
            {
                libid = Convert.ToInt32(strLibID);
            } 
            if (strInventoryID != "")
            {
                invenid = Convert.ToInt32(strInventoryID);
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
            ViewBag.totalInLibrary = totalInLib.ToString();
            ViewBag.totalReLibrary = totalReLib.ToString();
            List<FPT_SP_INVENTORY_Result> listData = ab.FPT_SP_INVENTORY(libid);

            List<string> listStr = myList.ToList();

            if (myList.Length != 0)
            {

                for (int j = 0; j < listData.Count; j++)
                {
                    for (int i = 0; i < listStr.Count; i++)
                    {
                        if (listData[j].CopyNumber == listStr[i])
                        {
                            listStr.RemoveAt(i);
                            listData.RemoveAt(j);
                        }
                    }

                }


            }
            if (listData.Count > 0)
            {
                ViewBag.LackDataResult = listData;
                ViewBag.totalLack = listData.Count.ToString();
            }
            else
            {
                ViewBag.LackDataResult = null;
                ViewBag.totalLack = "0";
            }

            if (listStr.Count > 0)
            {
                ViewBag.ExcessDataResult = listStr;
                ViewBag.totalEX = listStr.Count.ToString();
            }
            else
            {
                ViewBag.ExcessDataResult = null;
                ViewBag.totalEX = "0";
            }

            return PartialView("GetInventoryReport");
        }
    }
}