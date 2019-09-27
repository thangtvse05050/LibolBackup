using Libol.EntityResult;
using System;
using System.Collections.Generic;
using System.Data;
using Libol.SupportClass;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Libol.Models
{

    public class CatalogueBusiness
    {
        LibolEntities db = new LibolEntities();

        public CatalogueBusiness()
        {

        }



        public List<GET_CATALOGUE_FIELDS_Result> GetComplatedForm(int intIsAuthority, string strCreator, int SelectedIndex)
        {
            List<FPT_SP_CATA_GETFIELDS_OF_FORM_Result> GetForm = db.FPT_SP_CATA_GETFIELDS_OF_FORM(SelectedIndex, "", 0).ToList();
            string fields = "";
            foreach (FPT_SP_CATA_GETFIELDS_OF_FORM_Result item in GetForm)
            {
                if (item.FieldCode != "001")
                    fields = fields + item.FieldCode + ",";
            }

            List<GET_CATALOGUE_FIELDS_Result> list = GET_CATALOGUE_FIELDS(intIsAuthority, SelectedIndex, fields, "", 0);
            return list;
        }

        public List<GET_CATALOGUE_FIELDS_Result> GET_CATALOGUE_FIELDS(int intIsAuthority, int intFormID, string strFieldCodes, string strAddedFieldCodes, int intGroupBy)
        {
            List<GET_CATALOGUE_FIELDS_Result> list = db.Database.SqlQuery<GET_CATALOGUE_FIELDS_Result>("SP_CATA_GET_CATALOGUE_FIELDS {0}, {1}, {2},{3},{4}",
                new object[] { intIsAuthority, intFormID, strFieldCodes, strAddedFieldCodes, 0 }).ToList();
            return list;
        }

        public List<int> SearchIDByCondition(string strCode, string strCN, string strTT, string ISBN)
        {
            List<int> ItemIds = new List<int>();

            if (strCode != "")
            {
                int id = db.ITEMs.Where(a => a.Code == strCode).Select(a => a.ID).FirstOrDefault();
                ItemIds.Add(id);
            }
            if (strCN != "")
            {
                int id = db.HOLDINGs.Where(a => a.CopyNumber == strCN).Select(a => a.ItemID).FirstOrDefault();
                ItemIds.Add(id);
            }
            if (strTT != "")
            {
                strTT = strTT.ToUpper();
                List<int> id = db.ITEM_TITLE.Where(a => a.Title.Contains(strTT)).Select(a => a.ItemID).ToList();
                ItemIds = ItemIds.Concat(id).ToList();
            }
            if (ISBN != "")
            {
                int id = db.CAT_DIC_NUMBER.Where(a => a.Number == ISBN).Select(a => a.ItemID).Distinct().FirstOrDefault();
                ItemIds.Add(id);
            }



            return ItemIds;
        }


        //Seach Code for Update Catalogue
        public List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> SearchCode(string strCode, string strCN, string strTT)
        {
            List<int> ItemId = SearchIDByCondition(strCode, strCN, strTT, "");

            //get List Infor detail
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> inforList = new List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result>();
            foreach (int item in ItemId)
            {
                //inforList = inforList.Concat(db.FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM(item.ToString(), 0).ToList()).ToList();
                List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> inforListTemp = db.FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM(item.ToString(), 0).ToList();
                for (int i = 0; i < inforListTemp.Count; i++)
                {
                    if (inforListTemp[i].FieldCode == "001")
                    {
                        inforList.Add(inforListTemp[i]);
                    }
                    if (inforListTemp[i].FieldCode == "245")
                    {
                        inforListTemp[i].Content = new FormatHoldingTitle().OnFormatHoldingTitle(inforListTemp[i].Content);
                        inforList.Add(inforListTemp[i]);
                    }
                }
            }

            return inforList;
        }



        public List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> GetContentByID(string Id)
        {
            List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> list = db.FPT_SP_CATA_GET_CONTENTS_OF_ITEMS(Id, 0).ToList();

            //Ghep Cac truong trung nhau thanh 1 dong
            List<int> index = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    if (list[i].FieldCode == list[i - 1].FieldCode)
                    {
                        index.Add(i - 1);
                        list[i].Content = list[i - 1].Content + "::" + list[i].Content;
                    }
                    //if (listContent[i].FieldCode.StartsWith("852"))
                    //{
                    //    index.Add(i);
                    //}
                }

            }
            //remove các trường trùng đã được ghép
            for (int i = 0; i < index.Count; i++)
            {
                list.RemoveAt(index[i] - i);
            }
            return list;
        }



        //Check TITTLE
        public List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> CheckTitle(string TT)
        {
            TT = TT.ToUpper();
            List<int> listId = db.ITEM_TITLE.Where(a => a.Title.Contains(TT)).Select(a => a.ItemID).ToList();
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> inforList = new List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result>();
            foreach (int item in listId)
            {
                inforList = inforList.Concat(db.FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM(item.ToString(), 0).ToList()).ToList();
            }
            foreach (FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result item in inforList)
            {
                item.Content = new FormatHoldingTitle().OnFormatHoldingTitle(item.Content);

            }
            return inforList;
        }

        public List<SP_CATA_GET_MODIFIED_FIELDS_Result> FPT_SP_CATA_GET_MODIFIED_FIELDS(Nullable<int> intIsAuthority, Nullable<int> intFormID, string strFieldCodes, string strAddedFieldCodes, string strUsedFieldCodes, Nullable<int> intGroupBy)
        {
            List<SP_CATA_GET_MODIFIED_FIELDS_Result> list = db.Database.SqlQuery<SP_CATA_GET_MODIFIED_FIELDS_Result>("SP_CATA_GET_MODIFIED_FIELDS {0}, {1}, {2},{3},{4},{5}",
                new object[] { intIsAuthority, intFormID, strFieldCodes, strAddedFieldCodes, strUsedFieldCodes, intGroupBy }).ToList();
            return list;
        }

        public List<FPT_SP_CATA_SEARCH_MARC_FIELDS_Results> FPT_SP_CATA_SEARCH_MARC_FIELDS(string strPattern, Nullable<int> intHaveParentFieldCode, Nullable<int> intIsAuthority, string strFCURL1, string strFCURL2)
        {
            List<FPT_SP_CATA_SEARCH_MARC_FIELDS_Results> list = db.Database.SqlQuery<FPT_SP_CATA_SEARCH_MARC_FIELDS_Results>("SP_CATA_SEARCH_MARC_FIELDS {0}, {1}, {2},{3},{4}",
                new object[] { strPattern, intHaveParentFieldCode, intIsAuthority, strFCURL1, strFCURL2 }).ToList();
            return list;
        }


        public string InsertItem(ref ITEM item)
        {
            int formId = item.FormID;
            string recordType = item.RecordType;
            int mediumId = item.MediumID;
            int typeId = item.TypeID;
            string bibLevel = item.BibLevel;

            string callNumber = item.CallNumber;

            // check FormID,RecordType,... 
            if (!db.MARC_WORKSHEET.Any(m => m.ID == formId)
                || !db.CAT_DIC_RECORDTYPE.Any(m => m.Code == recordType)
                || !db.CAT_DIC_MEDIUM.Any(m => m.ID == mediumId)
                || !db.CAT_DIC_ITEM_TYPE.Any(m => m.ID == typeId)
                || !db.CAT_DIC_DIRLEVEL.Any(m => m.Code == bibLevel))
            {
                return "";
            }


            int id = db.ITEMs.Select(i => i.ID).Max() + 1;
            // leader 
            string leader = "";
            if (item.BibLevel == "m")
            {
                leader = "00025n" + item.RecordType + item.BibLevel + " a2200024 a 4500";
            }
            else
            {
                leader = "00025n" + item.RecordType + item.BibLevel + " a22        4500";
            }

            string cataloguer = Convert.ToString(HttpContext.Current.Session["FullName"]);
            // Mã tự tăng
            //string sysParam = db.SYS_PARAMETER.Where(s => s.Name.Equals("LIBRARY_ABBREVIATION")).Select(s => s.Val).FirstOrDefault();
            //string year = DateTime.Now.Year.ToString();
            //year = year.Substring(year.Length - 2);
            //db.Database.ExecuteSqlCommand("insert into book_code values(1)");
            //string maxIdBookCode = db.Book_code.Select(b => b.ID).Max().ToString().PadLeft(7, '0');
            //string code = sysParam + year + maxIdBookCode;
            string code = CreateItemCode();
            ITEM newItem = new ITEM()
            {
                AccessLevel = item.AccessLevel,
                BibLevel = item.BibLevel,
                Code = code,
                Leader = leader,
                NewRecord = true,
                MediumID = item.MediumID,
                FormID = item.FormID,
                RecordType = item.RecordType,
                TypeID = item.TypeID,
                CreatedDate = DateTime.Now,
                OPAC = true,
                Cataloguer = cataloguer,
                Reviewer = "",
                CoverPicture = "",
                ID = id,
                //doanhdq bo xung
                CallNumber = item.CallNumber,
                SourceAgencyID = item.SourceAgencyID


            };
            db.ITEMs.Add(newItem);
            db.SaveChanges();
            item = newItem;
            return code;
        }

        public string CreateItemCode()
        {
            string sysParam = db.SYS_PARAMETER.Where(s => s.Name.Equals("LIBRARY_ABBREVIATION")).Select(s => s.Val).FirstOrDefault();
            string year = DateTime.Now.Year.ToString();
            year = year.Substring(year.Length - 2);
            db.Database.ExecuteSqlCommand("insert into book_code values(1)");
            string maxIdBookCode = db.Book_code.Select(b => b.ID).Max().ToString().PadLeft(7, '0');
            string code = sysParam + year + maxIdBookCode;

            return code;
        }

        public string UpdateItem(List<string> listFieldName, List<string> listFieldValue, List<string> listFieldOrg, List<string> listValueOrg)
        {

            string ItemCode = listFieldValue[listFieldName.IndexOf("001")];
            listFieldValue.RemoveAt(listFieldName.IndexOf("001"));
            listFieldName.RemoveAt(listFieldName.IndexOf("001"));
            string ItemID = "";
            //If Code = null thì là trường Hợp Tạo mới Item và ngược lại là Update
            //Get ITEM information

            /////////////////////////////BEGIN TRANS///////////////////////////////
            ///
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                //try
                //{



                if (String.IsNullOrEmpty(ItemCode))
                {
                    string code = ""; //New Code for Insert
                    if (listFieldValue.Count > 0 && listFieldName.Count > 0)
                    {
                        byte accessLevel = 0;
                        int typeId = 1, mediumId = 3, formId = 14;
                        string coverPicture = "", bibLevel = "", recordType = "", sourceAgencyID = "", callNumber = "";
                        // insert item 

                        //sourceAgencyID
                        if (listFieldName.Contains("040$a"))
                        {
                            int index = listFieldName.IndexOf("040$a");
                            string valueAgency = listFieldValue[index];
                            sourceAgencyID = PickReferenceID("HOLDING_LIBRARY", valueAgency);
                        }
                        else
                        {
                            sourceAgencyID = null;
                        }

                        //CallNumber
                        if (listFieldName.Contains("090$a") && listFieldName.Contains("090$b"))
                        {
                            int index = listFieldName.IndexOf("090$a");
                            callNumber = listFieldValue[listFieldName.IndexOf("090$a")] + " " + listFieldValue[listFieldName.IndexOf("090$b")];
                        }
                        else
                        {
                            callNumber = null;
                        }

                        if (listFieldName.Contains("926"))

                        {
                            int index = listFieldName.IndexOf("926");
                            accessLevel = Convert.ToByte(listFieldValue[index]);
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        if (listFieldName.Contains("927"))
                        {
                            int index = listFieldName.IndexOf("927");
                            typeId = Convert.ToInt32(listFieldValue[listFieldName.IndexOf("927")]);
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }

                        if (listFieldName.Contains("907"))
                        {
                            int index = listFieldName.IndexOf("907");
                            coverPicture = listFieldValue[listFieldName.IndexOf("907")];
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }

                        if (listFieldName.Contains("DirLevel"))
                        {
                            int index = listFieldName.IndexOf("DirLevel");
                            bibLevel = listFieldValue[listFieldName.IndexOf("DirLevel")];
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        if (listFieldName.Contains("RecordType"))
                        {
                            int index = listFieldName.IndexOf("RecordType");
                            recordType = listFieldValue[listFieldName.IndexOf("RecordType")];
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        if (listFieldName.Contains("925"))
                        {
                            int index = listFieldName.IndexOf("925");
                            mediumId = Convert.ToInt32(listFieldValue[listFieldName.IndexOf("925")]);
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        if (listFieldName.Contains("FormId"))
                        {
                            int index = listFieldName.IndexOf("FormId");
                            formId = Convert.ToInt32(listFieldValue[listFieldName.IndexOf("FormId")]);
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }

                        ITEM item = new ITEM()
                        {
                            AccessLevel = accessLevel,
                            MediumID = mediumId,
                            TypeID = typeId,
                            CoverPicture = coverPicture,
                            FormID = formId,
                            RecordType = recordType,
                            BibLevel = bibLevel,
                            CallNumber = callNumber
                        };
                        //add Item
                        InsertItem(ref item);
                        code = item.Code;
                        //Add thông tin các trường điều khiển vào bảng tương ứng
                        //Doanhdq Bổ xung
                    }
                    ItemID = db.SP_GET_ITEMID_BYCODE(code).FirstOrDefault().ToString(); //New ItemID after Insert New Item
                }
                else
                //CODE Đã tồn tại THÌ THỰC HIỆN UPDATE
                {
                    ItemID = db.SP_GET_ITEMID_BYCODE(ItemCode).FirstOrDefault().ToString();
                    int ID = Int32.Parse(ItemID);

                    //Update ITEM Infor By ItemID
                    if (listFieldValue.Count > 0 && listFieldName.Count > 0)
                    {
                        ITEM item = db.ITEMs.Single(u => u.ID == ID);
                        string Leader = listFieldValue[listFieldName.IndexOf("000")];
                        listFieldValue.RemoveAt(listFieldName.IndexOf("000"));
                        listFieldName.RemoveAt(listFieldName.IndexOf("000"));



                        // UPDATE item 

                        item.Leader = Leader;
                        item.BibLevel = Leader.Substring(7, 1);
                        item.RecordType = Leader.Substring(6, 1);
                        //Update sourceAgencyID
                        if (listFieldName.Contains("040$a"))
                        {
                            string valueAgency = listFieldValue[listFieldName.IndexOf("040$a")];
                            item.SourceAgencyID = Int32.Parse(PickReferenceID("HOLDING_LIBRARY", valueAgency));
                        }

                        //CallNumber
                        if (listFieldName.Contains("090$a") && listFieldName.Contains("090$b"))
                        {
                            string NewCN = listFieldValue[listFieldName.IndexOf("090$a")] + " " + listFieldValue[listFieldName.IndexOf("090$b")];
                            item.CallNumber = NewCN;
                            //Update CallNumebr in Hoding table
                            var some = db.HOLDINGs.Where(x => x.ItemID == ID).ToList();
                            some.ForEach(cn => cn.CallNumber = NewCN);
                            //(from p in db.HOLDINGs where p.CallNumber == OldCN select p ).ToList().ForEach( x=> x.is)
                        }
                        //NewRecord
                        if (listFieldName.Contains("900"))
                        {
                            int index = listFieldName.IndexOf("900");
                            item.NewRecord = (Int32.Parse(listFieldValue[index]) == 0) ? false : true;
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        //coverPic
                        if (listFieldName.Contains("907"))
                        {
                            int index = listFieldName.IndexOf("907");
                            item.CoverPicture = listFieldValue[index];
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        //TypeID
                        if (listFieldName.Contains("927"))
                        {
                            int index = listFieldName.IndexOf("927");
                            string valueType = listFieldValue[index];
                            item.TypeID = Int32.Parse(PickReferenceID("CAT_DIC_ITEM_TYPE", valueType));
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        //AccessLv
                        if (listFieldName.Contains("926"))
                        {
                            int index = listFieldName.IndexOf("926");
                            item.AccessLevel = Byte.Parse(listFieldValue[index]);
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        //MediumID
                        if (listFieldName.Contains("925"))
                        {
                            int index = listFieldName.IndexOf("925");
                            string valueMedium = listFieldValue[index];
                            item.MediumID = Int32.Parse(PickReferenceID("CAT_DIC_MEDIUM", valueMedium));
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        //Cataloguer
                        if (listFieldName.Contains("911"))
                        {
                            int index = listFieldName.IndexOf("911");
                            item.Cataloguer = listFieldValue[index];
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }
                        //Reviewer
                        if (listFieldName.Contains("912"))
                        {
                            int index = listFieldName.IndexOf("912");
                            item.Reviewer = listFieldValue[index];
                            listFieldName.RemoveAt(index);
                            listFieldValue.RemoveAt(index);
                        }


                        db.SaveChanges();
                    }


                }

                //****************************************************DONE INSERT ITEM(table)****************************************
                //************************************************************************************************************

                //get ItemID By Code


                //Loaị bỏ các field null hoặc trống ***************************************************************************
                //Sử Dụng trong TH Insert New
                List<int> indexNull = new List<int>();
                for (int i = 0; i < listFieldName.Count(); i++)
                {
                    if (listFieldValue[i] == null || listFieldValue[i] == "")
                    {
                        indexNull.Add(i);

                    }
                    else
                    {
                        listFieldValue[i] = listFieldValue[i].Replace("'", "''");
                    }
                }
                for (int i = 0; i < indexNull.Count(); i++)
                {
                    if (i > 0)
                    {
                        listFieldName.RemoveAt(indexNull[i] - i);
                        listFieldValue.RemoveAt(indexNull[i] - i);
                    }
                    else
                    {
                        listFieldName.RemoveAt(indexNull[i]);
                        listFieldValue.RemoveAt(indexNull[i]);
                    }
                }

                //throw new Exception();
                // Update * ***************************************** UPDATE CÁC BẢNG PHỤC VỤ TÌM KIẾM * ************************************************
                //************************************************************************************************************
                List<string> listFieldDeleted = new List<string>();
                for (int i = 0; i < listFieldName.Count(); i++)
                {
                    //Update ****************************************** LANGUAGE  *************************************************
                    //************************************************************************************************************
                    if (listFieldName[i] == "008")
                    {
                        if (ItemCode != null && listFieldDeleted.IndexOf("008") == -1)
                        {
                            db.Database.ExecuteSqlCommand("DELETE FROM ITEM_LANGUAGE WHERE FieldCode = '008' AND ItemID = " + ItemID);
                            listFieldDeleted.Add("008");
                        }
                        //Update ******************************************DONE LANGUAGE  *************************************************
                        //************************************************************************************************************
                        UpdateLanguage(ItemID, listFieldValue[i], "008");
                    }
                    ////Sử dụng cho các bảng ITEM_TITLE
                    /////Update ****************************************** TITTLE *************************************************
                    //************************************************************************************************************
                    if (listFieldName[i] == "245$a" || listFieldName[i].StartsWith("245$b") || listFieldName[i] == "245$p")
                    {
                        if (ItemCode != null)
                        {
                            if (listFieldDeleted.IndexOf("245") == -1)
                            {
                                db.Database.ExecuteSqlCommand("DELETE FROM ITEM_TITLE WHERE FieldCode = '245' AND ItemID = " + ItemID);
                                listFieldDeleted.Add("245");
                            }

                            //Xoa b ki tu noi truoc khi UPDATE
                            if (listFieldValue[i].LastIndexOf("=") == listFieldValue[i].Length - 1 || listFieldValue[i].LastIndexOf(":") == listFieldValue[i].Length - 1 || listFieldValue[i].LastIndexOf(",") == listFieldValue[i].Length - 1 || listFieldValue[i].LastIndexOf("/") == listFieldValue[i].Length - 1)
                            {
                                //Cat nhan de song song
                                if (listFieldValue[i].Substring(listFieldValue[i].Length - 1) == "=")
                                    listFieldName[i + 1] = "245$b1";
                                //cat phu de
                                if (listFieldValue[i].Substring(listFieldValue[i].Length - 1) == ":")
                                    listFieldName[i + 1] = "245$b2";
                                listFieldValue[i] = listFieldValue[i].Remove(listFieldValue[i].Length - 1, 1);
                                UpdateItemTitle(ItemID, listFieldValue[i], "245");
                            }
                            else
                            {
                                UpdateItemTitle(ItemID, listFieldValue[i], "245");
                            }
                        }
                        else
                        {
                            //trường hợp lặp trường con (không phải trường lặp - ví dụ : 245 có 2 $b)
                            if (listFieldValue[i].Contains("//"))
                            {
                                string valTemp = listFieldValue[i];
                                valTemp = valTemp.Replace("//", "$");
                                var arrTemp = valTemp.Split('$');
                                foreach (string val in arrTemp)
                                {
                                    UpdateItemTitle(ItemID, val, "245");
                                }
                            }
                            else
                            {
                                UpdateItemTitle(ItemID, listFieldValue[i], "245");
                            }


                        }
                        //Update *****************************************DONE TITTLE *************************************************
                        //************************************************************************************************************

                    }


                }

                //Update ***********************FULL TEXT **********************************
                string fullText = String.Join(" ", listFieldValue);
                if (ItemCode != null)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM ITEM_FULLTEXT WHERE ItemID = " + ItemID);
                    //Loc truong 000 (leader)
                }
                UpdateItemFulltext(ItemID, fullText);

                //Nối chuối phục vụ việc select query
                string strFields = String.Join("','", listFieldName);
                //Update ***********************DONE FULL TEXT **********************************


                //**************************************************** UPDATE REFERENCE TABLES**********************************************
                //*************************************************************************************************************************
                List<FPT_SP_CATA_GET_DICINFOR_REFERENCES_Result> listRef = db.Database.SqlQuery<FPT_SP_CATA_GET_DICINFOR_REFERENCES_Result>("SELECT ID, FieldCode, DicID, FunctionID, FieldTypeID, LinkTypeID FROM MARC_BIB_FIELD WHERE(ID IN(SELECT ID FROM MARC_BIB_FIELD WHERE FieldCode IN('" + strFields + "')) OR ParentFieldCode IN('" + strFields + "')) AND(DicID > 0 OR FunctionID IN(6, 7, 8, 9, 14)) ORDER BY FieldCode").ToList();
                foreach (FPT_SP_CATA_GET_DICINFOR_REFERENCES_Result item in listRef)
                {

                    switch (item.FunctionID == null ? 0 : item.FunctionID)
                    {
                        case 6:
                            //["FieldTypeID"]) == 4
                            //UPDATE CAT_EDATA_FILE
                            break;
                        case 7:
                            //["FieldTypeID"]) == 7
                            //DELETE FROM ITEM_LINK
                            break;
                        case 9:
                            List<int> indexSN = Enumerable.Range(0, listFieldName.Count).Where(i => listFieldName[i] == item.FieldCode).ToList();
                            foreach (int i in indexSN)
                            {
                                if (ItemCode != null && listFieldDeleted.IndexOf(item.FieldCode) == -1)
                                {
                                    db.Database.ExecuteSqlCommand("DELETE FROM CAT_DIC_NUMBER WHERE FieldCode = '" + item.FieldCode + "' AND ItemID = " + ItemID);
                                    listFieldDeleted.Add(item.FieldCode);
                                }
                                UpdateStandardNumber(ItemID, listFieldValue[i], item.FieldCode);
                            }
                            break;
                        case 14:
                            List<int> indexY = Enumerable.Range(0, listFieldName.Count).Where(i => listFieldName[i] == item.FieldCode).ToList();
                            foreach (int i in indexY)
                            {
                                if (ItemCode != null && listFieldDeleted.IndexOf(item.FieldCode) == -1)
                                {
                                    db.Database.ExecuteSqlCommand("DELETE FROM CAT_DIC_YEAR WHERE FieldCode = '" + item.FieldCode + "' AND ItemID = " + ItemID);
                                    listFieldDeleted.Add(item.FieldCode);
                                }
                                UpdateYear(ItemID, listFieldValue[i], item.FieldCode);
                            }
                            break;
                        default:
                            if (item.DicID != null)
                            {
                                List<int> indexRef = Enumerable.Range(0, listFieldName.Count).Where(i => listFieldName[i] == item.FieldCode).ToList();
                                foreach (int i in indexRef)
                                {
                                    if (listFieldDeleted.IndexOf(item.FieldCode) == -1)
                                    {
                                        UpdateReference(ItemID, listFieldValue[i], item.FieldCode, item.DicID, ItemCode == null ? 1 : 0, true);
                                        listFieldDeleted.Add(item.FieldCode);
                                    }
                                    else
                                    {
                                        UpdateReference(ItemID, listFieldValue[i], item.FieldCode, item.DicID, ItemCode == null ? 1 : 0, false);
                                    }

                                }

                            }
                            break;
                    }

                }
                //****************************************************DONE INSERT REFERENCE (tables)***************************
                //************************************************************************************************************



                //****************************************************START INSERT Block Fields(tables)***************************
                //************************************************************************************************************
                if (ItemCode == null)
                {
                    //INSERT = 1
                    ParseFieldsValue(listFieldName, listFieldValue, ItemID);
                }
                else
                {
                    //UPDATE = 0
                    List<string> FieldControl = new List<string> { "000", "001", "852", "900", "907", "911", "912", "925", "926", "927" };
                    foreach (string value in FieldControl)
                    {
                        if (listFieldOrg.Contains(value))
                        {
                            listValueOrg.RemoveAt(listFieldOrg.IndexOf(value));
                            listFieldOrg.RemoveAt(listFieldOrg.IndexOf(value));
                        }
                    }
                    for (int i = 0; i < listValueOrg.Count; i++)
                    {
                        if (listValueOrg[i].Contains("'"))
                        {
                            listValueOrg[i] = listValueOrg[i].Replace("'", "''");
                        }
                    }
                    UpdateBlockField(listFieldOrg, listValueOrg, ItemID, 0);
                }
                transaction.Commit();
                //}
                //catch(Exception ex)
                //{
                //    transaction.Rollback();
                //    return "Error";
                //}
            }
            ////////////////////////////////////////END TRANS
            ////////////////////////////////////////END TRANS
            //****************************************************DONE INSERT Block Fields(tables)***************************
            //************************************************************************************************************

            return ItemID;//FPTxxxxxxx

        }


        //Parse Field : Ghép các trường với các kí tự nối + giá trị tương ứng =>>> UPDATE CÁC BẢNG FIELDS
        public void ParseFieldsValue(List<string> listFieldName, List<string> listFieldValue, string ItemID)
        {
            //if (action == 1)
            //{
            List<string> listFieldNameOutput = new List<string>();
            List<string> listFieldValueOutput = new List<string>();

            //Xử Lí ghép theo List các trường của Form Biên Mục Sách 2019
            //if (listFieldName.Count > 0 && listFieldValue.Count > 0)
            //{
            string outputValue245 = "";
            string outputValue300 = "";
            string outputValue260 = "";
            string outputValue090 = "";
            bool flag245 = false;
            bool flag300 = false;
            bool flag260 = false;
            bool flag090 = false;

            for (int i = 0; i < listFieldName.Count; i++)
            {
                bool checkInput = !String.IsNullOrEmpty(listFieldValue[i]);

                //if (listFieldName[i].Equals("001") && checkInput)
                //{
                //    // nothing
                //}
                string nameTmp = listFieldName[i];
                string valueTmp = listFieldValue[i];
                switch (nameTmp.Substring(0, 3))
                {
                    case "090":
                        if (listFieldName[i].Equals("090$a") && checkInput)
                        {
                            valueTmp = "$a" + valueTmp;
                        }
                        if (listFieldName[i].Equals("090$b") && checkInput)
                        {
                            valueTmp = "$b" + valueTmp;
                        }
                        flag090 = true;
                        outputValue090 = outputValue090 + valueTmp;

                        break;
                    case "245":
                        if (listFieldName[i].Equals("245$a") && checkInput)
                        {
                            valueTmp = "$a" + valueTmp;
                        }
                        //nhan de song song
                        if (listFieldName[i].Equals("245$b1") && checkInput)
                        {

                            valueTmp = "=$b" + valueTmp;
                            // dau "//" khi có 2 nhan đề song song (quy tắc nhập liệu)
                            if (valueTmp.Contains("//"))
                            {
                                valueTmp = valueTmp.Replace("//", "=$b");
                            }
                        }
                        //phu de
                        // dau "//" khi có 2 phụ đề (quy tắc nhập liệu)
                        if (listFieldName[i].Equals("245$b2") && checkInput)
                        {
                            valueTmp = ":$b" + valueTmp;
                            if (valueTmp.Contains("//"))
                            {
                                valueTmp = valueTmp.Replace("//", ":$b");
                            }
                        }
                        if (listFieldName[i].Equals("245$c") && checkInput)
                        {
                            valueTmp = "/$c" + valueTmp;
                        }
                        if (listFieldName[i].Equals("245$n") && checkInput)
                        {
                            valueTmp = ".$n" + valueTmp;
                        }
                        if (listFieldName[i].Equals("245$p") && checkInput)
                        {
                            valueTmp = ",$p" + valueTmp;
                        }
                        flag245 = true;
                        outputValue245 = outputValue245 + valueTmp;

                        break;
                    case "260":
                        if (listFieldName[i].Equals("260$a") && checkInput)
                        {
                            valueTmp = "$a" + valueTmp;
                        }
                        if (listFieldName[i].Equals("260$b") && checkInput)
                        {
                            valueTmp = ":$b" + valueTmp;
                        }
                        if (listFieldName[i].Equals("260$c") && checkInput)
                        {
                            valueTmp = ",$c" + valueTmp;
                        }
                        flag260 = true;
                        outputValue260 = outputValue260 + valueTmp;
                        break;
                    case "300":
                        if (listFieldName[i].Equals("300$a") && checkInput)
                        {
                            valueTmp = "$a" + listFieldValue[i];
                        }
                        if (listFieldName[i].Equals("300$b") && checkInput)
                        {
                            valueTmp = ":$b" + listFieldValue[i];
                        }
                        if (listFieldName[i].Equals("300$c") && checkInput)
                        {
                            valueTmp = ";$c" + listFieldValue[i];
                        }
                        if (listFieldName[i].Equals("300$e") && checkInput)
                        {
                            valueTmp = "+$e" + listFieldValue[i];
                        }
                        flag300 = true;
                        outputValue300 = outputValue300 + valueTmp;
                        break;
                    default:
                        listFieldValue[i] = "$a" + listFieldValue[i];
                        listFieldName[i] = listFieldName[i].Substring(0, 3);
                        //Add List chinh thuc
                        listFieldNameOutput.Add(listFieldName[i]);
                        listFieldValueOutput.Add(listFieldValue[i]);
                        break;
                }


            }

            if (flag090 && !String.IsNullOrEmpty(outputValue090))
            {
                listFieldValueOutput.Add(outputValue090);
                listFieldNameOutput.Add("090");
                flag090 = false;
            }

            if (flag245 && !String.IsNullOrEmpty(outputValue245))
            {
                listFieldValueOutput.Add(outputValue245);
                listFieldNameOutput.Add("245");
                flag245 = false;
            }

            if (flag300 && !String.IsNullOrEmpty(outputValue300))
            {
                listFieldValueOutput.Add(outputValue300);
                listFieldNameOutput.Add("300");
                flag300 = false;
            }

            if (flag260 && !String.IsNullOrEmpty(outputValue260))
            {
                listFieldValueOutput.Add(outputValue260);
                listFieldNameOutput.Add("260");
                flag260 = false;
            }


            UpdateBlockField(listFieldNameOutput, listFieldValueOutput, ItemID, 1);



            ///////////////////////////////////////////////// UPDATE CÁC BẢNG FIELDS/////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

        public void UpdateBlockField(List<string> listFieldNameOutput, List<string> listFieldValueOutput, string ItemID, int action)
        {
            //List have Field Deleted ( su dung trong d)
            List<string> listDeleted = new List<string>();
            for (int i = 0; i < listFieldNameOutput.Count; i++)
            {
                string fieldName = "FIELD" + listFieldNameOutput[i].Substring(0, 1) + "00s";
                if (action == 0 && listDeleted.IndexOf(listFieldNameOutput[i]) == -1)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM " + fieldName + " WHERE ItemID = " + ItemID + " AND FieldCode = '" + listFieldNameOutput[i] + "'");
                    listDeleted.Add(listFieldNameOutput[i]);
                }
                if (listFieldNameOutput[i] == "100")
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + fieldName + " (ItemID, FieldCode, Content, Ind1, Ind2) VALUES (" + ItemID + ",'" + listFieldNameOutput[i] + "',N'" + "" + listFieldValueOutput[i] + "','" + "1" + "','" + "'" + ")");
                }
                else
                if (listFieldNameOutput[i] == "245")
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + fieldName + " (ItemID, FieldCode, Content, Ind1, Ind2) VALUES (" + ItemID + ",'" + listFieldNameOutput[i] + "',N'" + "" + listFieldValueOutput[i] + "','" + "0" + "','" + "0'" + ")");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + fieldName + " (ItemID, FieldCode, Content, Ind1, Ind2) VALUES (" + ItemID + ",'" + listFieldNameOutput[i] + "',N'" + "" + listFieldValueOutput[i] + "','" + "" + "','" + "'" + ")");
                }


            }
        }

        //Update Years
        public void UpdateYear(string ItemID, string fieldValue, string fieldCode)
        {
            db.Database.ExecuteSqlCommand("INSERT INTO CAT_DIC_YEAR (ItemID, FieldCode, YEAR) VALUES ( " + ItemID + ",'" + fieldCode + "' ,'" + fieldValue + "')");
        }

        //Update CAT_DIC_NUMBER
        public void UpdateStandardNumber(string ItemID, string fieldValue, string fieldCode)
        {
            db.Database.ExecuteSqlCommand("INSERT INTO CAT_DIC_NUMBER (ItemID, FieldCode, Number) VALUES ( " + ItemID + ",'" + fieldCode + "','" + fieldValue + "')");
        }

        //Field 041$a || 008
        public void UpdateLanguage(string ItemID, string fieldValue, string fieldCode)
        {
            //Sử dụng cho các bảng CAT_DIC_LANGUAGE , CAT_DIC_COUNTRY
            string ID = PickReferenceID("CAT_DIC_LANGUAGE", fieldValue);
            //Insert Thông tin tương của trường theo ItemID tương ứng vào bảng INDEX tương ứng theo bảng CAT_DIC
            db.Database.ExecuteSqlCommand("INSERT INTO ITEM_LANGUAGE VALUES ( " + ItemID + "," + ID + ",'" + fieldCode + "')");
        }

        //Field 245$a & 245$b 
        public void UpdateItemTitle(string ItemID, string fieldValue, string fieldCode)
        {
            //Insert Thông tin tương của trường theo ItemID tương ứng vào bảng INDEX tương ứng theo bảng CAT_DIC
            db.Database.ExecuteSqlCommand("INSERT INTO ITEM_TITLE (ItemID, FieldCode, Title) VALUES ( " + ItemID + ",'" + fieldCode + "' ,N'" + fieldValue.ToUpper() + "')");
        }

        //Update ITEM_FullText
        public void UpdateItemFulltext(string ItemID, string fieldValue)
        {
            //Insert Thông tin tương của trường theo ItemID tương ứng vào bảng INDEX tương ứng theo bảng CAT_DIC
            fieldValue = fieldValue.Replace("'", "''");
            db.Database.ExecuteSqlCommand("INSERT INTO ITEM_FULLTEXT (ItemID, Contents) VALUES ( " + ItemID + ",N'" + fieldValue.ToUpper() + "')");
        }


        public void UpdateReference(string ItemID, string fieldValue, string fieldCode, int? DicID, int action, bool delete)
        {
            CAT_DIC_LIST DICInf = db.CAT_DIC_LIST.Where(i => i.ID == DicID).FirstOrDefault();

            //UPDATE
            if (action == 0)
            {
                if (delete && DICInf.IndexTable != null)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM " + DICInf.IndexTable + " WHERE ItemID = " + ItemID + " AND  FieldCode = '" + fieldCode + "'");
                }
                //Loc cac ki tu noi cua cac truong 300 & 260
                // , : ; +
                if (fieldCode.StartsWith("300") || fieldCode.StartsWith("260"))
                {
                    if (fieldValue.LastIndexOf(",") == fieldValue.Length - 1 || fieldValue.LastIndexOf(":") == fieldValue.Length - 1 || fieldValue.LastIndexOf(";") == fieldValue.Length - 1 || fieldValue.LastIndexOf("+") == fieldValue.Length - 1)
                    {
                        fieldValue = fieldValue.Remove(fieldValue.Length - 1, 1);
                    }
                }
            }

            int ID = db.Database.SqlQuery<int>("SELECT ID FROM " + DICInf.DicTable + " WHERE AccessEntry = N'" + fieldValue.ToUpper() + "' ").FirstOrDefault();
            //Trường hợp Field Chưa có trong DB => Insert thêm vào Cat_DIC tương ứng
            if (ID == 0)
            {
                //get next max Index in Table
                ID = db.Database.SqlQuery<int>("SELECT ISNULL(Max(ID), 0) + 1 as ID FROM " + DICInf.DicTable).FirstOrDefault();
                //Bo xung New Value vao bang CAT_DIC
                if (DICInf.DicTable == "HOLDING_LIBRARY")
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + DICInf.DicTable + " (ID ,AccessEntry, Code)  VALUES ( " + ID + ",N'" + fieldValue.ToUpper() + "',N'" + fieldValue + "')");
                }
                else if (DICInf.DicTable.StartsWith("DICTIONARY"))
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + DICInf.DicTable + " (ID ,AccessEntry, Dictionary)  VALUES ( " + ID + ",N'" + fieldValue.ToUpper() + "',N'" + fieldValue + "')");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + DICInf.DicTable + " (ID ,AccessEntry, DisplayEntry)  VALUES ( " + ID + ",N'" + fieldValue.ToUpper() + "',N'" + fieldValue + "')");
                }
                //db.Database.ExecuteSqlCommand("INSERT INTO " + DICInf.DicTable + " (ID ,AccessEntry, " + DICInf.CaptionField + "  )  VALUES ( " + ID + ",N'" + fieldValue.ToUpper() + "',N'" + fieldValue + "')");
            }

            //CHECK UPDATE

            //Insert Thông tin tương của trường theo ItemID tương ứng vào bảng INDEX tương ứng theo bảng CAT_DIC
            if (DICInf.IndexTable != null)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO " + DICInf.IndexTable + "(ItemID ," + DICInf.IndexIDField + ", FieldCode ) VALUES ( " + ItemID + "," + ID + ",'" + fieldCode + "')");
            }

        }

        //get ID of Values Of Reference Fields
        public string PickReferenceID(string tbDicName, string value)
        {
            int ID = db.Database.SqlQuery<int>("SELECT ID FROM " + tbDicName + " WHERE AccessEntry = N'" + value.ToUpper() + "' ").FirstOrDefault();
            //Trường hợp Field Chưa có trong DB => Insert thêm vào Cat_DIC tương ứng
            if (ID == 0)
            {
                //get next Index in Table
                ID = db.Database.SqlQuery<int>("SELECT ISNULL(Max(ID), 0) + 1 as ID FROM " + tbDicName).FirstOrDefault();
                if (tbDicName == "HOLDING_LIBRARY")
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + tbDicName + " (ID ,AccessEntry, Code)  VALUES ( " + ID + ",N'" + value.ToUpper() + "',N'" + value + "')");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO " + tbDicName + " (ID ,AccessEntry, DisplayEntry)  VALUES ( " + ID + ",N'" + value.ToUpper() + "',N'" + value + "')");
                }
            }

            return ID.ToString();
        }


        /// <summary>
        /// DELETE Catalogue///////////////////////////////////////////////////********************
        /// </summary>

        public List<SP_GET_TITLES_Result> SearchAllDeleteable()
        {
            List<Nullable<int>> listCode = db.FPT_SELECTALLDELETEABLE().ToList();
            string arrayID = String.Join(",", listCode.ToArray());
            List<SP_GET_TITLES_Result> FinalList = new ShelfBusiness().FPT_SP_GET_TITLES(arrayID);
            foreach (SP_GET_TITLES_Result item in FinalList)
            {
                item.Title = new SupportClass.FormatHoldingTitle().OnFormatHoldingTitle(item.Title);
            }
            return FinalList;
        }

        public List<SP_GET_TITLES_Result> SearchCodeDeleteable(string strCode, string strTT, string strISBN)
        {
            List<SP_GET_TITLES_Result> finalList = null;
            if (strCode == "" && strTT == "" && strISBN == "")
            {
                return SearchAllDeleteable();
            }
            else
            {
                //get List search by Condition
                List<int> ItemId = SearchIDByCondition(strCode, "", strTT, strISBN);
                //Get list all item ready to delete
                List<int> listID = db.Database.SqlQuery<int>("select ID from ITEM where ID not in (select distinct ItemID from HOLDING)").ToList();
                //string finalStrID = String.Join(",", listID.ToArray());

                List<int> listTemp = ItemId.Except(listID).ToList();
                List<int> FinalList = ItemId.Except(listTemp).ToList();
                if (FinalList.Count != 0)
                {
                    string finalStrID = String.Join(",", FinalList.ToArray());
                    finalList = new ShelfBusiness().FPT_SP_GET_TITLES(finalStrID);
                    foreach (SP_GET_TITLES_Result item in finalList)
                    {
                        item.Title = new SupportClass.FormatHoldingTitle().OnFormatHoldingTitle(item.Title);
                    }
                    return finalList;
                }
                else
                {
                    return finalList;
                }

            }

        }

        public string DeleteCatalogue(List<string> ItemCodes)
        {
            List<int> ItemIDs = new List<int>();
            int rs = 0;
            //Get ItemID by ItemCode
            foreach (string itemCode in ItemCodes)
            {
                int ItemID = db.ITEMs.Where(code => code.Code == itemCode).Select(i => i.ID).FirstOrDefault();
                ItemIDs.Add(ItemID);
            }
            //string x = "'" + String.Join("','", ItemIDs) + "'";
            foreach (int i in ItemIDs)
            {
                rs = db.SP_CATA_DELETE_ITEMS(0, i.ToString());
            }

            if (rs > 0)
            {
                return "Xóa thành công " + ItemCodes.Count + " mã tài liệu";
            }
            else
            {
                return "Xảy ra lỗi !Xóa mã tài liệu không thành công ";
            }
        }

    }
}