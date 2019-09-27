using Libol.EntityResult;
using Libol.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libol.SupportClass;

namespace Libol.Controllers
{
    public class CheckInController : Controller
    {
        private LibolEntities db = new LibolEntities();
        SearchPatronBusiness searchPatronBusiness = new SearchPatronBusiness();
        FormatHoldingTitle f = new FormatHoldingTitle();
        CirculationBusiness circulationBusiness = new CirculationBusiness();
        private static string fullname = "";
        private static string sessionpcode = "";

        [AuthAttribute(ModuleID = 3, RightID = "17")]
        public ActionResult Index(string PatronCode)
        {
            sessionpcode = "";
            ViewBag.HiddenPatronCode = PatronCode;
            return View();
        }

        [HttpPost]
        public PartialViewResult SearchPatronDetail(string strPatronCode)
        {
            string pcode = strPatronCode.Trim();
            if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == pcode).Count() == 0)
            {
                ViewBag.active = 1;
            }
            else
            {
                ViewBag.active = 0;
            }

            Getpatrondetail(pcode);
            sessionpcode = pcode;
            return PartialView("_checkinByCardNumber");
        }

        [HttpPost]
        public PartialViewResult CheckInByDKCB(
            int intType,
            int intAutoPaid,
            string strCopyNumbers,
            string strCheckInDate,
			string strPatronCode
		)
        {
            string CopyNumber = strCopyNumbers.Trim();
			int success = -1;
            if (!sessionpcode.Equals(""))
            {
                Getpatrondetail(sessionpcode);
            }
            else
            {
                ViewBag.PatronDetail = null;
            }
            if (db.HOLDINGs.Where(a => a.CopyNumber == CopyNumber).Count() == 0)
            {
                ViewBag.message = "ĐKCB không đúng";
            }
            else if (db.CIR_LOAN.Where(a => a.CopyNumber == CopyNumber).Count() == 0)
            {
                ViewBag.message = "ĐKCB chưa được ghi mượn";
            }
            else
            {
                string patroncode = db.CIR_LOAN.Where(a => a.CopyNumber == CopyNumber).First().CIR_PATRON.Code;
                success = db.SP_CHECKIN((int)Session["UserID"], intType, intAutoPaid, CopyNumber, strCheckInDate,
                    new ObjectParameter("strTransIDs", typeof(string)),
                    new ObjectParameter("strPatronCode", typeof(string)),
                    new ObjectParameter("intError", typeof(int)));
                if (success == -1)
                {
                    ViewBag.CurrentCheckin = null;
                    ViewBag.message = "Ghi trả thất bại";
                }
                else
                {
                    int lastid = db.CIR_LOAN_HISTORY.Max(a => a.ID);
                    int id = db.CIR_LOAN_HISTORY.Where(b => b.ID == lastid).First().ItemID;
                    String fieldcode = "245";
                    ViewBag.message = "";
                    ViewBag.CurrentCheckin = new CurrentCheckIn
                    {
                        Title = f.OnFormatHoldingTitle(db.FIELD200S.Where(a => a.ItemID == id).Where(a => a.FieldCode == fieldcode).First().Content),
                        Copynumber = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().CopyNumber,
                        CheckOutDate = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().CheckOutdate.ToString("dd/MM/yyyy"),
                        CheckInDate = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().CheckInDate.ToString("dd/MM/yyyy"),
                        OverdueFine = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().OverdueFine.ToString()
                    };
                    sessionpcode = patroncode;
                }
                Getpatrondetail(patroncode);
				if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == patroncode).Count() == 0)
				{
					ViewBag.active = 1;
				}
				else
				{
					ViewBag.active = 0;
				}
			}
			if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == strPatronCode).Count() == 0)
			{
				ViewBag.active = 1;
			}
			else
			{
				ViewBag.active = 0;
			}
			
			return PartialView("_checkinByDKCB");
        }

        [HttpPost]
        public PartialViewResult CheckInByDKCBs(
           string strPatronCode,
           int intType,
           int intAutoPaid,
           string[] strCopyNumbers,
           string strCheckInDate
       )
        {
            string pcode = strPatronCode.Trim();
            int success = -1;
			if (strCopyNumbers != null)
			{
				foreach (string CopyNumber in strCopyNumbers)
				{
					success = db.SP_CHECKIN((int)Session["UserID"], intType, intAutoPaid, CopyNumber, strCheckInDate,
					new ObjectParameter("strTransIDs", typeof(string)),
					new ObjectParameter("strPatronCode", typeof(string)),
					new ObjectParameter("intError", typeof(int)));
				}
			}
            Getpatrondetail(sessionpcode);
			//FPT_SP_UNLOCK_PATRON_CARD_LIST("'" + strPatronCode + "'");
			if (db.CIR_PATRON_LOCK.Where(a => a.PatronCode == strPatronCode).Count() == 0)
			{
				ViewBag.active = 1;
			}
			else
			{
				ViewBag.active = 0;
			}
			if (success == -1)
            {
                ViewBag.message = "Ghi trả thất bại";
                ViewBag.CurrentCheckin = null;
            }
            else
            {
               
				int lastid = db.CIR_LOAN_HISTORY.Max(a => a.ID);
				int id = db.CIR_LOAN_HISTORY.Where(b => b.ID == lastid).First().ItemID;
				String fieldcode = "245";
				ViewBag.message = "";
				ViewBag.CurrentCheckin = new CurrentCheckIn
				{
					Title = f.OnFormatHoldingTitle(db.FIELD200S.Where(a => a.ItemID == id).Where(a => a.FieldCode == fieldcode).First().Content),
					Copynumber = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().CopyNumber,
					CheckOutDate = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().CheckOutdate.ToString("dd/MM/yyyy"),
					CheckInDate = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().CheckInDate.ToString("dd/MM/yyyy"),
					OverdueFine = db.CIR_LOAN_HISTORY.Where(a => a.ID == lastid).First().OverdueFine.ToString()
				};
			}
            return PartialView("_checkinByDKCB");
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
                ViewBag.listpatron = searchPatronBusiness.FPT_SP_ILL_SEARCH_PATRONs(fullname, "").Where(a => a.DOB != null).ToList();
            }

            return PartialView("_findByCardNumber");
        }

        [HttpGet]
        public PartialViewResult FindByCardNumber()
        {
            ViewBag.listpatron = new List<FPT_SP_ILL_SEARCH_PATRON_Result>();
            ViewBag.PatronDetail = null;
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
            string[] PatronLockInfo = { LPatronName , LPatronCode , Lblackstartdate, Lblackenddate, LlockedDay , LblackNote };
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
                SP_GET_PATRON_INFOR_Result patroninfo =
              db.SP_GET_PATRON_INFOR("", strPatronCode, DateTime.Now.ToString("MM/dd/yyyy")).First();
                CIR_PATRON patron = db.CIR_PATRON.Where(a => a.Code == strPatronCode).First();
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

            foreach (SP_GET_PATRON_ONLOAN_COPIES_Result a in patronloaninfo)
            {
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
        }
		// open card
		
		public PartialViewResult OpenPatronCodeCheckIn(string patroncode)
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
			Getpatrondetail(patroncode);
			return PartialView("_checkinByCardNumber");
		}

		public List<SP_UNLOCK_PATRON_CARD_Result> FPT_SP_UNLOCK_PATRON_CARD_LIST(string PatronCode)
		{
			List<SP_UNLOCK_PATRON_CARD_Result> list = db.Database.SqlQuery<SP_UNLOCK_PATRON_CARD_Result>("SP_UNLOCK_PATRON_CARD {0}",
				new object[] { PatronCode }).ToList();
			return list;
		}
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

    public class CurrentCheckIn
    {
        public string Title { get; set; }
        public string Copynumber { get; set; }
        public string CheckOutDate { get; set; }
        public string CheckInDate { get; set; }
        public string OverdueFine { get; set; }
    }

    public class DetailPatron
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
}