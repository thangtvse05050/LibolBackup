using Libol.EntityResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    public class AcquisitionBusiness
    {
        LibolEntities db = new LibolEntities();
        public List<FPT_GET_LIQUIDBOOKS_Result> FPT_GET_LIQUIDBOOKS_LIST(string LiquidCode, int LibID, string LocPrefix, string LocID, string DateFrom, string DateTo, int UserID)
        {
            List<FPT_GET_LIQUIDBOOKS_Result> list = db.Database.SqlQuery<FPT_GET_LIQUIDBOOKS_Result>("FPT_GET_LIQUIDBOOKS {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                new object[] { LiquidCode, LibID, LocPrefix, LocID, DateFrom, DateTo, UserID }).ToList();
            return list;
        }
        public List<FPT_ACQ_YEAR_STATISTIC_Result> FPT_ACQ_YEAR_STATISTIC_LIST(int LibID, int LocID, string FromYear, string ToYear, int UserID)
        {
            List<FPT_ACQ_YEAR_STATISTIC_Result> list = db.Database.SqlQuery<FPT_ACQ_YEAR_STATISTIC_Result>("FPT_ACQ_YEAR_STATISTIC {0}, {1}, {2}, {3}, {4}",
                new object[] { LibID, LocID, FromYear, ToYear, UserID }).ToList();
            return list;
        }
        public List<FPT_ACQ_MONTH_STATISTIC_Result> FPT_ACQ_MONTH_STATISTIC_LIST(int LibID, int LocID, string InYear, int UserID)
        {
            List<FPT_ACQ_MONTH_STATISTIC_Result> list = db.Database.SqlQuery<FPT_ACQ_MONTH_STATISTIC_Result>("FPT_ACQ_MONTH_STATISTIC {0}, {1}, {2}, {3}",
                new object[] { LibID, LocID, InYear, UserID }).ToList();
            return list;
        }

        // STATISTIC BOOKIN
        public List<FPT_SP_GET_ITEM_Result> FPT_SP_GET_ITEM_LIST(string DateFrom, string DateTo, int LocID, int LibID)
        {
            List<FPT_SP_GET_ITEM_Result> list = db.Database.SqlQuery<FPT_SP_GET_ITEM_Result>("FPT_SP_GET_ITEM {0}, {1}, {2}, {3}",
                new object[] { DateFrom, DateTo, LocID, LibID }).ToList();
            return list;
        }
        public List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(int ItemID, int LocID, int LibID)
        {
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> list = db.Database.SqlQuery<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result>("FPT_COUNT_COPYNUMBER_BY_ITEMID {0}, {1}, {2}",
                new object[] { ItemID, LocID, LibID }).ToList();
            return list;
        }

        public List<SP_GET_ITEM_INFOR_Result> SP_GET_ITEM_INFOR_LIST(int ItemID)
        {
            List<SP_GET_ITEM_INFOR_Result> list = db.Database.SqlQuery<SP_GET_ITEM_INFOR_Result>("SP_GET_ITEM_INFOR {0}",
                new object[] { ItemID }).ToList();
            return list;
        }

        public List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> FPT_COUNT_COPYNUMBER_ONLOAN_LIST(int ItemID, int LocID, int LibID)
        {
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> list = db.Database.SqlQuery<FPT_COUNT_COPYNUMBER_ONLOAN_Result>("FPT_COUNT_COPYNUMBER_ONLOAN {0}, {1}, {2}",
                new object[] { ItemID, LocID, LibID }).ToList();
            return list;
        }

        //list liquid copynumber
        public List<FPT_SP_GET_ITEM_INFOR_Result> FPT_SP_GET_ITEM_INFOR_LIST(int ItemID, int LocID, int LibID)
        {
            List<FPT_SP_GET_ITEM_INFOR_Result> list = db.Database.SqlQuery<FPT_SP_GET_ITEM_INFOR_Result>("FPT_SP_GET_ITEM_INFOR {0}, {1}, {2}",
                new object[] { ItemID, LocID, LibID }).ToList();
            return list;
        }

        // Inventory
        public List<FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_Result> FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_LIST(int LibID, int LocID, string strShelf, int intMode)
        {
            List<FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_Result> list = db.Database.SqlQuery<FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV_Result>("FPT_SP_GET_GENERAL_LOC_INFOR_DUCNV {0}, {1}, {2}, {3}",
                new object[] { LibID, LocID, strShelf, intMode }).ToList();
            return list;
        }
        //Open Location
        public List<SP_HOLDING_LIBRARY_SELECT_Result> SP_HOLDING_LIBRARY_SELECT_LIST(int LibID, int LocalLib, int status, int userID, int type)
        {
            List<SP_HOLDING_LIBRARY_SELECT_Result> list = db.Database.SqlQuery<SP_HOLDING_LIBRARY_SELECT_Result>("SP_HOLDING_LIBRARY_SELECT {0}, {1}, {2}, {3}, {4}",
                new object[] { LibID, LocalLib, status, userID, type }).ToList();
            return list;
        }

        //get Location inventory
        public List<SP_HOLDING_LOCATION_GET_INFO_Result> SP_HOLDING_LOCATION_GET_INFO_LIST(int LibID, int userID, int LocID, int status)
        {
            List<SP_HOLDING_LOCATION_GET_INFO_Result> list = db.Database.SqlQuery<SP_HOLDING_LOCATION_GET_INFO_Result>("SP_HOLDING_LOCATION_GET_INFO {0}, {1}, {2}, {3}",
                new object[] { LibID, userID, LocID, status }).ToList();
            return list;
        }
        //close Location
        public List<SP_HOLDING_LOCATION_UPD_STATUS_Result> SP_HOLDING_LOCATION_UPD_STATUS(string strLocID, string strShelf, int intStatus)
        {
            List<SP_HOLDING_LOCATION_UPD_STATUS_Result> list = db.Database.SqlQuery<SP_HOLDING_LOCATION_UPD_STATUS_Result>("SP_HOLDING_LOCATION_UPD_STATUS {0}, {1}, {2}",
                new object[] { strLocID, strShelf, intStatus }).ToList();
            return list;
        }
        //recomend report
        public List<FPT_SP_GET_HOLDING_BY_RECOMMENDID_Newest_Result> FPT_SP_GET_HOLDING_BY_RECOMMENDID_Newest(int LibraryID, int LocationID, string ReCode, string StartDate, string EndDate)
        {
            List<FPT_SP_GET_HOLDING_BY_RECOMMENDID_Newest_Result> list = db.Database.SqlQuery<FPT_SP_GET_HOLDING_BY_RECOMMENDID_Newest_Result>("FPT_SP_GET_HOLDING_BY_RECOMMENDID_Newest {0}, {1}, {2}, {3}, {4}",
                new object[] { LibraryID, LocationID, ReCode, StartDate, EndDate }).ToList();
            return list;
        }
        
        public List<FPT_SP_INVENTORY_Result> FPT_SP_INVENTORY(int LibraryID, string LocationPrefix, string LocationID,int InUsed)
        {
            List<FPT_SP_INVENTORY_Result> list = db.Database.SqlQuery<FPT_SP_INVENTORY_Result>("FPT_SP_INVENTORY {0}, {1}, {2}, {3}", new object[] { LibraryID, LocationPrefix, LocationID, InUsed }).ToList();
            return list;

        }

		
    }
}