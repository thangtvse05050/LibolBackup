using OPAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Text;
using System.Text.RegularExpressions;

namespace OPAC.Dao
{
    public class SearchDao
    {
        /// <summary>
        /// Search book with key word
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <param name="option"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_Result> GetSearchingBook(string searchKeyword, string option, int page, int pageSize)
        {
            using (var dbContext = new OpacEntities())
            {
                searchKeyword = Regex.Replace(searchKeyword, @"\s+", " ").Trim();
                var list = dbContext.Database.SqlQuery<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_Result>("FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK {0}, {1}",
                     new object[] { searchKeyword, option }).ToList();

                foreach (var item in list)
                {
                    if (item.Version != null)
                    {
                        item.Version = item.Version.Replace("$a", "");
                    }
                }
                return list.ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// Search book by key word
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchBy"></param>
        /// <returns></returns>
        public IEnumerable<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD_Result> GetSearchingBookByKeyWord(string searchKeyword, int page, int pageSize, int searchBy)
        {
            /*
             * Parameter searchBy definition:
             * 1: Search by keyword
             * 2: Search by DDC
             */
            using (var dbContext = new OpacEntities())
            {
                var listResult = new List<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD_Result>();
                if (searchBy == 1)
                {
                    var getItemID = dbContext.FPT_SP_GET_ITEMID_BY_KEYWORD(searchKeyword).ToList();

                    foreach (var item in getItemID)
                    {
                        var list = dbContext.Database.SqlQuery<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD_Result>("FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD {0}",
                            new object[] { item }).ToList();

                        foreach (var result in list)
                        {
                            if (result.Version != null)
                            {
                                result.Version = result.Version.Replace("$a", "");
                            }
                        }

                        listResult.AddRange(list);
                    }
                }
                else if (searchBy == 2)
                {
                    var getItemID = dbContext.FPT_SP_OPAC_GET_ITEMID_BY_DDC(searchKeyword).ToList();

                    foreach (var item in getItemID)
                    {
                        var list = dbContext.Database.SqlQuery<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD_Result>("FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD {0}",
                            new object[] { item }).ToList();

                        foreach (var result in list)
                        {
                            if (result.Version != null)
                            {
                                result.Version = result.Version.Replace("$a", "");
                            }
                        }

                        listResult.AddRange(list);
                    }
                }

                return listResult.ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// Get all copy number of book by itemID
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static List<string> GetListCopyNumber(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var copyNum = (from g in dbContext.HOLDINGs
                               where g.ItemID == itemID
                               select g.CopyNumber).Take(18).ToList();

                return copyNum;
            }
        }

        /// <summary>
        /// Count number of book following by symbol
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static int CountTotalCopyNumberBySymbol(int itemID, string symbol)
        {
            using (var dbContext = new OpacEntities())
            {
                var counting = (from g in dbContext.HOLDINGs
                               join d in dbContext.HOLDING_LOCATION on g.LocationID equals d.ID
                               where g.ItemID == itemID && d.Symbol.Equals(symbol)
                               select g.CopyNumber).Count();

                return counting;
            }
        }

        /// <summary>
        /// Get title of item by itemID
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public string GetItemTitle(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var itemTitle = (from r in dbContext.ITEM_TITLE
                                  where r.ItemID == itemID
                                  select r.Title).FirstOrDefault();

                return itemTitle;
            }
        }

        /// <summary>
        /// Count number of book which is free to borrow, following by symbol
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static int CountTotalCopyNumberFreeBySymbol(int itemID, string symbol)
        {
            using (var dbContext = new OpacEntities())
            {
                var counting = (from g in dbContext.HOLDINGs
                               join d in dbContext.HOLDING_LOCATION on g.LocationID equals d.ID
                               where g.ItemID == itemID && d.Symbol.Equals(symbol) && g.InUsed == false
                               select g.CopyNumber).Count();

                return counting;
            }
        }

        /// <summary>
        /// Count number of result book after searching
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public int GetNumberResult(string searchKeyword, string option)
        {
            using (var dbContext = new OpacEntities())
            {
                searchKeyword = Regex.Replace(searchKeyword, @"\s+", " ").Trim();
                var numberResult = dbContext.Database.SqlQuery<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_Result>("FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK {0}, {1}",
                    new object[] { searchKeyword, option }).ToList().Count();

                return numberResult;
            }
        }

        /// <summary>
        /// Count number of result book after searching by key word
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchBy"></param>
        /// <returns></returns>
        public int GetNumberResultByKeyWord(string searchKeyword, int page, int pageSize, int searchBy)
        {
            var numberResult = new List<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD_Result>();
            using (var dbContext = new OpacEntities())
            {
                if (searchBy == 1)
                {
                    var getItemID = dbContext.FPT_SP_GET_ITEMID_BY_KEYWORD(searchKeyword).ToList();
                    foreach (var item in getItemID)
                    {
                        var list = dbContext.Database.SqlQuery<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD_Result>("FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD {0}",
                            new object[] { item }).ToList();

                        numberResult.AddRange(list);
                    }
                }
                else if (searchBy == 2)
                {
                    var getItemID = dbContext.FPT_SP_OPAC_GET_ITEMID_BY_DDC(searchKeyword).ToList();
                    foreach (var item in getItemID)
                    {
                        var list = dbContext.Database.SqlQuery<FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD_Result>("FPT_SP_OPAC_GET_SEARCHED_INFO_BOOK_BY_KEYWORD {0}",
                            new object[] { item }).ToList();

                        numberResult.AddRange(list);
                    }
                }

                return numberResult.ToPagedList(page, pageSize).TotalItemCount;
            }
        }

        /// <summary>
        /// Get detail information of book and display by MARC
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="isAuthority"></param>
        /// <returns></returns>
        public List<SP_CATA_GET_CONTENTS_OF_ITEMS_Result> SP_CATA_GET_CONTENTS_OF_ITEMS_LIST(int itemID, int isAuthority)
        {
            using (var dbContext = new OpacEntities())
            {
                var list = dbContext.Database.SqlQuery<SP_CATA_GET_CONTENTS_OF_ITEMS_Result>("SP_CATA_GET_CONTENTS_OF_ITEMS {0}, {1}",
                    new object[] { itemID, isAuthority }).ToList();

                return list;
            }
        }

        /// <summary>
        /// Get related term of a book
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public List<FPT_SP_OPAC_GET_RELATED_TERMS_Result> FPT_SP_OPAC_GET_RELATED_TERMS_LIST(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var list = dbContext.Database.SqlQuery<FPT_SP_OPAC_GET_RELATED_TERMS_Result>("FPT_SP_OPAC_GET_RELATED_TERMS {0}",
                    new object[] { itemID }).ToList();

                return list;
            }
        }

        /// <summary>
        /// Get all copy number of book
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public List<FPT_SP_GET_CODE_AND_SYMBOL_BY_ITEMID_Result> GetInforCopyNumberList(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var list = dbContext.Database.SqlQuery<FPT_SP_GET_CODE_AND_SYMBOL_BY_ITEMID_Result>("FPT_SP_GET_CODE_AND_SYMBOL_BY_ITEMID {0}",
                    new object[] { itemID }).ToList();

                return list;
            }
        }

        /// <summary>
        /// Get detal book with status
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<FPT_SP_GET_DETAIL_BOOK_WITH_STATUS_Result> GetDetailBookWithStatus(int itemID, string code)
        {
            using (var dbContext = new OpacEntities())
            {
                var list = dbContext.Database.SqlQuery<FPT_SP_GET_DETAIL_BOOK_WITH_STATUS_Result>("FPT_SP_GET_DETAIL_BOOK_WITH_STATUS {0}, {1}",
                    new object[] { itemID, code }).ToList();

                return list;
            }
        }

        /// <summary>
        /// Get full information of book after searching (Lấy thông tin sách hiển thị đầy đủ)
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public FullInforBook GetFullInforBook(int itemID)
        {
            StringBuilder getDocumentType = new StringBuilder("");
            StringBuilder getISBN = new StringBuilder("");
            StringBuilder getLanguageCode = new StringBuilder("");
            StringBuilder getPublishing = new StringBuilder("");
            StringBuilder getPublishingYear = new StringBuilder("");
            StringBuilder getPhysicDescription = new StringBuilder("");
            StringBuilder getBrief = new StringBuilder("");
            string[] specialCharacterList = { "$a", "$b", "$c", "$p", "$e", "$n" };

            using (var dbContext = new OpacEntities())
            {
                var getISBNTemp = dbContext.FPT_SP_GET_ISBN_ITEM(itemID).ToList();
                foreach (var item in getISBNTemp)
                {
                    getISBN.Append(item + " ");
                }
                
                var getLanguageCodeTemp = dbContext.FPT_SP_GET_LANGUAGE_CODE_ITEM(itemID).ToList();
                foreach (var item in getLanguageCodeTemp)
                {
                    getLanguageCode.Append(item);
                }
                
                var getPublishingTemp = dbContext.FPT_SP_GET_PUBLISH_INFO_ITEM(itemID).ToList();
                foreach (var item in getPublishingTemp)
                {
                    getPublishing.Append(item + " ");
                }

                string[] temp = getPublishing.ToString().Split(new[] {"$c"}, StringSplitOptions.None);
                if (temp.Length == 1)
                {
                    getPublishing = new StringBuilder(temp[0]);
                }
                else
                {
                    getPublishing = new StringBuilder(temp[0]);
                    getPublishingYear.Append(temp[1]);
                }

                var getPhysicDescriptionTemp = dbContext.FPT_SP_GET_PHYSICAL_INFO_ITEM(itemID).ToList();
                foreach (var item in getPhysicDescriptionTemp)
                {
                    getPhysicDescription.Append(item + " ");
                }

                foreach (var item in specialCharacterList)
                {
                    getISBN.Replace(item, "");
                    getLanguageCode.Replace(item, "");
                    getPublishing.Replace(item, "");
                    getPhysicDescription.Replace(item, "");
                }
            }

            FullInforBook fullBookInformation = new FullInforBook
            {
                ISBN = getISBN.ToString().Trim(),
                LanguageCode = getLanguageCode.ToString().Trim(),
                Publishing = getPublishing.ToString().Trim(),
                PublishingYear = getPublishingYear.ToString().Trim(),
                PhysicDescription = getPhysicDescription.ToString().Trim()
            };

            return fullBookInformation;
        }

        /// <summary>
        /// Get total of book
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int GetTotalBook(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var totalBook = (from total in dbContext.HOLDINGs
                                 where total.ItemID == itemID
                                 select total).Count();

                return totalBook;
            }
        }

        /// <summary>
        /// Get total of book which is not using yet
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int GetFreeBook(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var freeBook = (from f in dbContext.HOLDINGs
                                 where f.ItemID == itemID && f.InUsed == false
                                 select f).Count();

                return freeBook;
            }
        }

        /// <summary>
        /// Get number of book preparing to borrow by patron
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int GetOnHoldingBook(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var holdingBook = (from h in dbContext.HOLDINGs
                                where h.ItemID == itemID && h.OnHold == true
                                select h).Count();

                return holdingBook;
            }
        }

        /// <summary>
        /// Get list of document
        /// </summary>
        /// <returns></returns>
        public List<SP_OPAC_GET_DIC_ITEM_TYPE_Result> GetDocumentType()
        {
            using (var dbContext = new OpacEntities())
            {
                return dbContext.SP_OPAC_GET_DIC_ITEM_TYPE().ToList();
            }
        }

        /// <summary>
        /// Get list of library
        /// </summary>
        /// <returns></returns>
        public List<SP_GET_LIBRARY_Result> GetLibrary()
        {
            using (var dbContext = new OpacEntities())
            {
                return dbContext.SP_GET_LIBRARY().ToList();
            }
        }

        /// <summary>
        /// Get location
        /// </summary>
        /// <param name="libId"></param>
        /// <returns></returns>
        public List<Location> GetLocation(int libId)
        {
            using (var dbContext = new OpacEntities())
            {
                var list = new List<Location>();
                var locationList = dbContext.FPT_SP_OPAC_GET_LOCATION().Where(t => t.LibID == libId).ToList();
                foreach (var item in locationList)
                {
                    Location location = new Location()
                    {
                        ID = item.ID,
                        LibID = item.LibID,
                        SymbolAndCodeLoc = item.Symbol + " (" + item.CodeLoc + ")",
                        MaxNumber = item.MaxNumber,
                        Status = item.Status
                    };
                    list.Add(location);
                }

                return list;
            }
        }

        /// <summary>
        /// Get summary field
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public string GetSummary(int itemID)
        {
            using (var dbContext = new OpacEntities())
            {
                var summary = "";
                try
                {
                    summary = (from s in dbContext.FIELD500S
                        where s.FieldCode.Equals("520") && s.ItemID == itemID
                        select s.Content).FirstOrDefault();

                    summary = summary.Replace("$a", "");
                }
                catch (NullReferenceException)
                {
                    summary = "";
                }
                
                return summary;
            }
        }
    }
}