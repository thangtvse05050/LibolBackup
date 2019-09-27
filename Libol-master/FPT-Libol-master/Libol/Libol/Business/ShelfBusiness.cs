using Libol.Controllers;
using Libol.EntityResult;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Libol.Models
{
    public class ShelfBusiness
    {
        LibolEntities db = new LibolEntities();
        public bool IsExistHolding(string strCN, int intLocationID, int intCopyID = -1)
        {
            List<string> list = db.SP_CHECK_COPYNUMBER(strCN, intCopyID , intLocationID).ToList();
            return list.Count > 0 ? true : false;

        }
        public string GenCopyNumber(int locId)
        {
            string symbol = "";
            int maxNumber = 0;
            List<SP_HOLDING_LOCATION_GET_INFO_Result> list = FPT_SP_HOLDING_LOCATION_GET_INFO(0, 0, locId, -1);
            if (list != null && list.Count() > 0)
            {
                symbol = list[0].Symbol;
                maxNumber = Convert.ToInt32(list[0].MaxNumber) + 1;
            }
            else
            {
                return "Kho không tồn tại";
            }

            int length = 6 - maxNumber.ToString().Length;
            string stringZero = "";
            for (int i = 0; i < length; i++)
            {
                stringZero = stringZero + "0";
            }
            string copyNumber = symbol + stringZero + maxNumber;
            return copyNumber;
        }

        public string InsertHolding(HOLDING holding, int numberOfCN,string recommendID, ref string composite)
        {

            if (String.IsNullOrEmpty(holding.CopyNumber))
            {
                return "Hãy tạo đăng ký cá biệt";
            }
            ITEM item = db.ITEMs.FirstOrDefault(i => i.ID == holding.ItemID);
            if (item == null)
            {
                return "Không tồn tại bản ghi";
            }

            List<HOLDING> holdings = new List<HOLDING>();
            //   ITEM item = db.ITEMs.Where(i => i.ID == holding.ItemID).FirstOrDefault();
            holding.Volume = "";
            holding.UseCount = 0;
            holding.InUsed = false;
            holding.InCirculation = false;
            holding.ILLID = 0;
            holding.DateLastUsed = DateTime.Now;
            holding.CallNumber = db.ITEMs.Where(i => i.ID == holding.ItemID).FirstOrDefault().CallNumber;
            holding.Acquired = false;
            holding.Note = "";
            holding.POID = 0;


            // check start holding tồn tại chưa

            if (!IsExistHolding(holding.CopyNumber, holding.LocationID, -1))
            {

                string symbol = holding.CopyNumber.Substring(0, holding.CopyNumber.Length - 6);
                string strNumber = holding.CopyNumber.Substring(symbol.Length, 6);
                int number = Convert.ToInt32(strNumber);

                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    for (int i = 0; i < numberOfCN; i++)
                    {
                        // tạo list ĐKCB
                        int length = 6 - number.ToString().Length;
                        string stringZero = "";
                        for (int j = 0; j < length; j++)
                        {
                            stringZero = stringZero + "0";
                        }
                        string copyNumber = symbol + stringZero + number;

                        if (IsExistHolding(copyNumber, holding.LocationID, -1))
                        {
                            transaction.Rollback();
                            return "Hãy sinh lại giá trị";
                        };

                        number++;
                        // procedure đã + 1 giá trị MaxNumber trong HOLDING_LOCATION
                        db.SP_HOLDING_INS(
                            holding.ItemID,
                            holding.LocationID,
                            holding.LibID,
                            holding.UseCount,
                            holding.Volume,
                            // ngày bổ sung
                            holding.AcquiredDate.ToString(),
                            copyNumber,
                            holding.InUsed == true ? 1 : 0,
                            holding.InCirculation == true ? 1 : 0,
                            holding.ILLID,
                            holding.Price,
                            // giá sách
                            holding.Shelf,
                            holding.POID,
                            //ngày sử dụng cuối
                            holding.DateLastUsed.ToString(),
                            holding.CallNumber,
                            holding.Acquired == true ? 1 : 0,
                            holding.Note,
                            holding.LoanTypeID,
                            holding.AcquiredSourceID,
                            holding.Currency,
                            holding.Rate,
                            // số chứng từ
                            holding.RecordNumber,
                            // ngày chứng từ
                            holding.ReceiptedDate.ToString()

                            );
                        holding.CopyNumber = copyNumber;
                        holdings.Add(holding);

                    }
                    transaction.Commit();
                    if (!string.IsNullOrEmpty(recommendID))
                    {
                        InsertRecommend(recommendID, holding.ItemID);
                    }
                    composite = GenerateCompositeHoldings(holding.ItemID);



                }


            }
            else
            {
                return "ĐKCB đã tồn tại hãy sinh giá trị mới";
            }
            return "";
        }

        public string UpdateHolding(HoldingTable holding)
        {
           var currentHolding= db.HOLDINGs.Where(h=>h.ID == holding.ID).Single();
            //currentHolding.ID = holding.ID;
            //currentHolding.InCirculation = holding.InCirculation;
            //currentHolding.InUsed = holding.InUsed;
            //currentHolding.IsConfusion = holding.IsConfusion;
            //currentHolding.IsLost = holding.IsLost;
            //currentHolding.ItemID = holding.ItemID;
            //currentHolding.LibID = holding.LibID;
            //currentHolding.LoanTypeID = holding.LoanTypeID;
            //currentHolding.LocationID = holding.LocationID;
            //currentHolding.LockedReason = holding.LockedReason;
            currentHolding.Note = holding.Note;
          //  currentHolding.OnHold = holding.OnHold;
           // currentHolding.POID = holding.POID;
            currentHolding.Price = holding.Price;
            currentHolding.Rate = db.ACQ_CURRENCY.Where( c=>c.CurrencyCode == holding.Currency).Select( d=>d.Rate).Single();
          //  currentHolding.Reason = holding.Reason;
           // currentHolding.ReceiptedDate = DateTime.ParseExact(holding.ReceiptedDate,"dd/MM/yyyy",null) ;
            currentHolding.RecordNumber = holding.RecordNumber;
            currentHolding.Shelf = holding.Shelf;
         //   currentHolding.UseCount = holding.UseCount;
            currentHolding.Volume = holding.Volume;
         //   currentHolding.ILLID = holding.ILLID;
          //  currentHolding.DateLastUsed = DateTime.ParseExact(holding.Date, "dd/MM/yyyy", null);
            currentHolding.Currency = holding.Currency;
          //  currentHolding.CopyNumber = holding.CopyNumber;
            currentHolding.CallNumber = holding.CallNumber;
           // currentHolding.Availlable = holding.Availlable;
          //  currentHolding.AcquiredSourceID = holding.AcquiredSourceID;
          if(holding.AcquiredDate != null)
            {
                currentHolding.AcquiredDate = DateTime.ParseExact(holding.AcquiredDate, "yyyy-MM-dd", null);
            }
            
            if(holding.ReceiptedDate!= null)
            {
                currentHolding.ReceiptedDate = DateTime.ParseExact(holding.ReceiptedDate, "yyyy-MM-dd", null);
            }           
            //   currentHolding.Acquired = holding.Acquired;
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
       
            return "Cập nhật thành công!";
        }

        public string GetHoldingStatus(bool InUsed, bool InCirculation,bool Acquired)
        {

            string inUsed = InUsed ? "1" : "0";
            string inCirculation = InCirculation ? "1" : "0";
            string acquired = Acquired ? "1" : "0";
            string InUsed_InCirculation_Acquired = inUsed + inCirculation + acquired;
            switch (InUsed_InCirculation_Acquired) {
                case "011": return "<p style='color: #28a745'>Lưu thông<p>";
                case "001": return "Khóa";
                case "010": return "<p style='color: #dc3545'>Chưa kiểm nhận<p>";
                case "111": return "<p style='color: #deaa0f'>Đang cho mượn<p>";
                case "110": return "<p style='color: #dc3545'>Chưa kiểm nhận<p>";
                case "000": return "<p style='color: #dc3545'>Chưa kiểm nhận<p>";
                case "101": return "<p style='color: #dc3545'>Khóa<p>";
                case "100": return "<p style='color: #dc3545'>Chưa kiểm nhận<p>";
                default: return "";
            }
        }

        public string SearchItem(string title,string copynumber,string author,string publisher,string year,string isbn,ref List<SP_GET_TITLES_Result> data)
        {
            string querry = "SELECT DISTINCT ID FROM ITEM WHERE 1 = 1";
            if (!string.IsNullOrEmpty(title))
            {
                List<int> itemList = db.ITEM_TITLE.Where(it => it.Title.Contains(title.Trim())).Select(i => i.ItemID).ToList();
                if (itemList.Count > 0)
                {
                    querry = querry + " AND ITEM.ID IN (" + string.Join(",", itemList) + ")";
                }else
                {
                    return "Không tìm thấy biểu ghi phù hợp";
                }
            }
            if (!string.IsNullOrEmpty(copynumber))
            {
                List<int> itemList = db.HOLDINGs.Where(h => h.CopyNumber.Contains(copynumber.Trim())).Select(i => i.ItemID).ToList();
                if (itemList.Count > 0)
                {
                    querry = querry + " AND ITEM.ID IN (" + string.Join(",", itemList) + ")";
                }
                else
                {
                    return "Không tìm thấy biểu ghi phù hợp";
                }
            }
            if (!string.IsNullOrEmpty(author))
            {
                List<int> itemList = db.ITEM_AUTHOR.Where(it => it.CAT_DIC_AUTHOR.DisplayEntry.Contains(author.Trim())).Select(i => i.ItemID).ToList();
                if (itemList.Count > 0)
                {
                    querry = querry + " AND ITEM.ID IN (" + string.Join(",", itemList) + ")";
                }
                else
                {
                    return "Không tìm thấy biểu ghi phù hợp";
                }
            }
            if (!string.IsNullOrEmpty(publisher))
            {
                List<int> itemList = db.ITEM_PUBLISHER.Where(it => it.CAT_DIC_PUBLISHER.DisplayEntry.Contains(publisher.Trim())).Select(i => i.ItemID).ToList();
                if (itemList.Count > 0)
                {
                    querry = querry + " AND ITEM.ID IN (" + string.Join(",", itemList) + ")";
                }
                else
                {
                    return "Không tìm thấy biểu ghi phù hợp";
                }
            }
            if (!string.IsNullOrEmpty(year))
            {
                List<int> itemList = db.CAT_DIC_YEAR.Where(it => it.Year.Contains(year.Trim())).Select(i => i.ItemID).ToList();
                if (itemList.Count > 0)
                {
                    querry = querry + " AND ITEM.ID IN (" + string.Join(",", itemList) + ")";
                }
                else
                {
                    return "Không tìm thấy biểu ghi phù hợp";
                }
            }
            if (!string.IsNullOrEmpty(isbn))
            {
                List<int> itemList = db.CAT_DIC_NUMBER.Where(it => it.Number.Contains(isbn.Trim()) && it.FieldCode.Equals("020")).Select(i => i.ItemID).ToList();
                if (itemList.Count > 0)
                {
                    querry = querry + " AND ITEM.ID IN (" + string.Join(",", itemList) + ")";
                }
                else
                {
                    return "Không tìm thấy biểu ghi phù hợp";
                }
            }

            if (querry.Equals("SELECT DISTINCT ID FROM ITEM WHERE 1 = 1"))
            {
                return "Hãy điền thông tin tìm kiếm";
            }


            data = FPT_SP_GET_TITLES(querry);   
            if (data == null)
            {
                return "Không tìm thấy biểu ghi phù hợp";
            }

            foreach (var item in data)
            {
                //item.Title = GetContent(item.Title);
                item.Title = new SupportClass.FormatHoldingTitle().OnFormatHoldingTitle(item.Title);
            }
            return "";
        }

        public string GenerateCompositeHoldings(int itemID)
        {
            List<CompositeHolding> listCompositeHolding = new List<CompositeHolding>();
            listCompositeHolding = db.HOLDINGs.Where( h => h.ItemID == itemID)
                            .Join(db.HOLDING_LIBRARY, holding => holding.LibID, lib => lib.ID, (holding, lib) => new { HOLDING = holding, HOLDING_LIBRARY = lib })
                            .Join(db.HOLDING_LOCATION, both => both.HOLDING.LocationID, loc => loc.ID, (both,loc)  => new { Both = both, Loc = loc } )
                             .OrderBy(o => o.Both.HOLDING.CopyNumber).ToList().Select(x => new CompositeHolding()
                             {
                                 LibID = x.Both.HOLDING_LIBRARY.ID,
                                 LibCode = x.Both.HOLDING_LIBRARY.Code,
                                 LocationID = x.Loc.ID,
                                 LocSymbol = x.Loc.Symbol,
                                 Copynumber = x.Both.HOLDING.CopyNumber
                             }).ToList();

            if (listCompositeHolding.Count <= 0)
            {
                return "";
            }

            var dictionaryComposite = new Dictionary<string, string>();
            var dictionaryMaxNumber = new Dictionary<string, int>();

            // bool isChangeGroupIDComposite = false;


            for (int i=0;i < listCompositeHolding.Count; i++)
            {
                string groupIDComposite = "" + listCompositeHolding[i].LibID+"," + listCompositeHolding[i].LocationID;
                
                if (!dictionaryComposite.Keys.Contains(groupIDComposite))
                {
                    if (i > 0)
                    {
                       string previousGroupIDComposite = "" + listCompositeHolding[i-1].LibID + "," + listCompositeHolding[i-1].LocationID;
                       string currentContent = dictionaryComposite[previousGroupIDComposite];
                       dictionaryComposite[previousGroupIDComposite]= currentContent.Substring(0, currentContent.Length -1);
                    }

                    // start copynumber
                    string value = listCompositeHolding[i].LibCode + " / " + listCompositeHolding[i].LocSymbol + " / " + listCompositeHolding[i].Copynumber +"-";
                    string number = listCompositeHolding[i].Copynumber.Substring(listCompositeHolding[i].LocSymbol.Length);
                    var isNumeric = int.TryParse(number, out int currentMaxNumber);
                    dictionaryComposite.Add(groupIDComposite,value);
                    if (isNumeric)
                    {
                        dictionaryMaxNumber.Add(groupIDComposite, currentMaxNumber);
                    }
                    else
                    {
                        return "Không thể load dữ liệu";
                    }

                }
                else
                {
                   string value = dictionaryComposite[groupIDComposite];
                   int existedMaxNumber = dictionaryMaxNumber[groupIDComposite];
                    string number = listCompositeHolding[i].Copynumber.Substring(listCompositeHolding[i].LocSymbol.Length);
                    var isNumeric = int.TryParse(number, out int currentMaxNumber);
                    // validate
                    if (isNumeric)
                    {
                        // copynumber lien tiếp
                        if (currentMaxNumber == (existedMaxNumber+1))
                        {
                            if (value[value.Length-1].Equals('-'))
                            {
                                // điền end copynumber
                                dictionaryComposite[groupIDComposite] = value + currentMaxNumber + ",";
                            }
                            else if (value[value.Length - 1].Equals(','))
                            {
                                dictionaryComposite[groupIDComposite] = value.Substring(0, value.Length - currentMaxNumber.ToString().Length - 1) + currentMaxNumber +",";
                            }

                            dictionaryMaxNumber[groupIDComposite] = currentMaxNumber;
                        }
                        // đứt quãng
                        else
                        {
                           char test =  value[value.Length - 1];
                            // trường hợp ở giữa bị xóa 1 đăng ký cá biệt
                            if (value[value.Length - 1].Equals('-'))
                            {
                                // điền start 
                                dictionaryComposite[groupIDComposite] = value.Substring(0, value.Length -1 ) +"," + currentMaxNumber + "-";
                            }
                            else if (value[value.Length - 1].Equals(','))
                            {
                                dictionaryComposite[groupIDComposite] = value + currentMaxNumber +"-";
                            }

                            dictionaryMaxNumber[groupIDComposite] = currentMaxNumber;
                        }
                    }else
                    {
                        return "Không thể load dữ liệu";
                    }
                }
            }
            string finalGroupIDComposite = "" + listCompositeHolding[listCompositeHolding.Count-1].LibID + "," + listCompositeHolding[listCompositeHolding.Count - 1].LocationID;
            string currentText = dictionaryComposite[finalGroupIDComposite];
            dictionaryComposite[finalGroupIDComposite] = currentText.Substring(0, currentText.Length - 1);

            List<string> content = dictionaryComposite.Select(x => x.Value).ToList();
            string result="";
            for (int i = 0; i < content.Count; i++)
            {
                result = result + content[i] + "<br/>";
            }
           
            return result;
        }

        public string GetContent(string copynumber)
        {
            string validate = copynumber.Replace("$a", "");
            validate = validate.Replace("$b", "");
            validate = validate.Replace("$c", "");
            validate = validate.Replace(",$c ", "");
            validate = validate.Replace("=$b", "");
            validate = validate.Replace(":$b", "");
            validate = validate.Replace("/$c", "");
            validate = validate.Replace(".$n", "");
            validate = validate.Replace(":$p", "");
            validate = validate.Replace(";$c", "");
            validate = validate.Replace("+$e", "");
            validate = validate.Replace("$e", "");

            return validate;
        }

        public string InsertRecommend(string recommendID,int itemID)
        {
            FPT_RECOMMEND currentRecommend = db.FPT_RECOMMEND.FirstOrDefault(s => s.RecommendID.ToLower() == recommendID.ToLower());
            ITEM item = db.ITEMs.FirstOrDefault( i => i .ID == itemID);
            
            if (currentRecommend != null && item!= null)
            {
                currentRecommend.ITEMs.Add(item);
            }
            else if (currentRecommend == null && item != null)
            {
                currentRecommend = new FPT_RECOMMEND() { RecommendID = recommendID };
                currentRecommend.ITEMs.Add(item);
                db.FPT_RECOMMEND.Add(currentRecommend);
            }
            db.SaveChanges();

            return "";
        }

        public List<SP_HOLDING_LIBRARY_SELECT_Result> FPT_SP_HOLDING_LIBRARY_SELECT(int libID, int localLibId, int statusId, int userId, int typeId)
        {
            List<SP_HOLDING_LIBRARY_SELECT_Result> list = db.Database.SqlQuery<SP_HOLDING_LIBRARY_SELECT_Result>("SP_HOLDING_LIBRARY_SELECT {0}, {1}, {2},{3},{4}",
                new object[] { libID, localLibId, statusId, userId, typeId }).ToList();
            return list;
        }
        public List<SP_HOLDING_LOCATION_GET_INFO_Result> FPT_SP_HOLDING_LOCATION_GET_INFO(int libID, int userId, int locId, int statusId)
        {
            List<SP_HOLDING_LOCATION_GET_INFO_Result> list = db.Database.SqlQuery<SP_HOLDING_LOCATION_GET_INFO_Result>("SP_HOLDING_LOCATION_GET_INFO {0}, {1}, {2},{3}",
                new object[] { libID, userId, locId, statusId }).ToList();
            return list;
        }
        public List<SP_GET_TITLES_Result> FPT_SP_GET_TITLES(string querry)
        {
            string commandStatement = "SELECT  Code,Content AS TITLE,Ind1 FROM ITEM LEFT JOIN FIELD200s ON ITEM.ID = FIELD200s.ItemID WHERE FIELD200s.FieldCode = '245' and ITEM.ID in (" + querry +")";
            Debug.WriteLine("Querry: " + commandStatement);
            List<SP_GET_TITLES_Result> list = db.Database.SqlQuery<SP_GET_TITLES_Result>(commandStatement).ToList();
            return list;
        }
    }


    public class CompositeHolding
    {
        public int LibID { get; set; }
        public int LocationID { get; set; }
        public string LibCode { get; set; }
        public string LocSymbol { get; set; }
        public string Copynumber { get; set; }


    }
}