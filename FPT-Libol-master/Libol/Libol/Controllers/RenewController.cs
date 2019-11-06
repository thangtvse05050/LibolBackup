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
    public class RenewController : Controller
    {
        private LibolEntities db = new LibolEntities();
        RenewBusiness renewBusiness = new RenewBusiness();
        private static byte Type = 0;
        private static string CodeVal = "";
        private static string CodetogetLock = "";
        FormatHoldingTitle f = new FormatHoldingTitle();
        CirculationBusiness circulationBusiness = new CirculationBusiness();

        [AuthAttribute(ModuleID = 3, RightID = "18")]
        public ActionResult Renew()
        {
            CodeVal = "";
            return View();
        }

        [HttpPost]
        public PartialViewResult SearchToRenew(byte intType, string strCodeVal)
        {
            CodeVal = strCodeVal.Trim();
            if(intType == 1)
            {
                CheckLockPatron(strCodeVal);
                CodetogetLock = strCodeVal;
            }
            else if (intType == 3)
            {
                if(db.CIR_LOAN.Where(a => a.CopyNumber == strCodeVal).Count() != 0)
                {
                    string pccode = db.CIR_LOAN.Where(a => a.CopyNumber == strCodeVal).First().CIR_PATRON.Code;
                    CheckLockPatron(pccode);
                    CodetogetLock = pccode;
                }
                else
                {
                    ViewBag.active = 1;
                }
            }
            else
            {
                ViewBag.active = 1;
            }
            GetContentRenew((int)Session["UserID"], intType, strCodeVal);
            Type = intType;
            return PartialView("_searchToRenew");
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 3, RightID = "19")]
        public PartialViewResult Renew(int[] intLoanID, byte intAddTime, byte intTimeUnit, string strFixedDueDate, string[] duedates,string strCodeVal, int[] inttimes, int[] intrange)
        {
            int codeErrorCount = 0;
            if (intLoanID is null)
            {
                ViewBag.message = "Vui lòng chọn ấn phẩm cấn gia hạn";
            }
            else
            {
                if (intLoanID.Length == 1)
                {
                    int LoadID = intLoanID[0];
                    DateTime expiredDate = db.CIR_LOAN.Where(a => a.ID == LoadID).First().CIR_PATRON.ExpiredDate;
                    if (inttimes[0] >= intrange[0])
                    {
                        ViewBag.message = "Số lượt gia hạn đã đạt mức tối đa";
                    }
                    else if (Equals(strFixedDueDate, ""))
                    {
                        ViewBag.message = "Vui lòng chọn ngày gia hạn";
                    }
                    else if (DateTime.Compare(Convert.ToDateTime(strFixedDueDate), Convert.ToDateTime(duedates[0])) < 0)
                    {
                        ViewBag.message = "Ngày gia hạn sớm hơn hạn trả hiện tại";
                    }
                    else if (DateTime.Compare(expiredDate, Convert.ToDateTime(strFixedDueDate)) < 0)
                    {
                        ViewBag.message = "Ngày gia hạn muộn hơn ngày hết hạn thẻ";
                    }
                    else
                    {
                        ViewBag.message = "Gia hạn thành công";
                        db.SP_RENEW_ITEM(intLoanID[0], intAddTime, intTimeUnit, strFixedDueDate);
                    }
                }
                else if (intLoanID.Length > 1)
                {
                    for (int i = 0; i < intLoanID.Length; i++)
                    {
                        int LoadID = intLoanID[i];
                        DateTime expiredDate = db.CIR_LOAN.Where(a => a.ID == LoadID).First().CIR_PATRON.ExpiredDate;
                        if (inttimes[i] >= intrange[i])
                        {
                            codeErrorCount = codeErrorCount + 1;
                        }
                        else if (Equals(strFixedDueDate, ""))
                        {
                            codeErrorCount = codeErrorCount + 1;
                        }
                        else if (DateTime.Compare(Convert.ToDateTime(strFixedDueDate), Convert.ToDateTime(duedates[i])) < 0)
                        {
                            codeErrorCount = codeErrorCount + 1;
                        }
                        else if (DateTime.Compare(expiredDate, Convert.ToDateTime(strFixedDueDate)) < 0)
                        {
                            codeErrorCount = codeErrorCount + 1;
                        }
                        else
                        {
                            db.SP_RENEW_ITEM(intLoanID[i], intAddTime, intTimeUnit, strFixedDueDate);
                        }
                    }
                    if(codeErrorCount == 0)
                    {
                        ViewBag.message = "Gia hạn thành công"; 
                    }
                    else
                    {
                        ViewBag.message = "Gia hạn thất bại( " + codeErrorCount + " )bản ghi";
                    }
                    
                }
            }
            GetContentRenew((int)Session["UserID"], Type, strCodeVal);
            CheckLockPatron(CodetogetLock);
            return PartialView("_searchToRenew");
        }

        public JsonResult QuinkCheckInAndCheckOut(string strCopynumber, string strPatronCode, string strDueDate, string strCheckOutDate)
        {
            // ham nay khong can Trim() vi bien chuyen vao da dk format dung
            if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == strPatronCode).Count() != 0)
            {
                ViewBag.message = "Mượn lại thất bại vì thẻ đang bị khóa";
            }
            else { 
            db.SP_CHECKIN((int)Session["UserID"], 1, 0, strCopynumber, strCheckOutDate,
                    new ObjectParameter("strTransIDs", typeof(string)),
                    new ObjectParameter("strPatronCode", typeof(string)),
                    new ObjectParameter("intError", typeof(int)));
            db.SP_CHECKOUT(strPatronCode, (int)Session["UserID"], 1, strCopynumber, strDueDate, strCheckOutDate, 0,
                       new ObjectParameter("intOutValue", typeof(int)),
                        new ObjectParameter("intOutID", typeof(int)));
            ViewBag.message = "Mượn lại thành công";
            }
            return Json(ViewBag.message);
        }

        // Edit LockCard()
        [HttpPost]
        public JsonResult UpdatedLockCardPatron(string patronCode, int lockDays, string note)
        {
            string lnote = note.Trim();
            List<FPT_SP_UPDATE_UNLOCK_PATRON_CARD_Result> listResult = circulationBusiness.FPT_SP_UPDATE_UNLOCK_PATRON_CARD(patronCode, lockDays, lnote);
            ViewData["listResult"] = listResult;
            return Json(listResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetLockPatronInfo(string strCodeVal)
        {
            CIR_PATRON patron = db.CIR_PATRON.Where(a => a.Code == strCodeVal).First();
            string LPatronName = patron.FirstName + " " + patron.MiddleName + " " + patron.LastName;
            string LPatronCode = strCodeVal;
            ViewBag.blackstartdate = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().StartedDate;
            string Lblackstartdate = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().StartedDate.ToString("dd/MM/yyyy");
            string Lblackenddate = ViewBag.blackstartdate.AddDays(db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().LockedDays).ToString("dd/MM/yyyy");
            string LlockedDay = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().LockedDays.ToString();
            string LblackNote = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().Note;
            string[] PatronLockInfo = { LPatronName, LPatronCode, Lblackstartdate, Lblackenddate, LlockedDay, LblackNote };
			CodeVal = "";
            return Json(PatronLockInfo, JsonRequestBehavior.AllowGet);
        }

        private void GetContentRenew(int intUserID, byte intType, string strCodeVal)
        {
            List<SP_CIR_GET_RENEW_Result> results = renewBusiness.FPT_SP_CIR_GET_RENEW(intUserID, intType, strCodeVal);
            List<CustomRenew> customRenews = new List<CustomRenew>();
            foreach (SP_CIR_GET_RENEW_Result a in results)
            {
                customRenews.Add(new CustomRenew
                {
                    ID = a.ID,
                    DueDate = a.DueDate.ToString("yyyy-MM-dd"), // format khác để sử dụng trong insert db gọi từ hàm khác
                    Content = f.OnFormatHoldingTitle(a.Content),
                    DateRange = a.CheckOutDate.ToString("dd/MM/yyyy") + "-" + a.DueDate.ToString("dd/MM/yyyy"),
                    FullName = a.FullName,
                    RenewCount = a.RenewCount.ToString(),
                    Renewals = a.Renewals.ToString(),
                    CopyNumber = a.CopyNumber,
                    Code = a.Code,
                    OverDueDates = (DateTime.Now - a.DueDate).Days > 0 ? " ( " + (DateTime.Now - a.DueDate).Days.ToString() + " )" : "",
                    Note = (DateTime.Now - a.DueDate).Days < -3 ? "Chưa đến thời gian gia hạn" : (DateTime.Now - a.DueDate).Days > 0 ? "Số ngày quá hạn: " : ""
                });
            }
            ViewBag.ContentRenew = customRenews;
			if (customRenews.Count > 0) { ViewBag.CodePatron = customRenews[0].Code; }
			

		}


        private void CheckLockPatron(string code)
        {
            if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == code).Count() == 0)
            {
                ViewBag.active = 1;
            }
            else
            {
                ViewBag.active = 0;
            }
        }
        public JsonResult AutoComplete(string term)
        {
            var records = db.FPT_GET_DATE_SUGGEST_RENEW(term).Distinct().ToList();
            return Json(records);
    }

    }

    public class CustomRenew
    {
        public int ID { get; set; }
        public string DueDate { get; set; }
        public string Content { get; set; }
        public string DateRange { get; set; }
        public string FullName { get; set; }
        public string RenewCount { get; set; }
        public string Renewals { get; set; }
        public string CopyNumber { get; set; }
        public string Code { get; set; }
        public string OverDueDates { get; set; }
        public string Note { get; set; }
    }
   

}