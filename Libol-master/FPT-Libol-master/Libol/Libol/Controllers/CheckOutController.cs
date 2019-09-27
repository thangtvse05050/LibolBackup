using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libol.Models;
using Libol.EntityResult;
using System.Data;
using System.Data.Entity.Core.Objects;
using Libol.SupportClass;

namespace Libol.Controllers
{
    public class CheckOutController : Controller
    {
        private LibolEntities db = new LibolEntities();
        CheckOutBusiness checkOutBusiness = new CheckOutBusiness();
        SearchPatronBusiness searchPatronBusiness = new SearchPatronBusiness();
        private static string strTransactionIDs = "0";
        private static string patroncode = "0";
        private static string fullname = "";
        private static string sessionpcode = "";
        FormatHoldingTitle f = new FormatHoldingTitle();
        CirculationBusiness circulationBusiness = new CirculationBusiness();

        [AuthAttribute(ModuleID = 3, RightID = "16")]
        public ActionResult Index(string PatronCode)
        {
            strTransactionIDs = "0";
            patroncode = "0";
            sessionpcode = "";
            ViewBag.HiddenPatronCode = PatronCode;
            ViewBag.HiddenCheckduplicate = "";
            return View();
        }

        [HttpPost]
        public PartialViewResult CheckOutCardInfo(string strPatronCode)
        {
            string Patroncode = strPatronCode.Trim();
            if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == Patroncode).Count() == 0)
            {
                ViewBag.active = 1;
            }
            else
            {
                ViewBag.active = 0;
            }
            Getpatrondetail(Patroncode);
            sessionpcode = Patroncode;
            return PartialView("_showPatronInfo");
        }

        // GET: CheckOutSuccess
        [HttpPost]
        public PartialViewResult CheckOut(
            string strPatronCode,
            string strDueDate,
            int intLoanMode,
            int intHoldIgnore,
            string strCopyNumbers,
            string strCheckOutDate,
            bool boolAllowDuplacate
            )
        {
            string PatronCode = strPatronCode.Trim();
            bool duplicate = false;
            int PatronID = db.CIR_PATRON.Where(a => a.Code == PatronCode).First().ID;
            var cIR_LOANs = db.CIR_LOAN.Where(a => a.PatronID == PatronID).ToList();
            string CopyNumber = strCopyNumbers.Trim();
            List<int?> ItemIds = new List<int?>();
            Getpatrondetail(PatronCode);
            int onloan = db.SP_GET_PATRON_ONLOAN_COPIES(PatronID).ToList<SP_GET_PATRON_ONLOAN_COPIES_Result>().Count();

            //kiểm tra cùng 1 người thực hiện ghi mượn
            if (patroncode != PatronCode)
            {
                strTransactionIDs = "0";
            }
            if (db.HOLDINGs.Where(a => a.CopyNumber == CopyNumber).Count() == 0)
            {
                ViewBag.message = "ĐKCB không đúng";
                ViewBag.HiddenCheckduplicate = "";
            }
            else if (db.CIR_LOAN.Where(a => a.CopyNumber == CopyNumber).Count() != 0)
            {
                ViewBag.message = "ĐKCB đang được ghi mượn";
                ViewBag.HiddenCheckduplicate = "";
            }
            else
            {
                if (!Checkonloanquota(PatronID, intLoanMode))
                {
                    ViewBag.message = "Hết hạn ngạch tối đa có thể mượn";
                    ViewBag.HiddenCheckduplicate = "";
                }
                else
                {
                    int ItemID = db.HOLDINGs.Where(a => a.CopyNumber == CopyNumber).First().ItemID;
                    foreach (CIR_LOAN loan in cIR_LOANs)
                    {
                        ItemIds.Add(loan.ItemID);
                    }
                    foreach (int? id in ItemIds)
                    {
                        if (ItemID == id)
                        {
                            duplicate = true;
                        }
                    }
                    if (duplicate == true && boolAllowDuplacate == false)
                    {
                        ViewBag.HiddenCheckduplicate = "duplicate";
                    }
                    else
                    {
                        int success = db.SP_CHECKOUT(PatronCode, (int)Session["UserID"], intLoanMode, CopyNumber, strDueDate, strCheckOutDate, intHoldIgnore,
                           new ObjectParameter("intOutValue", typeof(int)),
                            new ObjectParameter("intOutID", typeof(int)));
                        string lastid = db.CIR_LOAN.Max(a => a.ID).ToString();

                        if (success == -1)
                        {
                            ViewBag.message = "Ghi mượn thất bại";
                        }
                        else
                        {
                            if (patroncode == PatronCode)
                            {
                                strTransactionIDs = strTransactionIDs + "," + lastid;
                            }
                            else
                            {
                                strTransactionIDs = lastid;
                            }
                        }
                        ViewBag.HiddenCheckduplicate = "";
                    }
                }
            }
            ViewBag.HiddenDuplicateCopyNumber = CopyNumber;
            Getcurrentloandetail();
            patroncode = PatronCode;
            return PartialView("_checkoutSuccess");
        }



        //thu hồi 1 ấn phẩm vừa mượn
        public PartialViewResult Rollbackacheckout(string strCopyNumbers)
        {
            db.SP_CHECKIN((int)Session["UserID"], 1, 0, strCopyNumbers, DateTime.Now.ToString("MM/dd/yyyy"),
               new ObjectParameter("strTransIDs", typeof(string)),
               new ObjectParameter("strPatronCode", typeof(string)),
               new ObjectParameter("intError", typeof(int)));

            strTransactionIDs = strTransactionIDs.Replace("," + strCopyNumbers, "");
            Getcurrentloandetail();
            Getpatrondetail(patroncode);
            return PartialView("_checkoutSuccess");
        }

        //thay đổi ghi chú của ấn phẩm đang mượn
        public PartialViewResult ChangeNote(string strCopyNumber, string strNote, string strDueDate)
        {
            int lngTransactionID = db.CIR_LOAN.Where(a => a.CopyNumber == strCopyNumber).First().ID;
            db.SP_UPDATE_CURRENT_LOAN(lngTransactionID, strNote, strDueDate);
            Getcurrentloandetail();
            Getpatrondetail(patroncode);
            return PartialView("_checkoutSuccess");
        }

		public PartialViewResult OpenPatronCode (string patroncode)
		{
			FPT_SP_UNLOCK_PATRON_CARD_LIST("'" + patroncode + "'");
			if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == patroncode).Count() == 0)
			{
				ViewBag.active = 1;
			}
			else
			{
				ViewBag.active = 0;
			}
			Getcurrentloandetail();
			Getpatrondetail(patroncode);
			return PartialView("_showPatronInfo");
		}

		public List<SP_UNLOCK_PATRON_CARD_Result> FPT_SP_UNLOCK_PATRON_CARD_LIST(string PatronCode)
		{
			List<SP_UNLOCK_PATRON_CARD_Result> list = db.Database.SqlQuery<SP_UNLOCK_PATRON_CARD_Result>("SP_UNLOCK_PATRON_CARD {0}",
				new object[] { PatronCode }).ToList();
			return list;
		}

		[HttpPost]
        public PartialViewResult FindByName(string strFullName)
        {
            if (String.IsNullOrEmpty(strFullName))
            {
                ViewBag.listpatron = new List<FPT_SP_ILL_SEARCH_PATRON_Result>();
            }
            else
            {
                fullname = strFullName;
                ViewBag.listpatron = searchPatronBusiness.FPT_SP_ILL_SEARCH_PATRONs(strFullName, "").Where(a => a.DOB != null).ToList();
            }

            return PartialView("_findByCardNumber");
        }
        [HttpGet]

        public PartialViewResult FindByCardNumber()
        {
            ViewBag.listpatron = new List<FPT_SP_ILL_SEARCH_PATRON_Result>();
            return PartialView("_findByCardNumber");
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
        public JsonResult GetLockPatronInfo()
        {
            CIR_PATRON patron = db.CIR_PATRON.Where(a => a.Code == sessionpcode).First();
            string LPatronName = patron.FirstName + " " + patron.MiddleName + " " + patron.LastName;
            string LPatronCode = sessionpcode;
            ViewBag.blackstartdate = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().StartedDate;
            string Lblackstartdate = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().StartedDate.ToString("dd/MM/yyyy");
            string Lblackenddate = ViewBag.blackstartdate.AddDays(db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().LockedDays).ToString("dd/MM/yyyy");
            string LlockedDay = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().LockedDays.ToString();
            string LblackNote = db.CIR_PATRON_LOCK.Where(a => a.PatronCode == LPatronCode).First().Note;
            string[] PatronLockInfo = { LPatronName, LPatronCode, Lblackstartdate, Lblackenddate, LlockedDay, LblackNote };
            return Json(PatronLockInfo, JsonRequestBehavior.AllowGet);
        }

        public void Getpatrondetail(string strPatronCode)
        {
            if (db.SP_GET_PATRON_INFOR("", strPatronCode, DateTime.Now.ToString("MM/dd/yyyy")).Count() == 0)
            {
                ViewBag.message = "Số thẻ không tồn tại";
                ViewBag.PatronDetail = null;
            }
            else
            {
                ViewBag.message = "";
                SP_GET_PATRON_INFOR_Result patroninfo =
              db.SP_GET_PATRON_INFOR("", strPatronCode, DateTime.Now.ToString("MM/dd/yyyy")).First();
                CIR_PATRON patron = db.CIR_PATRON.Where(a => a.Code == strPatronCode).First();
                ViewBag.loanquota = patron.CIR_PATRON_GROUP.LoanQuota;
                ViewBag.message = "";
                ViewBag.PatronDetail = new DetailPatron
                {
                    ID = patron.ID,
                    strCode = patron.Code,
                    Name = patron.FirstName + " " + patron.MiddleName + " " + patron.LastName,
                    strDOB = Convert.ToDateTime(patron.DOB).ToString("dd/MM/yyyy"),
                    strValidDate = Convert.ToDateTime(patroninfo.ValidDate).ToString("dd/MM/yyyy"),
                    strExpiredDate = Convert.ToDateTime(patron.ExpiredDate).ToString("dd/MM/yyyy"),
                    Sex = patron.Sex == "1" ? "Nam" : "Nữ",
                    intEthnicID = db.CIR_DIC_ETHNIC.Where(a => a.ID == patron.EthnicID).Count() == 0 ? "" : db.CIR_DIC_ETHNIC.Where(a => a.ID == patron.EthnicID).First().Ethnic,
                    intCollegeID = (patron.CIR_PATRON_UNIVERSITY == null || patron.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE == null) ? "" : patron.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE.College,
                    intFacultyID = (patron.CIR_PATRON_UNIVERSITY == null || patron.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY == null) ? "" : patron.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY.Faculty,
                    strEducationlevel = patron.CIR_DIC_EDUCATION == null ? null : patron.CIR_DIC_EDUCATION.EducationLevel,
                    strWorkPlace = patroninfo.WorkPlace,
                    strGrade = patron.CIR_PATRON_UNIVERSITY == null ? "" : patron.CIR_PATRON_UNIVERSITY.Grade,
                    strClass = patron.CIR_PATRON_UNIVERSITY == null ? "" : patron.CIR_PATRON_UNIVERSITY.Class,
                    strAddress = patron.CIR_PATRON_OTHER_ADDR.Count == 0 ? "" : patron.CIR_PATRON_OTHER_ADDR.First().Address,
                    strTelephone = patron.Telephone,
                    strMobile = patron.Mobile,
                    strEmail = patron.Email,
                    strNote = patron.Note,
                    intOccupationID = patron.CIR_DIC_OCCUPATION == null ? "" : patron.CIR_DIC_OCCUPATION.Occupation,
                    intPatronGroupID = patron.CIR_PATRON_GROUP == null ? "" : patron.CIR_PATRON_GROUP.Name,
                    strPortrait = patron.Portrait
                };
                int id2 = ViewBag.PatronDetail.ID;
                Getonloandetail(id2);
            }
        }

        public void Getonloandetail(int id)
        {
            List<SP_GET_PATRON_ONLOAN_COPIES_Result> patronloaninfo = db.SP_GET_PATRON_ONLOAN_COPIES(id).ToList<SP_GET_PATRON_ONLOAN_COPIES_Result>();
            List<OnLoan> onLoans = new List<OnLoan>();
            int owningcount = 0;
            int outofquota = 0;
            foreach (SP_GET_PATRON_ONLOAN_COPIES_Result a in patronloaninfo)
            {
                if ((DateTime.Now - a.DUEDATE.Value).Days > 0)
                {
                    owningcount = owningcount + 1;
                }
                if(a.LOANMODE == 3)
                {
                    outofquota = outofquota + 1;
                }
                onLoans.Add(new OnLoan
                {
                    Title = f.OnFormatHoldingTitle(a.TITLE),
                    Copynumber = a.COPYNUMBER,
                    CheckoutDate = a.CHECKOUTDATE.ToString("dd/MM/yyyy"),
                    DueDate = a.DUEDATE.Value.ToString("dd/MM/yyyy"),
                    OverDueDate = (DateTime.Now - a.DUEDATE.Value).Days > 0 ? (DateTime.Now - a.DUEDATE.Value).Days.ToString() : "",
                    Note = a.NOTE
                });
            }
            ViewBag.patronloaninfo = onLoans;
            // số ấn phẩm mượn ngoài hạn ngạch
            ViewBag.outofquota = outofquota;
            //số ấn phẩm đang quá hạn
            ViewBag.owningcount = owningcount;
        }

        public bool Checkonloanquota(int id, int loanMode)
        {
            bool check = true;
            List<SP_GET_PATRON_ONLOAN_COPIES_Result> patronloaninfo = db.SP_GET_PATRON_ONLOAN_COPIES(id).ToList<SP_GET_PATRON_ONLOAN_COPIES_Result>();
            int outofquota = 0;
            foreach (SP_GET_PATRON_ONLOAN_COPIES_Result a in patronloaninfo)
            {
                if (a.LOANMODE == 3)
                {
                    outofquota = outofquota + 1;
                }
            }
            int loanquota = db.CIR_PATRON.Where(a => a.ID == id).First().CIR_PATRON_GROUP.LoanQuota;
            if (patronloaninfo.Count()- outofquota >= loanquota && loanMode == 1)
            {
                check = false;
            }
            return check;
        }

        public void Getcurrentloandetail()
        {
            List<SP_GET_CURRENT_LOANINFOR_Result> currentloaninfo = checkOutBusiness.SP_GET_CURRENT_LOANINFORs(strTransactionIDs, "Loan").ToList();
            List<OnLoan> onLoans = new List<OnLoan>();

            foreach (SP_GET_CURRENT_LOANINFOR_Result a in currentloaninfo)
            {
                onLoans.Add(new OnLoan
                {
                    Title = f.OnFormatHoldingTitle(a.Title),
                    Copynumber = a.CopyNumber,
                    CheckoutDate = a.CheckOutDate.ToString("dd/MM/yyyy"),
                    DueDate = a.DueDate.ToString("dd/MM/yyyy"),
                    OverDueDate = "",
                    Note = a.Note
                });
            }
            ViewBag.currentloaninfo = onLoans;
        }

        public class CustomPatron
        {
            public int ID { get; set; }
            public string strCode { get; set; }
            public string Name { get; set; }
            public string strDOB { get; set; }
            public string strValidDate { get; set; }
            public string strExpiredDate { get; set; }
            public string Sex { get; set; }
            public string intEthnicID { get; set; }
            public string intCollegeID { get; set; }
            public string intFacultyID { get; set; }
            public string strEducationlevel { get; set; }
            public string strWorkPlace { get; set; }
            public string strGrade { get; set; }
            public string strClass { get; set; }
            public string strAddress { get; set; }
            public string strTelephone { get; set; }
            public string strMobile { get; set; }
            public string strEmail { get; set; }
            public string strNote { get; set; }
            public string intOccupationID { get; set; }
            public string intPatronGroupID { get; set; }
            public string strPortrait { get; set; }
        }

        public class OnLoan
        {
            public string Title { get; set; }
            public string Copynumber { get; set; }
            public string CheckoutDate { get; set; }
            public string DueDate { get; set; }
            public string OverDueDate { get; set; }
            public string Note { get; set; }
        }

    }
}