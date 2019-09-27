using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Libol.Models;
using Libol.EntityResult;
using System.Data.Entity.Core.Objects;
using Libol.SupportClass;
using System.Data;

namespace Libol.Controllers
{
    public class CatalogueController : Controller
    {
        private LibolEntities db = new LibolEntities();
        CatalogueBusiness catalogueBusiness = new CatalogueBusiness();

        [AuthAttribute(ModuleID = 1, RightID = "0")]
        public ActionResult MainTab()
        {
            return View();
        }



        //----------------Add New Cata ----------------
        //---------------------------------------------
        [AuthAttribute(ModuleID = 1, RightID = "13")]
        public ActionResult AddNewCatalogue()
        {
            //get list marc form
            ViewData["ListMarcForm"] = db.FPT_SP_CATA_GET_MARC_FORM(0, 0).ToList();
            //Cấp thư mục
            ViewData["listLevelDir"] = db.CAT_DIC_DIRLEVEL.OrderBy(d => d.Description).ToList();
            ViewData["ListRecordType"] = db.CAT_DIC_RECORDTYPE.OrderBy(r => r.Description).ToList();
            ViewData["listItemType"] = db.CAT_DIC_ITEM_TYPE.Where(t => !String.IsNullOrEmpty(t.TypeName)).OrderBy(t => t.TypeName).ToList();
            //vật mang tin
            ViewData["listMedium"] = db.CAT_DIC_MEDIUM.Where(m => !String.IsNullOrEmpty(m.Description)).OrderBy(m => m.Description).ToList();

            byte[] listAccessLevel = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            ViewData["listAccessLevel"] = listAccessLevel;

            return View();
        }


        //Chức năng được phát triển theo yêu cầu phát sinh ------------------------------------
        //[HttpPost]
        //public JsonResult UploadFile()
        //{
        //    string rs = "";
        //    HttpFileCollectionBase Files = Request.Files;
        //    int id = Int32.Parse(Request.Form["ID"]) ;

        //    if (Files.Count  != 0)
        //    {
        //        for (int i = 0; i < Files.Count; i++)
        //        {

        //            var file = Files[i];
        //            var fileName = DateTime.Now.ToString("yyMMddHHmmss") + Path.GetFileName(file.FileName);
        //            string path = Path.Combine(Server.MapPath("~/CAT_FILE"),Path.GetFileName(fileName));
        //            file.SaveAs(path);
        //            int indexi = i + 1;
        //            //save DB
        //            db.FPT_CATA_FILE_NEW.Add(new FPT_CATA_FILE_NEW {  ItemID = id, FileName= file.FileName, FilePath= path });
        //            db.SaveChanges();

        //            rs = "Upload Thành Công " + indexi + " File !";
        //        }
        //    }
        //    else
        //    {
        //        rs = "Chưa có File được chọn !";
        //    }

        //    return Json(rs, JsonRequestBehavior.AllowGet);

        //}
        //[HttpGet]
        //public FileResult Download(string FileName , string FilePath )
        //{
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(FilePath);
        //    //string fileName = "190814100030SBV_SG3.1_Tai lieu mo ta source code Web Application-SWIFT.docx";
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet , FileName);
        //}


        //**************************************************************CHECK TITLE**************************************************************
        [HttpPost]
        public JsonResult CheckTitle(string strTitle, string strItemType)
        {
            //catalogueBusiness.CheckExistNumber("9781184", "020$a");
            //string fieldCode = GetFieldByID(intIsAuthority,"", intFormID);
            //strTitle = "N'" + strTitle +"'";
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> titleList = catalogueBusiness.CheckTitle(strTitle);
            return Json(titleList, JsonRequestBehavior.AllowGet);

        }

        ////**************************************************************CHECK ISBN**************************************************************
        [HttpPost]
        public JsonResult CheckItemNumber(string strFieldValue, string strFieldCode)
        {
            ObjectParameter Output = new ObjectParameter("lngItemID", typeof(Int32));
            db.FPT_SP_CATA_CHECK_EXIST_ITEMNUMBER(strFieldValue, strFieldCode, Output);
            return Json(Output.Value, JsonRequestBehavior.AllowGet);
            //return Json("", JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult LoadFormComplated(int intIsAuthority, int intFormID)
        {
            //catalogueBusiness.CheckExistNumber("9781184", "020$a");
            //string fieldCode = GetFieldByID(intIsAuthority,"", intFormID);
            List<GET_CATALOGUE_FIELDS_Result> formComplated = catalogueBusiness.GetComplatedForm(0, "", intFormID);
            ViewData["MarcFormComplated"] = formComplated;

            return Json(formComplated, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItemInf(string itemID)
        {
            int ID = Int32.Parse(itemID);
            int FormID = db.ITEMs.First(i => i.ID == ID).FormID;
            return Json(FormID, JsonRequestBehavior.AllowGet);
        }

        //Get Content of Item by ID to reuse
        [HttpPost]
        public JsonResult ReUseGetContentByID(string itemID)
        {
            List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> listContent = catalogueBusiness.GetContentByID(itemID);
            return Json(listContent, JsonRequestBehavior.AllowGet);
        }



        //----------------Add Item For Detail <Biên Mục Chi Tiết> ---------------------
        //-----------------------------------------------------------------------------

        [HttpPost]
        public JsonResult InsertOrUpdateCatalogue(List<string> listFieldsName, List<string> listFieldsValue, List<string> listFieldsOrg, List<string> listValuesOrg)
        {
            string ItemID = catalogueBusiness.UpdateItem(listFieldsName, listFieldsValue, listFieldsOrg, listValuesOrg);
            int tempCode = Int32.Parse(ItemID);
            string ItemCode = db.ITEMs.Where(i => i.ID == tempCode).Select(i => i.Code).FirstOrDefault().ToString();
            string[] data = { ItemCode, ItemID };
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        //----------------Search Field Cata ----------------------------------------
        //--------------------------------------------------------------------------
        [AuthAttribute(ModuleID = 1, RightID = "15")]
        public ActionResult SearchCodeNumber()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SearchCode(string strCode, string strCN, string strTT)
        {
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> inforList = catalogueBusiness.SearchCode(strCode, strCN, strTT);
            return Json(inforList, JsonRequestBehavior.AllowGet);
        }





        //----------------Detail Cata -----------
        //---------------------------------------------
        [AuthAttribute(ModuleID = 1, RightID = "15")]
        public ActionResult AddNewCatalogueDetail()
        {
            string Id = Request["ID"];
            string strFieldCode = "";
            if (!String.IsNullOrEmpty(Id))
            {
                List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> listContent = catalogueBusiness.GetContentByID(Id).ToList();
                if (listContent.Count == 0) return View();
                //Lay Content cua LEADERty
                ViewData["Leader"] = listContent[0];
                listContent.RemoveAt(0);


                //****************************************************Done List Content****************************************************
                //*************************************************************************************************************************
                ViewData["ListContent"] = listContent;

                //get mô tả từng trường
                foreach (FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result item in listContent)
                {
                    strFieldCode = strFieldCode + item.IDSort + ",";
                }

                List<SP_CATA_GET_MODIFIED_FIELDS_Result> listField = catalogueBusiness.FPT_SP_CATA_GET_MODIFIED_FIELDS(0, 0, strFieldCode, "", "", 0).ToList();

                //****************************************************Done List Des****************************************************
                //*************************************************************************************************************************
                ViewData["ListField"] = listField;

                //Load File
                //int IdIn = Int32.Parse(Id);
                //List<FPT_CATA_FILE_NEW> listFile = db.FPT_CATA_FILE_NEW.Where(i => i.ItemID== IdIn).ToList();
                //ViewData["ListFile"] = listFile;
            }
            else
            {
                return RedirectToAction("SearchCodeNumber", "catalogue");
            }

            return View();
        }

        [HttpPost]
        public JsonResult SearchField(string strSearch)
        {
            if (String.IsNullOrEmpty(strSearch))
            {
                return Json(new List<FPT_SP_CATA_SEARCH_MARC_FIELDS_Results>(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<FPT_SP_CATA_SEARCH_MARC_FIELDS_Results> listSearch = catalogueBusiness.FPT_SP_CATA_SEARCH_MARC_FIELDS(strSearch, (-1), 0, "", "");
                return Json(listSearch, JsonRequestBehavior.AllowGet);
            }

        }


        //**************************************************************************
        [AuthAttribute(ModuleID = 1, RightID = "15")]
        public ActionResult ViewCatalogue()
        {
            ViewBag.TotalCount = db.ITEMs.Count();
            //List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> listContent = catalogueBusiness.GetContentByID(Id).ToList();

            return View();
        }


        public JsonResult ViewCataContentByIndex(int? index)
        {


            int actualIndex = (int)index;
            //string ItemID = db.ITEMs.ToList().ElementAt(actualIndex - 1).ID.ToString();
            string ItemID = db.FPT_CATA_GETCONTENT_BY_INDEX(index).FirstOrDefault().ToString();
            List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> listContent = db.FPT_SP_CATA_GET_CONTENTS_OF_ITEMS(ItemID, 0).ToList();
            return Json(listContent, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetItemIDByCode(string ItemCode)
        {
            var ItemID = db.SP_GET_ITEMID_BYCODE(ItemCode).ToList().FirstOrDefault();
            return Json(ItemID, JsonRequestBehavior.AllowGet);
        }


        //////////////////// **** DELETE ITEM **** ////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////


        [AuthAttribute(ModuleID = 1, RightID = "15")]
        public ActionResult DeleteCatalogue()
        {
            //get all Item ready to delete
            ViewData["Deleteable"] = catalogueBusiness.SearchAllDeleteable();
            return View();
        }

        public JsonResult DelCatalogue(List<string> ItemCodes)
        {
            //Get ItemID by ItemCode
            string rs = catalogueBusiness.DeleteCatalogue(ItemCodes);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchDeleteable(string strCode, string strTT, string strISBN)
        {
            List<SP_GET_TITLES_Result> listContent = catalogueBusiness.SearchCodeDeleteable(strCode, strTT, strISBN);
            return Json(listContent, JsonRequestBehavior.AllowGet);
        }

    }



}
