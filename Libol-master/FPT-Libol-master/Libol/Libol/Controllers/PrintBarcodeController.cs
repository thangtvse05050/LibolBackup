using Libol.EntityResult;
using Libol.Models;
using Libol.SupportClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;

namespace Libol.Controllers
{
    public class PrintBarcodeController : Controller
    {
        private LibolEntities db = new LibolEntities();
        ShelfBusiness shelfBusiness = new ShelfBusiness();

        [AuthAttribute(ModuleID = 4, RightID = "30")]
        public ActionResult Index()
        {
            ViewBag.Library = shelfBusiness.FPT_SP_HOLDING_LIBRARY_SELECT(0, 1, -1, (int)Session["UserID"], 1);
            ViewBag.Template = db.SP_SYS_GET_TEMPLATE(0, 79).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult OnchangeLibrary(int LibID)
        {
            List<SP_HOLDING_LOCATION_GET_INFO_Result> list = shelfBusiness.FPT_SP_HOLDING_LOCATION_GET_INFO(LibID, (int)Session["UserID"], 0, -1);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AuthAttribute(ModuleID = 4, RightID = "0")]
        public ActionResult Print(int intSelMode,
            int intLibID,
            int intLocID,
            string strFromItemCode,
            string strToItemCode,
            string strFromCopyNumber,
            string strToCopyNumber,
            string strElse,
            int ddlType,
            int txtHeight,
            int txtWidth,
            int ddlImageType,
            int ddlRotation,
            int txtRowSpace,
            int txtColSpace,
            int txtColImageNumber,
            int txtPageImageNumber,
            int TemplateID,
            int Page)
        {
            if(Page < 1)
            {
                Page = 1;
            }
            var selectCopyNumber = db.HOLDINGs.Where(a => true);
            switch (intSelMode)
            {
                case 0:
                    selectCopyNumber = selectCopyNumber
                        .Where(a => db.ITEMs.Where(i => i.Code.CompareTo(strFromItemCode) >= 0 && i.Code.CompareTo(strToItemCode) <= 0).Select(i => i.ID).Contains(a.ItemID));
                    break;
                case 1:
                    selectCopyNumber = selectCopyNumber.Where(a => string.Compare(a.CopyNumber, strFromCopyNumber) >= 0 &&
                                                                   string.Compare(a.CopyNumber, strToCopyNumber) <= 0);
                    if (intLibID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LibID == intLibID);
                    }
                    if (intLocID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LocationID == intLocID);
                    }
                    
                    break;
                case 2:
                    selectCopyNumber = selectCopyNumber.Where(a => strElse.Contains(a.CopyNumber));
                    if (intLibID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LibID == intLibID);
                    }
                    if (intLocID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LocationID == intLocID);
                    }
                    
                    break;
            }

            double TotalItem = selectCopyNumber.Count();
            double TotalItemPerPage = txtPageImageNumber;
            double TotalPage = Math.Ceiling(TotalItem / TotalItemPerPage);
            ViewBag.CurrentPage = Page;
            ViewBag.TotalPage = TotalPage;
            var CurrentItemInPage = selectCopyNumber.OrderBy(a => a.ItemID).Skip((int)TotalItemPerPage * (Page - 1)).Take((int)TotalItemPerPage).ToArray();
            string Data = "<table>";
            int Count = 0;
            for(int i = 0; i < Math.Ceiling((double)txtPageImageNumber/txtColImageNumber); i++)
            {
                Data += "<tr>";
                for (int j = 0; j < txtColImageNumber; j++)
                {                    
                    if (Count < CurrentItemInPage.Length)
                    {
                        var writer = new BarcodeWriter
                        {
                            Format = BarcodeFormat.CODE_39,
                            Options = new EncodingOptions
                            {
                                Height = 60,
                                Width = 300
                            }
                        };

                        var bitmap = writer.Write(CurrentItemInPage[Count].CopyNumber);
                        MemoryStream ms = new MemoryStream();
                        bitmap.Save(ms, ImageFormat.Bmp);
                        byte[] byteImage = ms.ToArray();
                        var SigBase64 = Convert.ToBase64String(byteImage);
                        Data += "<td>";
                        Data += "<img src='data:image/png;base64,"+ SigBase64 + "'/>";
                        Data += "</td>";                        
                        Count++;
                    }                    
                }
                Data += "</tr>";
            }
            Data += "</table>";
            ViewBag.Data = Data;
            return View();
        }

        [AuthAttribute(ModuleID = 4, RightID = "0")]
        public FileStreamResult PrintBarcode(int intSelMode,
            int intLibID,
            int intLocID,
            string strFromItemCode,
            string strToItemCode,
            string strFromCopyNumber,
            string strToCopyNumber,
            string strElse,
            int ddlType,
            int txtHeight,
            int txtWidth,
            int ddlImageType,
            int ddlRotation,
            int txtRowSpace,
            int txtColSpace,
            int txtColImageNumber,
            int txtPageImageNumber,
            int TemplateID,
            int Page)
        {
            var selectCopyNumber = db.HOLDINGs.Where(a => true);
            switch (intSelMode)
            {
                case 0:
                    selectCopyNumber = selectCopyNumber
                        .Where(a => db.ITEMs.Where(i => i.Code.CompareTo(strFromItemCode) >= 0 && i.Code.CompareTo(strToItemCode) <= 0).Select(i => i.ID).Contains(a.ItemID));
                    break;
                case 1:
                    selectCopyNumber = selectCopyNumber.Where(a => string.Compare(a.CopyNumber, strFromCopyNumber) >= 0 &&
                                                                   string.Compare(a.CopyNumber, strToCopyNumber) <= 0);
                    if (intLibID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LibID == intLibID);
                    }
                    if (intLocID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LocationID == intLocID);
                    }

                    break;
                case 2:
                    selectCopyNumber = selectCopyNumber.Where(a => strElse.Contains(a.CopyNumber));
                    if (intLibID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LibID == intLibID);
                    }
                    if (intLocID > 0)
                    {
                        selectCopyNumber = selectCopyNumber.Where(a => a.LocationID == intLocID);
                    }

                    break;
            }

            string Data = "";
            var Items = selectCopyNumber.OrderBy(a => a.ItemID).ToArray();
            string Template = db.SYS_TEMPLATE.Where(a => a.ID == TemplateID).First().Content;
            for(int i = 0; i < Math.Ceiling((double)Items.Length / 2); i++)
            {
                if(i*2 + 1== Items.Length)
                {
                    Data += Template.Replace("<$copynumber1$>", Items[i * 2].CopyNumber).Replace("<$copynumber2$>", Items[i * 2].CopyNumber)
                        .Replace("<$COPYNUMBER1$>", Items[i * 2].CopyNumber).Replace("<$COPYNUMBER2$>", Items[i * 2].CopyNumber);
                }
                else
                {
                    Data += Template.Replace("<$copynumber1$>", Items[i * 2].CopyNumber).Replace("<$copynumber2$>", Items[i * 2 + 1].CopyNumber)
                        .Replace("<$COPYNUMBER1$>", Items[i * 2].CopyNumber).Replace("<$COPYNUMBER2$>", Items[i * 2 + 1].CopyNumber);
                }
            }
            
            var string_with_your_data = Data;
            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/plain", "barcode.txt");
        }

    }
}