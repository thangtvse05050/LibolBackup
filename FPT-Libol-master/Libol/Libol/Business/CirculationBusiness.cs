using Libol.EntityResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    public class CirculationBusiness
    {
        LibolEntities db = new LibolEntities();
        public List<GET_PATRON_LOANINFOR_Result> GET_PATRON_LOAN_INFOR_LIST(string PatronCode, string ItemCode, string CopyNumber, int LibraryID, string LocationPrefix, string LocationID, string CheckOutDateFrom, string CheckOutDateTo, string CheckInDateFrom, string CheckInDateTo, string Serial, int UserID)
        {
            List<GET_PATRON_LOANINFOR_Result> list = db.Database.SqlQuery<GET_PATRON_LOANINFOR_Result>("FPT_GET_PATRON_LOANINFOR {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                new object[] { PatronCode, ItemCode, CopyNumber, LibraryID, LocationPrefix, LocationID, CheckOutDateFrom, CheckOutDateTo, CheckInDateFrom, CheckInDateTo, Serial, UserID }).ToList();
            return list;
        }
        public List<GET_PATRON_RENEW_LOAN_INFOR_Result> GET_PATRON_RENEW_LOAN_INFOR_LIST(string PatronCode, string ItemCode, string CopyNumber, int LibraryID, string LocationPrefix, string LocationID, string CheckOutDateFrom, string CheckOutDateTo, string CheckInDateFrom, string CheckInDateTo, int UserID)
        {
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> list = db.Database.SqlQuery<GET_PATRON_RENEW_LOAN_INFOR_Result>("FPT_GET_PATRON_RENEW_LOAN_INFOR {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
                new object[] { PatronCode, ItemCode, CopyNumber, LibraryID, LocationPrefix, LocationID, CheckOutDateFrom, CheckOutDateTo, CheckInDateFrom, CheckInDateTo, UserID }).ToList();
            return list;
        }
        public List<GET_PATRON_ONLOANINFOR_Result> GET_PATRON_ONLOAN_INFOR_LIST(string PatronCode, string ItemCode, string CopyNumber, int LibraryID, string LocationPrefix, string LocationID, string CheckOutDateFrom, string CheckOutDateTo, string DueDateFrom, string DueDateTo, string Serial, int UserID)
        {
            List<GET_PATRON_ONLOANINFOR_Result> list = db.Database.SqlQuery<GET_PATRON_ONLOANINFOR_Result>("FPT_GET_PATRON_ONLOANINFOR {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                new object[] { PatronCode, ItemCode, CopyNumber, LibraryID, LocationPrefix, LocationID, CheckOutDateFrom, CheckOutDateTo, DueDateFrom, DueDateTo, Serial, UserID }).ToList();
            return list;
        }
        public List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> GET_PATRON_RENEW_ONLOAN_INFOR_LIST(string PatronCode, string ItemCode, string CopyNumber, int LibraryID, string LocationPrefix, string LocationID, string CheckOutDateFrom, string CheckOutDateTo, string CheckInDateFrom, string CheckInDateTo, int UserID)
        {
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> list = db.Database.SqlQuery<GET_PATRON_RENEW_ONLOAN_INFOR_Result>("FPT_GET_PATRON_RENEW_ONLOAN_INFOR {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
                new object[] { PatronCode, ItemCode, CopyNumber, LibraryID, LocationPrefix, LocationID, CheckOutDateFrom, CheckOutDateTo, CheckInDateFrom, CheckInDateTo, UserID }).ToList();
            return list;
        }
        public List<FPT_CIR_YEAR_STATISTIC_Result> GET_FPT_CIR_YEAR_STATISTIC_LIST(int LibraryID, string strLocPrefix, string LocationID, int Type, int Status, string FromYear, string ToYear, int UserID)
        {
            List<FPT_CIR_YEAR_STATISTIC_Result> list = db.Database.SqlQuery<FPT_CIR_YEAR_STATISTIC_Result>("FPT_CIR_YEAR_STATISTIC {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                new object[] { LibraryID, strLocPrefix, LocationID, Type, Status, FromYear, ToYear, UserID }).ToList();
            return list;
        }
        public List<FPT_CIR_MONTH_STATISTIC_Result> GET_FPT_CIR_MONTH_STATISTIC_LIST(int LibraryID, string LocPrefix, string LocationID, int Type, int Status, string fromDate, string toDate, int UserID)
        {
            List<FPT_CIR_MONTH_STATISTIC_Result> list = db.Database.SqlQuery<FPT_CIR_MONTH_STATISTIC_Result>("FPT_CIR_MONTH_STATISTIC {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                new object[] { LibraryID, LocPrefix, LocationID, Type, Status, fromDate, toDate, UserID }).ToList();
            return list;
        }
        public List<SP_GET_LOCKEDPATRONS_Result> GET_SP_GET_LOCKEDPATRONS_LIST(string PatronCode, string Note, string LockDateFrom, string LockDateTo, int CollegeID)
        {
            List<SP_GET_LOCKEDPATRONS_Result> list = db.Database.SqlQuery<SP_GET_LOCKEDPATRONS_Result>("FPT_GET_PATRON_LOCK_STATISTIC {0}, {1}, {2}, {3}, {4}",
                new object[] { PatronCode, Note, LockDateFrom, LockDateTo, CollegeID }).ToList();
            return list;
        }
        public List<SP_LOCK_PATRON_CARD_Result> GET_SP_LOCK_PATRON_CARD_LIST(string strPatronCode, Nullable<int> intLockedDays, string strStartedDate, string strNote)
        {
            List<SP_LOCK_PATRON_CARD_Result> list = db.Database.SqlQuery<SP_LOCK_PATRON_CARD_Result>("SP_LOCK_PATRON_CARD {0}, {1}, {2}, {3}",
                new object[] { strPatronCode, intLockedDays, strStartedDate, strNote }).ToList();
            return list;
        }
        public List<SP_UNLOCK_PATRON_CARD_Result> FPT_SP_UNLOCK_PATRON_CARD_LIST(string PatronCode)
        {
            List<SP_UNLOCK_PATRON_CARD_Result> list = db.Database.SqlQuery<SP_UNLOCK_PATRON_CARD_Result>("SP_UNLOCK_PATRON_CARD {0}",
                new object[] { PatronCode }).ToList();
            return list;
        }
        public List<SP_GET_LOCKEDPATRONS_Result2> GET_SP_GET_LOCKEDPATRONS_LIST2(string PatronCode, string LockDateFrom, string LockDateTo, int CollegeID)
        {
            List<SP_GET_LOCKEDPATRONS_Result2> list = db.Database.SqlQuery<SP_GET_LOCKEDPATRONS_Result2>("FPT_GET_PATRON_LOCK_STATISTIC {0}, {1}, {2}, {3}",
                new object[] { PatronCode, LockDateFrom, LockDateTo, CollegeID }).ToList();
            return list;
        }
        public List<FPT_SP_UPDATE_UNLOCK_PATRON_CARD_Result> FPT_SP_UPDATE_UNLOCK_PATRON_CARD(string PatronCode, Nullable<int> intLockedDays, string strNote)
        {
            List<FPT_SP_UPDATE_UNLOCK_PATRON_CARD_Result> list = db.Database.SqlQuery<FPT_SP_UPDATE_UNLOCK_PATRON_CARD_Result>("FPT_SP_UPDATE_UNLOCK_PATRON_CARD {0}, {1}, {2}",
                new object[] { PatronCode, intLockedDays, strNote}).ToList();
            return list;
        }

        //list liquid copynumber
        public List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST(string dkcb)
        {
            List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> list = db.Database.SqlQuery<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result>("FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER {0}",
                new object[] { dkcb }).ToList();
            return list;
        }
        //statistic top20
        public List<FPT_CIR_SP_STAT_TOP20_Result> FPT_CIR_SP_STAT_TOP20_LIST(int history, int catID, int userID, int libID)
        {
            List<FPT_CIR_SP_STAT_TOP20_Result> list = db.Database.SqlQuery<FPT_CIR_SP_STAT_TOP20_Result>("FPT_CIR_SP_STAT_TOP20 {0}, {1}, {2}, {3}",
                new object[] { history, catID, userID, libID }).ToList();
            return list;
        }
    }
}