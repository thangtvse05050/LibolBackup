using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libol.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

using Libol.SupportClass;
using Libol.EntityResult;
using System.Data.Entity;

namespace Libol.Controllers
{
    public class CirculationController : Controller
    {
        LibolEntities le = new LibolEntities();
        CirculationBusiness cb = new CirculationBusiness();
        PatronBusiness pb = new PatronBusiness();
        FormatHoldingTitle format = new FormatHoldingTitle();
        //int UserID = 49;
        public string GetContent(string copynumber)
        {
            string validate = copynumber.Replace("$a", " ");
            validate = validate.Replace("$b", " ");
            validate = validate.Replace("$c", " ");
            validate = validate.Replace("$n", " ");
            validate = validate.Replace("$p", " ");
            validate = validate.Replace("$e", " ");

            return validate.Trim();
        }
        [AuthAttribute(ModuleID = 3, RightID = "0")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        [AuthAttribute(ModuleID = 3, RightID = "21")]
        public ActionResult ReportOnLoanCopy()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var l in le.FPT_SP_CIR_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
            }
            ViewData["lib"] = lib;
            return View();
        }
        [HttpPost]
        public PartialViewResult GetOnLoanStats()
        {
            return PartialView("GetOnLoanStats");
        }

        [HttpPost]
        public JsonResult GetPatronOnLoanInfo(DataTableAjaxPostModel model, string strLibID, string strLocPrefix, string strLocID, string strPatronNumber, string strItemCode, string strDueDateFrom, string strDueDateTo, string strCheckOutDateFrom, string strCheckOutDateTo, string strCopyNumber)
        {
            int LibID = 0;
            if (!string.IsNullOrEmpty(strLibID)) LibID = Convert.ToInt32(strLibID);
            var patronLoanInfors = cb.GET_PATRON_ONLOAN_INFOR_LIST(strPatronNumber, strItemCode, strCopyNumber, LibID, strLocPrefix, strLocID, strCheckOutDateFrom, strCheckOutDateTo, strDueDateFrom, strDueDateTo, null, (int)Session["UserID"]);
            var search = patronLoanInfors.Where(a => true);
            if (model.search.value != null)
            {
                string searchValue = model.search.value;
                search = search.Where(a => (format.OnFormatHoldingTitle(a.Content) ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.CopyNumber ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.FullName ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || ((a.Price == 0) ? 0 : a.Price).ToString().ToUpper().Contains(searchValue.ToUpper())
                    || (a.Currency ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || a.CheckOutDate.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.DueDate.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                );
            }
            var sorting = search.OrderBy(a => false);
            if (model.order[0].column == 0)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Content);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Content);
                }
            }
            else if (model.order[0].column == 1)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CopyNumber);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CopyNumber);
                }
            }
            else if (model.order[0].column == 2)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.FullName);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.FullName);
                }
            }
            else if (model.order[0].column == 3)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CheckOutDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CheckOutDate);
                }
            }
            else if (model.order[0].column == 4)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.DueDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.DueDate);
                }
            }
            else if (model.order[0].column == 5)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Price);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Price);
                }
            }
            var paging = sorting.Skip(model.start).Take(model.length).ToList();
            List<GET_PATRON_ONLOANINFOR_Result_2> result = new List<GET_PATRON_ONLOANINFOR_Result_2>();
            foreach (var i in paging)
            {
                result.Add(new GET_PATRON_ONLOANINFOR_Result_2()
                {
                    Content = format.OnFormatHoldingTitle(i.Content),
                    CopyNumber = i.CopyNumber,
                    CheckOutDate = i.CheckOutDate == null ? "" : i.CheckOutDate.Value.ToString("dd/MM/yyyy"),
                    DueDate = i.DueDate == null ? "" : i.DueDate.Value.ToString("dd/MM/yyyy"),
                    RenewCount = i.RenewCount,
                    Serial = i.Serial,
                    FullName = i.FullName,
                    Price = i.Price.ToString() + " " + i.Currency,
                    Currency = i.Currency
                });
            }
            return Json(new
            {
                draw = model.draw,
                recordsTotal = patronLoanInfors.Count(),
                recordsFiltered = search.Count(),
                patronCount = search.Select(a => a.FullName).Distinct().Count(),
                loanCount = search.Count(),
                data = result
            });
        }

        [HttpPost]
        public PartialViewResult GetFilteredOnLoanStats()
        {
            return PartialView("GetFilteredOnLoanStats");
        }

        [HttpPost]
        public JsonResult GetPatronRenewOnLoanInfo(DataTableAjaxPostModel model, string strLibID, string strLocPrefix, string strLocID, string strPatronNumber, string strItemCode, string strCheckInDateFrom, string strCheckInDateTo, string strCheckOutDateFrom, string strCheckOutDateTo, string strCopyNumber)
        {
            int LibID = 0;
            if (!string.IsNullOrEmpty(strLibID)) LibID = Convert.ToInt32(strLibID);
            var patronLoanInfors = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST(strPatronNumber, strItemCode, strCopyNumber, LibID, strLocPrefix, strLocID, strCheckOutDateFrom, strCheckOutDateTo, strCheckInDateFrom, strCheckInDateTo, (int)Session["UserID"]);
            var search = patronLoanInfors.Where(a => true);
            if (model.search.value != null)
            {
                string searchValue = model.search.value;
                search = search.Where(a => (format.OnFormatHoldingTitle(a.Content) ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.CopyNumber ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.FullName ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || ((a.Price == 0) ? 0 : a.Price).ToString().ToUpper().Contains(searchValue.ToUpper())
                    || (a.Currency ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || a.CheckOutDate.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.RenewDate.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.OverDueDateNew.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.OverDueDateOld.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                );
            }
            var sorting = search.OrderBy(a => false);
            if (model.order[0].column == 0)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Content);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Content);
                }
            }
            else if (model.order[0].column == 1)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CopyNumber);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CopyNumber);
                }
            }
            else if (model.order[0].column == 2)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.FullName);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.FullName);
                }
            }
            else if (model.order[0].column == 3)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CheckOutDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CheckOutDate);
                }
            }
            else if (model.order[0].column == 4)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.OverDueDateOld);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.OverDueDateOld);
                }
            }
            else if (model.order[0].column == 5)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.OverDueDateNew);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.OverDueDateNew);
                }
            }
            else if (model.order[0].column == 6)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => false);
                }
                else
                {
                    sorting = search.OrderByDescending(a => false);
                }
            }
            else if (model.order[0].column == 7)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.RenewDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.RenewDate);
                }
            }
            else if (model.order[0].column == 8)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Price);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Price);
                }
            }
            var paging = sorting.Skip(model.start).Take(model.length).ToList();
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result_2> result = new List<GET_PATRON_RENEW_ONLOAN_INFOR_Result_2>();
            foreach (var i in paging)
            {
                result.Add(new GET_PATRON_RENEW_ONLOAN_INFOR_Result_2()
                {
                    Content = format.OnFormatHoldingTitle(i.Content),
                    CopyNumber = i.CopyNumber,
                    CheckOutDate = i.CheckOutDate.Value.ToString("dd/MM/yyyy"),
                    DueDate = i.DueDate.Value.ToString("dd/MM/yyyy"),
                    FullName = i.FullName,
                    RenewDate = i.RenewDate.Value.ToString("dd/MM/yyyy"),
                    OverDueDateNew = i.OverDueDateNew.Value.ToString("dd/MM/yyyy"),
                    OverDueDateOld = i.OverDueDateOld.Value.ToString("dd/MM/yyyy"),
                    //CheckInDate = "",
					OverDueDate = (i.RenewDate.Value - i.OverDueDateOld.Value).Days > 0 ? (i.RenewDate.Value - i.OverDueDateOld.Value).Days.ToString() : "",
                    Price = i.Price.ToString() + " " + i.Currency,
					FeeOverDueDate=(i.RenewDate.Value - i.OverDueDateOld.Value).Days > 0 ? ((i.RenewDate.Value - i.OverDueDateOld.Value).Days * 5).ToString()+"000 "+i.Currency:"",

					Currency = i.Currency
                });
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = patronLoanInfors.Count(),
                recordsFiltered = search.Count(),
                patronCount = search.Select(a => a.FullName).Distinct().Count(),
                loanCount = search.Count(),
                data = result
            });
        }
        //-------------------END OF ONLOAN REPORT---------------------
        [AuthAttribute(ModuleID = 3, RightID = "21")]
        public ActionResult ReportLoanCopy()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var l in le.FPT_SP_CIR_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
            }
            ViewData["lib"] = lib;
            return View();
        }

        //GET LOCATIONS PREFIX BY LIBRARY
        public JsonResult GetLocationsPrefix(string id)
        {
            List<SelectListItem> LocPrefix = new List<SelectListItem>();
            LocPrefix.Add(new SelectListItem { Text = "Tất cả", Value = "0" });
            if (!string.IsNullOrEmpty(id))
            {
                foreach (var lp in le.FPT_CIR_GET_LOCLIBUSER_PREFIX_SEL((int)Session["UserID"], Int32.Parse(id)))
                {
                    LocPrefix.Add(new SelectListItem { Text = Regex.Replace(lp.ToString(), @"[^0-9a-zA-Z]+", ""), Value = lp.ToString() });
                }
            }
            return Json(new SelectList(LocPrefix, "Value", "Text"));
        }

        //GET LOCATIONS BY LOCATION PREFIX, LIBRARY, USERID
        public JsonResult GetLocationsByPrefix(int id, string prefix)
        {
            List<SelectListItem> LocByPrefix = new List<SelectListItem>();
            LocByPrefix.Add(new SelectListItem { Text = "Tất cả", Value = "" });

            foreach (var lbp in le.FPT_CIR_GET_LOCFULLNAME_LIBUSER_SEL((int)Session["UserID"], id, prefix))
            {
                LocByPrefix.Add(new SelectListItem { Text = lbp.Symbol, Value = lbp.ID.ToString() });
            }
            return Json(new SelectList(LocByPrefix, "Value", "Text"));
        }

        [HttpPost]
        public PartialViewResult GetLoanStats()
        {
            return PartialView("GetLoanStats");
        }
        [HttpPost]
        public JsonResult GetPatronLoanInfo(DataTableAjaxPostModel model, string strLibID, string strLocPrefix, string strLocID, string strPatronNumber, string strItemCode, string strCheckInDateFrom, string strCheckInDateTo, string strCheckOutDateFrom, string strCheckOutDateTo, string strCopyNumber)
        {
            int LibID = 0;
            if (!string.IsNullOrEmpty(strLibID)) LibID = Convert.ToInt32(strLibID);
            var patronLoanInfors = cb.GET_PATRON_LOAN_INFOR_LIST(strPatronNumber, strItemCode, strCopyNumber, LibID, strLocPrefix, strLocID, strCheckOutDateFrom, strCheckOutDateTo, strCheckInDateFrom, strCheckInDateTo, null, (int)Session["UserID"]);
            var search = patronLoanInfors.Where(a => true);
            if (model.search.value != null)
            {
                string searchValue = model.search.value;
                search = search.Where(a => (format.OnFormatHoldingTitle(a.Content) ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.CopyNumber ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.FullName ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || ((a.Price == 0) ? 0 : a.Price).ToString().ToUpper().Contains(searchValue.ToUpper())
                    || (a.Currency ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || a.CheckOutDate.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.CheckInDate.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.OverdueDays.Value.ToString().Contains(searchValue)
                    || a.OverdueFine.ToString().Contains(searchValue)
                );
            }
            var sorting = search.OrderBy(a => false);
            if (model.order[0].column == 0)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Content);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Content);
                }
            }
            else if (model.order[0].column == 1)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CopyNumber);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CopyNumber);
                }
            }
            else if (model.order[0].column == 2)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.FullName);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.FullName);
                }
            }
            else if (model.order[0].column == 3)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CheckOutDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CheckOutDate);
                }
            }
            else if (model.order[0].column == 4)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CheckInDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CheckInDate);
                }
            }
            else if (model.order[0].column == 5)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.OverdueDays);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.OverdueDays);
                }
            }
            else if (model.order[0].column == 6)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.OverdueFine);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.OverdueFine);
                }
            }
            else if (model.order[0].column == 7)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Price);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Price);
                }
            }
            var paging = sorting.Skip(model.start).Take(model.length).ToList();
            List<GET_PATRON_LOANINFOR_Result_2> result = new List<GET_PATRON_LOANINFOR_Result_2>();
            foreach (var i in paging)
            {
                result.Add(new GET_PATRON_LOANINFOR_Result_2()
                {
                    Content = format.OnFormatHoldingTitle(i.Content),
                    CopyNumber = i.CopyNumber,
                    CheckOutDate = i.CheckOutDate.Value.ToString("dd/MM/yyyy"),
                    CheckInDate = i.CheckInDate.Value.ToString("dd/MM/yyyy"),
                    RenewCount = i.RenewCount,
                    Serial = i.Serial,
                    FullName = i.FullName,
                    OverdueDays = i.OverdueDays,
                    OverdueFine = i.OverdueFine,
                    Price = i.Price.ToString() + " " + i.Currency,
                    Currency = i.Currency
                });
            }
            return Json(new
            {
                draw = model.draw,
                recordsTotal = patronLoanInfors.Count(),
                recordsFiltered = search.Count(),
                patronCount = search.Select(a => a.FullName).Distinct().Count(),
                loanCount = search.Count(),
                data = result
            });
        }


        [HttpPost]
        public PartialViewResult GetFilteredLoanStats()
        {
            return PartialView("GetFilteredLoanStats");
        }

        [HttpPost]
        public JsonResult GetPatronRenewLoanInfo(DataTableAjaxPostModel model, string strLibID, string strLocPrefix, string strLocID, string strPatronNumber, string strItemCode, string strCheckInDateFrom, string strCheckInDateTo, string strCheckOutDateFrom, string strCheckOutDateTo, string strCopyNumber)
        {
            int LibID = 0;
            if (!string.IsNullOrEmpty(strLibID)) LibID = Convert.ToInt32(strLibID);
            var patronLoanInfors = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST(strPatronNumber, strItemCode, strCopyNumber, LibID, strLocPrefix, strLocID, strCheckOutDateFrom, strCheckOutDateTo, strCheckInDateFrom, strCheckInDateTo, (int)Session["UserID"]);
            foreach (var i in patronLoanInfors)
            {
                if ((i.CheckInDate - i.OverDueDateNew).Days > i.OverdueDays)
                {
                    i.OverdueDays = 0;
                    i.OverdueFine = 0;
                }
            }
            var search = patronLoanInfors.Where(a => true);
            if (model.search.value != null)
            {
                string searchValue = model.search.value;
                search = search.Where(a => (format.OnFormatHoldingTitle(a.Content) ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.CopyNumber ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || (a.FullName ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || ((a.Price == 0) ? 0 : a.Price).ToString().ToUpper().Contains(searchValue.ToUpper())
                    || (a.Currency ?? "").ToUpper().Contains(searchValue.ToUpper())
                    || a.CheckOutDate.Value.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.CheckInDate.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.OverdueDays.Value.ToString().Contains(searchValue)
                    || a.OverdueFine.ToString().Contains(searchValue)
                    || a.OverDueDateNew.ToString("dd/MM/yyyy").Contains(searchValue)
                    || a.RenewDate.ToString("dd/MM/yyyy").Contains(searchValue)
                );
            }
            var sorting = search.OrderBy(a => false);
            if (model.order[0].column == 0)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Content);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Content);
                }
            }
            else if (model.order[0].column == 1)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CopyNumber);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CopyNumber);
                }
            }
            else if (model.order[0].column == 2)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.FullName);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.FullName);
                }
            }
            else if (model.order[0].column == 3)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CheckOutDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CheckOutDate);
                }
            }
            else if (model.order[0].column == 4)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.OverDueDateNew);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.OverDueDateNew);
                }
            }
            else if (model.order[0].column == 5)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.CheckInDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.CheckInDate);
                }
            }
            else if (model.order[0].column == 6)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.RenewDate);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.RenewDate);
                }
            }
            else if (model.order[0].column == 7)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.OverdueDays);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.OverdueDays);
                }
            }
            else if (model.order[0].column == 8)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.OverdueFine);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.OverdueFine);
                }
            }
            else if (model.order[0].column == 9)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Price);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Price);
                }
            }
            var paging = sorting.Skip(model.start).Take(model.length).ToList();
            List<GET_PATRON_RENEW_LOAN_INFOR_Result_2> result = new List<GET_PATRON_RENEW_LOAN_INFOR_Result_2>();

            foreach (var i in paging)
            {
                result.Add(new GET_PATRON_RENEW_LOAN_INFOR_Result_2()
                {
                    Content = format.OnFormatHoldingTitle(i.Content),
                    CopyNumber = i.CopyNumber,
                    CheckOutDate = i.CheckOutDate.Value.ToString("dd/MM/yyyy"),
                    CheckInDate = i.CheckInDate.ToString("dd/MM/yyyy"),
                    FullName = i.FullName,
                    RenewDate = i.RenewDate.ToString("dd/MM/yyyy"),
                    OverDueDateNew = i.OverDueDateNew.ToString("dd/MM/yyyy"),
                    OverDueDateOld = i.OverDueDateOld.ToString("dd/MM/yyyy"),
                    OverdueDays = i.OverdueDays,
                    OverdueFine = i.OverdueFine.ToString("#.##"),
                    Price = i.Price.ToString() + " " + i.Currency,
                    Currency = i.Currency,
					//OverDueDate=i.OverdueFine-i.OverDueDateNew
				});
            }
            return Json(new
            {
                draw = model.draw,
                recordsTotal = patronLoanInfors.Count(),
                recordsFiltered = search.Count(),
                patronCount = search.Select(a => a.FullName).Distinct().Count(),
                loanCount = search.Count(),
                data = result
            });
        }
        //-------------------END OF LOAN HISTORY REPORT---------------------
        public ActionResult StatisticAnnual()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var l in le.FPT_SP_CIR_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
            }
            ViewData["lib"] = lib;
            return View();
        }
        [AuthAttribute(ModuleID = 3, RightID = "23")]
        public ActionResult StatisticYear()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var l in le.FPT_SP_CIR_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
            }
            ViewData["lib"] = lib;
            return View();
        }
        //GET LOCATIONS BY LIBRARY
        public JsonResult GetLocations(string id)
        {
            List<SelectListItem> loc = new List<SelectListItem>();
            loc.Add(new SelectListItem { Text = "Tất cả các kho", Value = "0" });
            if (!string.IsNullOrEmpty(id))
            {
                foreach (var l in le.FPT_SP_CIR_LIBLOCUSER_SEL((int)Session["UserID"], Int32.Parse(id)).ToList())
                {
                    loc.Add(new SelectListItem { Text = l.Symbol, Value = l.ID.ToString() });
                }
            }
            return Json(new SelectList(loc, "Value", "Text"));
        }
        [HttpPost]
        public PartialViewResult GetYearStats(string strLibID, string strLocPrefix, string strLocID, string strFromYear, string strToYear, string strType)
        {
            int LibID = 0;
            
            int Type = 0;
            if (!string.IsNullOrEmpty(strLibID)) LibID = Convert.ToInt32(strLibID);
            //if (!string.IsNullOrEmpty(strLocID)) LocID = Convert.ToInt32(strLocID);
            if (!string.IsNullOrEmpty(strType)) Type = Convert.ToInt32(strType);
            if (Type == 3)
            {
                ViewBag.TypeName = "bạn đọc";
            }
            else if (Type == 2)
            {
                ViewBag.TypeName = "bản ấn phẩm";
            }
            else if (Type == 1)
            {
                ViewBag.TypeName = "đầu ấn phẩm";
            }
            ViewBag.UsedResult = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(LibID, strLocPrefix, strLocID, Type, 0, strFromYear, strToYear, (int)Session["UserID"]);
            ViewBag.UsingResult = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(LibID, strLocPrefix, strLocID, Type, 1, strFromYear, strToYear, (int)Session["UserID"]);
            return PartialView("GetYearStats");
        }
        [HttpPost]
        public PartialViewResult GetMonthStats(string strLibID, string strLocPrefix, string strLocID, string strDateFrom, string strDateTo, string strType)
        {
            int LibID = 0;
            //int LocID = 0;
            int Type = 0;
            if (!string.IsNullOrEmpty(strLibID)) LibID = Convert.ToInt32(strLibID);
            //if (!string.IsNullOrEmpty(strLocID)) LocID = Convert.ToInt32(strLocID);
            if (!string.IsNullOrEmpty(strType)) Type = Convert.ToInt32(strType);
            if (Type == 3)
            {
                ViewBag.TypeName = "bạn đọc";
            }
            else if (Type == 2)
            {
                ViewBag.TypeName = "bản ấn phẩm";
            }
            else if (Type == 1)
            {
                ViewBag.TypeName = "đầu ấn phẩm";
            }
            ViewBag.UsedResult = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(LibID, strLocPrefix, strLocID, Type, 0, strDateFrom, strDateTo, (int)Session["UserID"]);
            ViewBag.UsingResult = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(LibID, strLocPrefix, strLocID, Type, 1, strDateFrom, strDateTo, (int)Session["UserID"]);
            return PartialView("GetMonthStats");
        }
        [AuthAttribute(ModuleID = 3, RightID = "23")]
        public ActionResult StatisticMonth()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var l in le.FPT_SP_CIR_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
            }
            ViewData["lib"] = lib;
            return View();
        }

        [AuthAttribute(ModuleID = 3, RightID = "19")]
        public ActionResult LockPatronStats()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var l in le.CIR_DIC_COLLEGE.ToList())
            {
                lib.Add(new SelectListItem { Text = l.College, Value = l.ID.ToString() });
            }
            ViewData["lib"] = lib;
            //FPT_CIR_GET_LOCKEDPATRONS(PatronCode, LockDateTo, LockDateFrom, LibraryID, UserID)
            //ViewBag.Result = cb.GET_SP_GET_LOCKEDPATRONS_LIST(null, null, null);
            string LibraryFilter = Request.Form["LibraryFilter"];
            if (!string.IsNullOrEmpty(LibraryFilter))
            {
                ViewBag.iddd = Int32.Parse(LibraryFilter);

            }
            else
            {
                ViewBag.iddd = -1;
            }
            string PatronCodeFilter = Request.Form["PatronCodeFilter"];
            string LockDateFromFilter = Request.Form["LockDateFromFilter"];
            string LockDateToFilter = Request.Form["LockDateToFilter"];
            string NoteFilter = Request.Form["NoteFilter"];
            int CollegeID = 0;
            if (!string.IsNullOrEmpty(LibraryFilter)) CollegeID = Convert.ToInt32(LibraryFilter);

            ShelfBusiness shelfBusiness = new ShelfBusiness();
            ViewBag.Library = shelfBusiness.FPT_SP_HOLDING_LIBRARY_SELECT(0, 1, -1, Int32.Parse(Session["UserID"].ToString()), 1);
            ViewBag.Result = cb.GET_SP_GET_LOCKEDPATRONS_LIST(PatronCodeFilter, NoteFilter, LockDateFromFilter, LockDateToFilter, CollegeID);
            List<SelectListItem> note = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn lý do", Value = "" }
            };
            foreach (var l in le.CIR_PATRON_LOCK.ToList())
            {
                note.Add(new SelectListItem { Text = l.Note, Value = l.Note.ToString() });
            }
            ViewData["note"] = note;
            return View(ViewBag.iddd);
        }
        // LockCard()
        [HttpPost]
        public JsonResult LockCardPatron(string cardNumber, string startDate, int lockDays, string note)
        {
            string[] myList = cardNumber.Split(' ');
            int error = 0;
            int mSuccess = 0;
            string listSuccess = "";
            string merror = "";
            string locked = "";
            string numLoan = "";
            if (myList.Length == 1)
            {
                string cnumber = myList[0];
                if (le.CIR_PATRON.Where(a => a.Code == cnumber).Count() == 0)
                {
                    ViewBag.message = "Số thẻ " + myList[0] + " không tồn tại";
                }
                else if (le.CIR_PATRON_LOCK.Where(a => a.PatronCode == cnumber).Count() != 0)
                {
                    ViewBag.message = cnumber + " Đã bị khóa";
                }
                else
                {
                    int pID = le.CIR_PATRON.Where(a => a.Code == cnumber).First().ID;
                    if (le.CIR_LOAN.Where(a => a.PatronID == pID).Count() != 0)
                    {
                        ViewBag.message = "Khóa thẻ thành công !" + "<br>Số thẻ đang mượn sách là : " + cnumber;
                    }
                    else
                    {
                        ViewBag.message = "Khóa thẻ thành công !";
                    }
                    List<SP_LOCK_PATRON_CARD_Result> listResult = cb.GET_SP_LOCK_PATRON_CARD_LIST(myList[0], lockDays, startDate, note);
                }
            }
            else if (myList.Count() > 1)
            {
                foreach (string cnumber in myList)
                {

                    if (le.CIR_PATRON.Where(a => a.Code == cnumber).Count() == 0)
                    {
                        error = error + 1;
                        merror = merror + " " + cnumber;
                    }
                    else if (le.CIR_PATRON_LOCK.Where(a => a.PatronCode == cnumber).Count() != 0)
                    {
                        error = error + 1;
                        locked = locked + " " + cnumber;
                    }
                    else
                    {
                        int pID = le.CIR_PATRON.Where(a => a.Code == cnumber).First().ID;
                        if (le.CIR_LOAN.Where(a => a.PatronID == pID).Count() != 0)
                        {
                            numLoan = numLoan + " " + cnumber;
                        }
                        List<SP_LOCK_PATRON_CARD_Result> listResult = cb.GET_SP_LOCK_PATRON_CARD_LIST(cnumber, lockDays, startDate, note);
                        mSuccess = mSuccess + 1;
                        listSuccess = listSuccess + " " + cnumber;
                    }
                }
                if (error == 0)
                {
                        ViewBag.message = "Khóa thẻ thành công !" + "<br>Số thẻ đang mượn sách là : " + numLoan;

                }
                else
                {
                    ViewBag.message = "Tổng số thẻ khóa thành công : " + mSuccess + "<br>Số thẻ đang mượn sách là : " + numLoan + "<br>Số thẻ không thể khóa : " + error + "<br>Số thẻ không tồn tại là : " + merror + "<br>Số thẻ đã bị khóa là : " + locked;
                }
            }

            return Json(ViewBag.message, JsonRequestBehavior.AllowGet);

        }
        // Edit LockCard()
        [HttpPost]
        public JsonResult UpdatedLockCardPatron(string patronCode, int lockDays, string note)
        {
            List<FPT_SP_UPDATE_UNLOCK_PATRON_CARD_Result> listResult = cb.FPT_SP_UPDATE_UNLOCK_PATRON_CARD(patronCode, lockDays, note);
            ViewData["listResult"] = listResult;
            return Json(listResult, JsonRequestBehavior.AllowGet);

        }
        // UnLockCard()
        [HttpPost]
        public JsonResult UnLockCardPatron(List<string> patroncodeList)
        {
            try
            {
                foreach (var item in patroncodeList)
                {
                    cb.FPT_SP_UNLOCK_PATRON_CARD_LIST("'" + item + "'"); 
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(new { Message = "Mở Khóa thành công!" }, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult GetLockPatronStats(string strPatronCode, string strLockDateTo, string strLockDateFrom, string strCollegeID)
        {
            //int CollegeID = 0;
            //if (!string.IsNullOrEmpty(strCollegeID)) CollegeID = Convert.ToInt32(strCollegeID);
            //ViewBag.Result = cb.GET_SP_GET_LOCKEDPATRONS_LIST(strPatronCode, strLockDateFrom, strLockDateTo, CollegeID);
            //List<SelectListItem> lib = new List<SelectListItem>
            //{
            //    new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            //};
            //foreach (var l in le.FPT_GET_COLLEGE().ToList())
            //{
            //    lib.Add(new SelectListItem { Text = l.COLLEGE, Value = l.ID.ToString() });
            //}
            //ViewData["lib"] = lib;
            return PartialView("GetLockPatronStats");
        }

        [AuthAttribute(ModuleID = 3, RightID = "23")]
        public ActionResult StatisticPatronGroup()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Chọn thư viện", Value = "" }
            };
            foreach (var item in le.SP_HOLDING_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = item.Code, Value = item.ID.ToString() });
            }
            ViewBag.list_lib = lib;
            return View();
        }
        [HttpPost]
        public PartialViewResult DisplayPatronGroup(string strLibID, string strDateFrom, string strDateTo, string strType)
        {
            string userID = Session["UserID"].ToString();
            List<PATRON_GROUP> result_now =
                pb.PATRON_GROUP_NOW(userID, strDateFrom, strDateTo, strType, strLibID);

            List<PATRON_GROUP> result_pass =
                pb.PATRON_GROUP_PASS(userID, strDateFrom, strDateTo, strType, strLibID);

            ViewBag.rnow = result_now;

            ViewBag.rpass = result_pass;

            return PartialView("DisplayPatronGroup");
        }
        [AuthAttribute(ModuleID = 3, RightID = "23")]
        public ActionResult StatisticTopPatron()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var item in le.SP_HOLDING_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = item.Code, Value = item.ID.ToString() });
            }
            ViewData["lib"] = lib;
            return View();
        }
        [HttpPost]
        public PartialViewResult DisplayTopPatron(string strLibID, string strLocPrefix, string strLocID, string strDateFrom, string strDateTo,
            string strNumPatron, string strHireTimes, string strType)
        {
            string userID = Session["UserID"].ToString();

            List<FPT_SP_STAT_PATRONMAX_Result> result =
                pb.FPT_SP_STAT_PATRONMAX_LIST(userID, strDateFrom, strDateTo, strNumPatron, strHireTimes, strType, strLocID, strLocPrefix, strLibID);

            ViewBag.test = result;
            return PartialView("DisplayTopPatron");
        }
        [AuthAttribute(ModuleID = 3, RightID = "23")]
        public ActionResult StatisticTopCopy()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var item in le.SP_HOLDING_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = item.Code, Value = item.ID.ToString() });
            }
            ViewData["lib"] = lib;
            return View();
        }
        [HttpPost]
        public PartialViewResult DisplayTopCopy(string strLibID, string strDateFrom, string strDateTo,
        string strNumPatron, string strHireTimes, string strLocPrefix, string strLocID)
        {
            string userID = Session["UserID"].ToString();

            List<ITEMMAX> result =
                pb.TOP_COPY(userID, strDateFrom, strDateTo, strNumPatron, strHireTimes, strLibID, strLocPrefix, strLocID);

            ViewBag.test = result;
            return PartialView("DisplayTopCopy");
        }

        public JsonResult GetLocationsDemo(int id)
        {
            List<SelectListItem> loc = new List<SelectListItem>();
            loc.Add(new SelectListItem { Text = "Tất cả các kho", Value = "0" });
            foreach (var l in le.SP_HOLDING_LIBLOCUSER_SEL((int)Session["UserID"], id).ToList())
            {
                loc.Add(new SelectListItem { Text = l.Symbol, Value = l.ID.ToString() });
            }
            return Json(new SelectList(loc, "Value", "Text"));
        }
        public ActionResult DemoDataTableEport()
        {
            ViewBag.ParentNodes = le.SP_HOLDING_LIB_SEL((int)Session["UserID"]).ToList();
            //le.SP_HOLDING_LIBLOCUSER_SEL(UserID, id).ToList();
            return View();
        }
        // get list lock patron in datatable
        [HttpPost]
        public JsonResult GetLockPatron(DataTableAjaxPostModel model, int libraryID, string PatronCode, string Note, string StartedDate, string FinishDate)
        {
            var lockedpatron = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "", "", "", 0);
            var search = lockedpatron.Where(a => true);
            if (libraryID != -1)
            {
                List<string> listInLib = le.CIR_PATRON.Where(c => c.CIR_PATRON_GROUP == null ? false : c.CIR_PATRON_GROUP.HOLDING_LOCATION.Select(l => l.LibID).Contains(libraryID)).Select(c => c.Code).ToList();
                search = lockedpatron.Where(a => listInLib.Contains(a.PatronCode));
            }
            if (!string.IsNullOrEmpty(PatronCode))
            {
                search = search.Where(a => a.PatronCode.ToLower().Contains(PatronCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(Note))
            {
                search = search.Where(a => a.Note.ToLower().Contains(Note.ToLower()));
            }
            if (!string.IsNullOrEmpty(StartedDate))
            {
                search = search.Where(a => a.StartedDate.ToString("yyyy-MM-dd").CompareTo(StartedDate) >= 0);
            }
            if (!string.IsNullOrEmpty(FinishDate))
            {
                search = search.Where(a => a.StartedDate.ToString("yyyy-MM-dd").CompareTo(FinishDate) <= 0);
            }
            var paging = search.Skip(model.start).Take(model.length).ToList();
            var result = paging.ToList();
            List<SP_GET_LOCKEDPATRONS_Result2> list = new List<SP_GET_LOCKEDPATRONS_Result2>();
            foreach (var i in result)
            {
                list.Add(new SP_GET_LOCKEDPATRONS_Result2()
                {
                    PatronCode = i.PatronCode,
                    StartedDate = i.StartedDate.ToString("dd/MM/yyyy"),
                    Note = i.Note,
                    FullName = i.FullName,
                    FinishDate = i.FinishDate.ToString("dd/MM/yyyy"),
                    LockedDays = i.LockedDays

                });
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = lockedpatron.Count(),
                recordsFiltered = search.Count(),
                data = list
            });
        }

        // statistic top 20
        public ActionResult CirStatisticTop20()
        {
            List<SelectListItem> lib = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hãy chọn thư viện", Value = "" }
            };
            foreach (var l in le.FPT_SP_CIR_LIB_SEL((int)Session["UserID"]).ToList())
            {
                lib.Add(new SelectListItem { Text = l.Code, Value = l.ID.ToString() });
            }
            ViewData["lib"] = lib;
            List<SelectListItem> cat = new List<SelectListItem>();
            foreach (var c in le.CAT_DIC_LIST.ToList())
            {
                cat.Add(new SelectListItem { Text = c.Name.ToString(), Value = c.ID.ToString() });
            }
            ViewData["cat"] = cat;
            return View();
        }
        //get statistic top 20
        public PartialViewResult GetCirTop20Stats(string strLibID, string strCatID)
        {
            int id = 0;
            int libid = 0;
            if (!String.IsNullOrEmpty(strCatID)) id = Int32.Parse(strCatID);
            if (!String.IsNullOrEmpty(strLibID)) libid = Int32.Parse(strLibID);
            CAT_DIC_LIST cat = le.CAT_DIC_LIST.Where(a => a.ID == id).First();
            if (cat.ID == 38)
            {
                ViewBag.OnLoanResult = null;
                ViewBag.LoanHisResult = null;
            }
            else
            {
                ViewBag.OnLoanResult = cb.FPT_CIR_SP_STAT_TOP20_LIST(0, cat.ID, (int)Session["UserID"], libid);
                ViewBag.LoanHisResult = cb.FPT_CIR_SP_STAT_TOP20_LIST(1, cat.ID, (int)Session["UserID"], libid);
            }
            if(ViewBag.OnLoanResult.Count == 0)
            {
                ViewBag.OnLoanResult = null;
            }
            if (ViewBag.LoanHisResult.Count == 0)
            {
                ViewBag.LoanHisResult = null;
            }
            ViewBag.Category = cat.Name;
            return PartialView("GetCirTop20Stats");
        }

        // list liquid copynumber
        [AuthAttribute(ModuleID = 3, RightID = "25")]
        public ActionResult CopyNumberLiquidationStats()
        {

            return View();
        }

        public PartialViewResult GetCopyNumberLiquidationStats(string strDKCBID)
        {
            strDKCBID = strDKCBID.Trim();
            string[] myList = strDKCBID.Split('\n');
            //foreach (var cnumber in myList)
            //{
            //    string dddd = cnumber;
            //}
                List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> listResult = new List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result>();
            if (myList.Length != 0)
            {
                foreach (var cnumber in myList)
                {
                    List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> list = cb.FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST(cnumber);
                    if(list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            item.Content = GetContent(item.Content);
                            listResult.Add(
                                new FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result(
                                    item.Reason, item.Content, item.CopyNumber,
                                    item.LiquidCode, item.Price, item.UseCount, item.RemovedDate));
                        }
                    }
                    
                }
                

            }
            if(listResult.Count > 0)
            {
                ViewBag.Result = listResult;
            }
            else
            {
                ViewBag.Result = null;
            }
             
            return PartialView("GetCopyNumberLiquidationStats");
        }        
    }

    public class GET_PATRON_LOANINFOR_Result_2
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public string CheckOutDate { get; set; }
        public string CheckInDate { get; set; }
        public int? RenewCount { get; set; }
        public string Serial { get; set; }
        public string FullName { get; set; }
        public int? OverdueDays { get; set; }
        public decimal OverdueFine { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
    }

    public class GET_PATRON_RENEW_LOAN_INFOR_Result_2
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public string CheckOutDate { get; set; }
        public string CheckInDate { get; set; }
        public string FullName { get; set; }
        public string RenewDate { get; set; }
        public string OverDueDateNew { get; set; }
        public string OverDueDateOld { get; set; }
        public int? OverdueDays { get; set; }
        public string OverdueFine { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
		public string OverDueDate { get; set; }
	}

    public class GET_PATRON_ONLOANINFOR_Result_2
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public string CheckOutDate { get; set; }
        public string DueDate { get; set; }
        public Nullable<System.Int16> RenewCount { get; set; }
        public string Serial { get; set; }
        public string FullName { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
    }

    public class GET_PATRON_RENEW_ONLOAN_INFOR_Result_2
    {
        public string Content { get; set; }
        public string CopyNumber { get; set; }
        public string CheckOutDate { get; set; }
        public string DueDate { get; set; }
        public string FullName { get; set; }
        public string RenewDate { get; set; }
        public string OverDueDateNew { get; set; }
        public string OverDueDateOld { get; set; }
        public string CheckInDate { get; set; }
        public string Price { get; set; }
		public string OverDueDate { get; set; }
		public string FeeOverDueDate { get; set; }
		public string Currency { get; set; }
    }
}