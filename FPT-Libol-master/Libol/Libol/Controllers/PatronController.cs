using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Libol.Models;
using Libol.SupportClass;
using OfficeOpenXml;

namespace Libol.Controllers
{
    public class PatronController : Controller
    {
        private LibolEntities db = new LibolEntities();

        [AuthAttribute(ModuleID = 2, RightID = "0")]
        public ActionResult PatronProfile()
        {
            return View();
        }

        [AuthAttribute(ModuleID = 2, RightID = "1")]
        public ActionResult SearchPatronFilter()
        {
            ViewBag.PatronGroup = db.SP_PAT_GET_PATRONGROUP().ToList();
            ViewBag.Faculty = db.CIR_DIC_FACULTY.Select(a => a.Faculty).Distinct().ToList();
            ViewBag.Occupation = db.SP_PAT_GET_OCCUPATION().ToList();
            return View();
        }

        [AuthAttribute(ModuleID = 2, RightID = "2,3")]
        public ActionResult Create(string strPatronID)
        {
            
            ViewBag.Ethnic = db.SP_PAT_GET_ETHNIC().ToList();
            ViewBag.PatronGroup = db.SP_PAT_GET_PATRONGROUP().ToList();
            ViewBag.Education = db.SP_PAT_GET_EDUCATION().ToList();
            ViewBag.Occupation = db.SP_PAT_GET_OCCUPATION().ToList();
            ViewBag.College = db.SP_PAT_GET_COLLEGE().ToList();
            int CollegeID = db.SP_PAT_GET_COLLEGE().ToList()[0].ID;
            ViewBag.Faculty = db.CIR_DIC_FACULTY.Where(a => a.CollegeID == CollegeID).ToList();
            ViewBag.Province = db.CIR_DIC_PROVINCE.ToList();
            ViewBag.Countries = db.SP_GET_COUNTRIES().ToList();

            
            if(!String.IsNullOrEmpty(strPatronID))
            {
                int id = Int32.Parse(strPatronID);
                var patron = db.CIR_PATRON.Where(a => a.ID == id).Count() == 0 ? null : db.CIR_PATRON.Where(a => a.ID == id).First();
                if(patron != null)
                {
                    if (patron.CIR_PATRON_UNIVERSITY != null)
                    {
                        ViewBag.Faculty = db.CIR_DIC_FACULTY.Where(a => a.CollegeID == patron.CIR_PATRON_UNIVERSITY.CollegeID).ToList();
                    }

                    return View(patron);
                }
                else
                {
                    return View(new CIR_PATRON());
                }
            }
            else
            {
                return View(new CIR_PATRON());
            }
            
        }



        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "0")]
        public JsonResult OnchangeCollege(int CollegeID)
        {
            ViewBag.Faculty = db.CIR_DIC_FACULTY.Where(a => a.CollegeID == CollegeID).ToList();
            List<CIR_DIC_FACULTY> list = new List<CIR_DIC_FACULTY>();
            foreach (var f in ViewBag.Faculty)
            {
                list.Add(new CIR_DIC_FACULTY()
                {
                    ID = f.ID,
                    Faculty = f.Faculty,
                    CollegeID = f.CollegeID
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "2")]
        public JsonResult NewPatron(string strCode, string strValidDate, string strExpiredDate, string strLastIssuedDate, string strLastName, string strFirstName,
             Nullable<bool> blnSex, string strDOB, Nullable<int> intEthnicID, Nullable<int> intEducationID, Nullable<int> intOccupationID,
            string strWorkPlace, string strTelephone, string strMobile, string strEmail, string strPortrait, Nullable<int> intPatronGroupID, string strNote,
            Nullable<int> intIsQue, string strIDCard, string strAddress, Nullable<int> intProvinceID, string strCity, Nullable<int> intCountryID, string strZip,
            Nullable<int> intisActive, int intCollegeID, int intFacultyID, string strGrade, string strClass)
        {
            string InvalidFields = "";
            if ( String.IsNullOrEmpty(strFirstName))
            {
                InvalidFields += "strFirstName-";
            }
            if (String.IsNullOrEmpty(strLastName))
            {
                InvalidFields += "strLastName-";
            }
            if (String.IsNullOrEmpty(strDOB))
            {
                InvalidFields += "strDOB-";
            }
            if (String.IsNullOrEmpty(strCode))
            {
                InvalidFields += "strCode-";
            }
            if (intPatronGroupID == null)
            {
                InvalidFields += "intPatronGroupID-";
            }
            if (String.IsNullOrEmpty(strValidDate))
            {
                InvalidFields += "strValidDate-";
            }
            if (String.IsNullOrEmpty(strExpiredDate))
            {
                InvalidFields += "strExpiredDate-";
            }
            if (String.IsNullOrEmpty(strLastIssuedDate))
            {
                InvalidFields += "strLastIssuedDate-";
            }
            //if (intCollegeID == -1)
            //{
            //    InvalidFields += "college-";
            //}
            //if (intFacultyID == -1)
            //{
            //    InvalidFields += "faculty-";
            //}
            if (String.IsNullOrEmpty(strWorkPlace))
            {
                InvalidFields += "strWorkPlace-";
            }
            if (String.IsNullOrEmpty(strAddress))
            {
                InvalidFields += "strAddress-";
            }
            if (String.IsNullOrEmpty(strEmail))
            {
                InvalidFields += "strEmail-";
            }

            if (InvalidFields != "")
            {
                return Json(new Result()
                {
                    CodeError = 1,
                    Data = InvalidFields
                }, JsonRequestBehavior.AllowGet);
            }
            else
            if (db.CIR_PATRON.Where(a => a.Code == strCode).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Bạn đọc với số thẻ " + strCode + " đã tồn tại!"
                }, JsonRequestBehavior.AllowGet);
            }else
            if (db.CIR_PATRON.Where(a => a.Code != strCode && a.Email == strEmail).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Email " + strEmail + " không hợp lệ!"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string strMiddleName = "";
                if (strFirstName.Split(' ').Length > 1)
                {
                    List<string> names = strFirstName.Split(' ').ToList();
                    string firstName = names.First();
                    names.RemoveAt(0);
                    strMiddleName = string.Join(",", names);
                    strFirstName = firstName;
                }

                var intPatronID = new ObjectParameter("intRetval", typeof(int));
                db.SP_PAT_CREATE_PATRON(
                    strCode, strValidDate, strExpiredDate, strLastIssuedDate, strLastName, strFirstName, strMiddleName, blnSex, strDOB, intEthnicID, intEducationID,
                    intOccupationID, strWorkPlace, strTelephone, strMobile, strEmail, strPortrait, intPatronGroupID, strNote, intIsQue, strIDCard, intPatronID
                    );
                int patronID = (int)intPatronID.Value;
                db.CIR_PATRON.Where(a => a.ID == patronID).First().Password = strCode;
                db.SaveChanges();
                if (strAddress != null && strAddress != "")
                {
                    db.SP_PAT_CREATE_OTHERADDRESS(patronID, strAddress, intProvinceID, strCity, intCountryID, strZip, intisActive);
                }
                if (intCollegeID > 0)
                {
                    db.SP_PAT_CREATE_PATRON_UNIV(patronID, intFacultyID, intCollegeID, strGrade, strClass);
                }
                return Json(new Result()
                {
                    CodeError = 0,
                    Data = strCode
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "3")]
        public JsonResult UpdatePatron(int ID, string strCode, string strValidDate, string strExpiredDate, string strLastIssuedDate, string strLastName, string strFirstName,
             Nullable<bool> blnSex, string strDOB, Nullable<int> intEthnicID, Nullable<int> intEducationID, Nullable<int> intOccupationID,
            string strWorkPlace, string strTelephone, string strMobile, string strEmail, string strPortrait, Nullable<int> intPatronGroupID, string strNote,
            Nullable<int> intIsQue, string strIDCard, string strAddress, Nullable<int> intProvinceID, string strCity, Nullable<int> intCountryID, string strZip,
            Nullable<int> intisActive, int intCollegeID, int intFacultyID, string strGrade, string strClass)
        {
            var patron = db.CIR_PATRON.Where(a => a.ID == ID).Count() == 0 ? null : db.CIR_PATRON.Where(a => a.ID == ID).First();
            if (patron == null)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Xảy ra lỗi vui lòng tìm kiếm lại!"
                }, JsonRequestBehavior.AllowGet);
            }


            string InvalidFields = "";
            if (String.IsNullOrEmpty(strFirstName))
            {
                InvalidFields += "strFirstName-";
            }
            if (String.IsNullOrEmpty(strLastName))
            {
                InvalidFields += "strLastName-";
            }
            if (String.IsNullOrEmpty(strDOB))
            {
                InvalidFields += "strDOB-";
            }
            if (String.IsNullOrEmpty(strCode))
            {
                InvalidFields += "strCode-";
            }
            if (intPatronGroupID == null)
            {
                InvalidFields += "intPatronGroupID-";
            }
            if (String.IsNullOrEmpty(strValidDate))
            {
                InvalidFields += "strValidDate-";
            }
            if (String.IsNullOrEmpty(strExpiredDate))
            {
                InvalidFields += "strExpiredDate-";
            }
            if (String.IsNullOrEmpty(strLastIssuedDate))
            {
                InvalidFields += "strLastIssuedDate-";
            }
            //if (intCollegeID == -1)
            //{
            //    InvalidFields += "college-";
            //}
            //if (intFacultyID == -1)
            //{
            //    InvalidFields += "faculty-";
            //}
            if (String.IsNullOrEmpty(strWorkPlace))
            {
                InvalidFields += "strWorkPlace-";
            }
            if (String.IsNullOrEmpty(strAddress))
            {
                InvalidFields += "strAddress-";
            }
            if (String.IsNullOrEmpty(strEmail))
            {
                InvalidFields += "strEmail-";
            }

            if (InvalidFields != "")
            {
                return Json(new Result()
                {
                    CodeError = 1,
                    Data = InvalidFields
                }, JsonRequestBehavior.AllowGet);
            }
            else
            if (db.CIR_PATRON.Where(a => (a.Code == strCode && a.ID != ID)).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Bạn đọc với số thẻ " + strCode + " đã tồn tại!"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            if (db.CIR_PATRON.Where(a => a.Code != strCode && a.Email == strEmail).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Email " + strEmail + " không hợp lệ!"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string strMiddleName = "";
                if (strFirstName.Split(' ').Length > 1)
                {
                    List<string> names = strFirstName.Split(' ').ToList();
                    string firstName = names.First();
                    names.RemoveAt(0);
                    strMiddleName = string.Join(",", names);
                    strFirstName = firstName;
                }

                var intPatronID = new ObjectParameter("intRetval", typeof(int));
                if (String.IsNullOrEmpty(strPortrait))
                {
                    strPortrait = db.CIR_PATRON.Where(a => a.ID == ID).First().Portrait;
                }
                db.SP_PAT_UPDATE_PATRON(
                    ID, strCode, strValidDate, strExpiredDate, strLastIssuedDate, strLastName, strFirstName, strMiddleName, blnSex, strDOB, intEthnicID, intEducationID,
                    intOccupationID, strWorkPlace, strTelephone, strMobile, strEmail, strPortrait, intPatronGroupID, strNote, strIDCard, intPatronID
                    );
                int patronID = (int)intPatronID.Value;
                db.CIR_PATRON.Where(a => a.ID == patronID).First().Password = strCode;
                db.SaveChanges();
                if (strAddress != null && strAddress != "" && patron.CIR_PATRON_OTHER_ADDR.Count() > 0)
                {
                    db.SP_CIR_PATRON_OA_DELETE(patron.CIR_PATRON_OTHER_ADDR.First().ID);
                    db.SP_PAT_CREATE_OTHERADDRESS(patronID, strAddress, intProvinceID, strCity, intCountryID, strZip, intisActive);
                }
                if (intCollegeID > 0)
                {
                    db.SP_PAT_UPDATE_PATRON_UNIV(patronID, intFacultyID, intCollegeID, strGrade, strClass);
                }
                return Json(new Result()
                {
                    CodeError = 0,
                    Data = strCode
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "0")]
        public JsonResult UploadPhotoPatron()
        {
            string strCode = Request.Form["strCode"];
            if (strCode != null)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var fileName = strCode + " - " + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/ImagePatron"), fileName);
                    file.SaveAs(path);
                    db.CIR_PATRON.Where(a => a.Code == strCode).First().Portrait = fileName;
                    db.SaveChanges();
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [AuthAttribute(ModuleID = 2, RightID = "4")]
        public ActionResult AddPatronByFile()
        {
            return View();
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "4")]
        public ActionResult PreviewPatronFile()
        {
            string[] result;
            int day = 0;
            int month = 0;
            int year = 0;
            int stop = 0;
            string date = "";
            char[] splitchar = { '/' };
            List<PatronFile> listPatronInFile = new List<PatronFile>();
            List<PatronFile> listPatronInFileInvalid = new List<PatronFile>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileName = Path.GetFileName(file.FileName);
                if (String.IsNullOrEmpty(fileName))
                {
                    ViewBag.ListPatron = listPatronInFile;
                    ViewBag.ListPatronInvalid = listPatronInFileInvalid;
                    return View();
                }
                var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                file.SaveAs(path);

                FileInfo excel = new FileInfo(Server.MapPath("/Uploads/" + fileName));
                using (var package = new ExcelPackage(excel))
                {
                    var workbook = package.Workbook;

                    //*** Sheet 1
                    var worksheet = workbook.Worksheets.First();

                    //*** Retrieve to List                    
                    int totalRows = worksheet.Dimension.End.Row;
                    for (int u = 2; u <= totalRows; u++)
                    {
                        if (!String.IsNullOrEmpty(worksheet.Cells[u, 2].Text.ToString()))
                        {
                            string dob = worksheet.Cells[u, 5].Text.ToString();
                            result = dob.Split(splitchar);
                            for (int count = 0; count <= result.Length - 1; count++)
                            {
                                stop++;
                                //Console.WriteLine(result[count]);
                                day = Convert.ToInt32(result[0]);
                                month = Convert.ToInt32(result[1]);
                                year = Convert.ToInt32(result[2]);

                                if (stop == 1)
                                {
                                    break;
                                }
                            }

                            date = month + "/" + day + "/" + year;


                            DateTime e = DateTime.Parse(date);
                            listPatronInFile.Add(new PatronFile
                            {
                                strCode = worksheet.Cells[u, 2].Text.ToString(),
                                FullName = worksheet.Cells[u, 3].Text.ToString(),
                                blnSex = worksheet.Cells[u, 4].Text.ToString(),
                                strDOB = e /*Convert.ToDateTime(worksheet.Cells[u, 5].Text.ToString())*/,
                                strEmail = worksheet.Cells[u, 6].Text.ToString(),
                                strAddress = worksheet.Cells[u, 7].Text.ToString(),
                                Faculty = worksheet.Cells[u, 8].Text.ToString(),
                                strMobile = worksheet.Cells[u, 9].Text.ToString(),
                                strGrade = worksheet.Cells[u, 10].Text.ToString(),
                                College = worksheet.Cells[u, 11].Text.ToString(),
                                strCity = worksheet.Cells[u, 12].Text.ToString(),
                                strClass = worksheet.Cells[u, 13].Text.ToString(),
                                PatronGroup = worksheet.Cells[u, 14].Text.ToString(),
                            });
                        }

                    }
                }

            }

            ViewBag.ListPatron = listPatronInFile;
            ViewBag.ListPatronInvalid = listPatronInFileInvalid;



            if (listPatronInFile != null)
            {
                foreach (PatronFile p in listPatronInFile)
                {
                    string strLastName = "";
                    string strFirstName = "";
                    if (p.FullName.Split(' ').Length > 1)
                    {
                        List<string> names = p.FullName.Split(' ').ToList();
                        strLastName = names.Last();
                        names.RemoveAt(names.Count - 1);
                        strFirstName = string.Join(" ", names);
                    }
                    int intPatronGroupID = 0;
                    CIR_PATRON_GROUP patronGroup = db.CIR_PATRON_GROUP.Where(a => a.Name.Trim() == p.PatronGroup.Trim()).Count() == 0 ?
                        null : db.CIR_PATRON_GROUP.Where(a => a.Name.Trim() == p.PatronGroup.Trim()).First();
                    if (patronGroup != null)
                    {
                        intPatronGroupID = patronGroup.ID;
                    }
                    int intCollegeID = 0;
                    CIR_DIC_COLLEGE college = db.CIR_DIC_COLLEGE.Where(a => a.College.Trim() == p.College.Trim()).Count() == 0 ?
                        null : db.CIR_DIC_COLLEGE.Where(a => a.College.Trim() == p.College.Trim()).First();
                    if (college != null)
                    {
                        intCollegeID = college.ID;
                    }
                    int intFacultyID = 0;
                    CIR_DIC_FACULTY faculty = db.CIR_DIC_FACULTY.Where(a => a.CollegeID == intCollegeID).Where(a => a.Faculty.Trim() == p.Faculty.Trim()).Count() == 0 ?
                        null : db.CIR_DIC_FACULTY.Where(a => a.CollegeID == intCollegeID).Where(a => a.Faculty.Trim() == p.Faculty.Trim()).First();
                    if (faculty != null)
                    {
                        intFacultyID = faculty.ID;
                    }
                    DateTime strExpiredDate = DateTime.Now;
                    strExpiredDate = strExpiredDate.AddYears(4);
                    NewPatron(p.strCode, DateTime.Now.ToShortDateString(), strExpiredDate.ToShortDateString(), DateTime.Now.ToShortDateString(), strLastName, strFirstName, p.blnSex == "Nam" ? true : false, p.strDOB.ToString("yyyy-MM-dd"), null, null, null, "Đại học FPT", null, p.strMobile
                        , p.strEmail, null, intPatronGroupID, null, 0, null, p.strAddress, 1, p.strCity, 209, "", 0, intCollegeID, intFacultyID, p.strGrade, p.strClass);
                }
            }
            return View();
        }

        public bool CheckCodeInFile(string strCode, List<PatronFile> listPatronInFile)
        {
            bool IsValid = true;
            foreach(PatronFile p in listPatronInFile)
            {
                if(p.strCode == strCode)
                {
                    IsValid = false;
                }
            }
            return IsValid;
        }        

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "2,3")]
        public JsonResult AddDictionary(string field, string data, int CollegeID)
        {
            AddDictionaryResult addDictionaryResult = new AddDictionaryResult();
            List<DictionarySelection> list = new List<DictionarySelection>();
            if (field == "intEthnicID")
            {
                db.SP_PAT_CREATE_ETHNIC(data, new ObjectParameter("intOut", typeof(int)));
                addDictionaryResult.Field = "intEthnicID";
                foreach(SP_PAT_GET_ETHNIC_Result r in db.SP_PAT_GET_ETHNIC().ToList())
                {
                    DictionarySelection dictionary = new DictionarySelection();
                    dictionary.ID = r.ID;
                    dictionary.Data = r.Ethnic;
                    list.Add(dictionary);
                }
                addDictionaryResult.ListSelection = list;
                
            }

            if (field == "intOccupationID")
            {
                db.SP_PAT_CREATE_OCCUPATION(data, new ObjectParameter("intOut", typeof(int)));
                addDictionaryResult.Field = "intOccupationID";
                foreach (SP_PAT_GET_OCCUPATION_Result r in db.SP_PAT_GET_OCCUPATION().ToList())
                {
                    DictionarySelection dictionary = new DictionarySelection();
                    dictionary.ID = r.ID;
                    dictionary.Data = r.Occupation;
                    list.Add(dictionary);
                }
                addDictionaryResult.ListSelection = list;
            }

            if (field == "college")
            {
                db.SP_PAT_CREATE_COLLEGE(data, new ObjectParameter("intOut", typeof(int)));
                addDictionaryResult.Field = "college";
                foreach (SP_PAT_GET_COLLEGE_Result r in db.SP_PAT_GET_COLLEGE().ToList())
                {
                    DictionarySelection dictionary = new DictionarySelection();
                    dictionary.ID = r.ID;
                    dictionary.Data = r.College;
                    list.Add(dictionary);
                }
                addDictionaryResult.ListSelection = list;
            }

            if (field == "faculty")
            {
                db.SP_PAT_CREATE_FACULTY(CollegeID ,data, new ObjectParameter("intOut", typeof(int)));
                addDictionaryResult.Field = "faculty";
                foreach (CIR_DIC_FACULTY r in db.CIR_DIC_FACULTY.Where(a => a.CollegeID == CollegeID).ToList())
                {
                    DictionarySelection dictionary = new DictionarySelection();
                    dictionary.ID = r.ID;
                    dictionary.Data = r.Faculty;
                    list.Add(dictionary);
                }
                addDictionaryResult.ListSelection = list;
            }

            if (field == "intProvinceID")
            {
                db.SP_PAT_CREATE_PROVINCE(data, new ObjectParameter("intOut", typeof(int)));
                addDictionaryResult.Field = "intProvinceID";
                foreach (CIR_DIC_PROVINCE r in db.CIR_DIC_PROVINCE.ToList())
                {
                    DictionarySelection dictionary = new DictionarySelection();
                    dictionary.ID = r.ID;
                    dictionary.Data = r.Province;
                    list.Add(dictionary);
                }
                addDictionaryResult.ListSelection = list;
            }

            if (field == "intEducationID")
            {
                db.SP_PAT_CREATE_EDUCATION(data, new ObjectParameter("intOut", typeof(int)));
                addDictionaryResult.Field = "intEducationID";
                foreach (SP_PAT_GET_EDUCATION_Result r in db.SP_PAT_GET_EDUCATION().ToList())
                {
                    DictionarySelection dictionary = new DictionarySelection();
                    dictionary.ID = r.ID;
                    dictionary.Data = r.EducationLevel;
                    list.Add(dictionary);
                }
                addDictionaryResult.ListSelection = list;
            }

            return Json(addDictionaryResult, JsonRequestBehavior.AllowGet);
        }

        [AuthAttribute(ModuleID = 2, RightID = "1")]
        public ActionResult SearchPatron()
        {
            ViewBag.Ethnic = db.SP_PAT_GET_ETHNIC().ToList();
            ViewBag.PatronGroup = db.SP_PAT_GET_PATRONGROUP().ToList();
            ViewBag.College = db.SP_PAT_GET_COLLEGE().ToList();
            ViewBag.Faculty = db.CIR_DIC_FACULTY.Select(a => a.Faculty).Distinct().ToList();
            return View();
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "1")]
        public JsonResult ListPatron(DataTableAjaxPostModel model,string strCode, string  blnSex, string strLastIssuedDate, string  intPatronGroupID,
            string strClass, string strGrade, string strName, string strDOB, string strExpiredDate, string faculty, string intOccupationID)
        {
            string name = strName.Replace("  ", " ").Trim();
            while(name.Contains("  "))
            {
                name = name.Replace("  ", " ");
            }
            var patrons = db.CIR_PATRON;
            var search = patrons.Where(a => true);
            if (!String.IsNullOrEmpty(strCode.Trim()))
            {
                search = search.Where(a => a.Code.Contains(strCode.Trim()));
            }
            if (!String.IsNullOrEmpty(blnSex))
            {
                search = search.Where(a => a.Sex.ToString() == blnSex);
            }
            if (!String.IsNullOrEmpty(strLastIssuedDate))
            {
                search = search.Where(a => (SqlFunctions.DatePart("year", a.LastIssuedDate) + "-" + SqlFunctions.DatePart("month", a.LastIssuedDate) + "-" + SqlFunctions.DatePart("day", a.LastIssuedDate)) == (strLastIssuedDate)
                        || ( SqlFunctions.DatePart("year", a.LastIssuedDate) + "-" + SqlFunctions.DatePart("month", a.LastIssuedDate) + "-0" + SqlFunctions.DatePart("day", a.LastIssuedDate))==(strLastIssuedDate)
                        || (SqlFunctions.DatePart("year", a.LastIssuedDate) + "-0" + SqlFunctions.DatePart("month", a.LastIssuedDate) + "-" + SqlFunctions.DatePart("day", a.LastIssuedDate))==(strLastIssuedDate)
                        || (SqlFunctions.DatePart("year", a.LastIssuedDate) + "-0" + SqlFunctions.DatePart("month", a.LastIssuedDate) + "-0" + SqlFunctions.DatePart("day", a.LastIssuedDate))==(strLastIssuedDate));
            }
            if (!String.IsNullOrEmpty(intPatronGroupID))
            {
                search = search.Where(a => a.CIR_PATRON_GROUP == null ? false : a.CIR_PATRON_GROUP.Name == intPatronGroupID);
            }
            if (!String.IsNullOrEmpty(strClass.Trim()))
            {
                search = search.Where(a => a.CIR_PATRON_UNIVERSITY != null && a.CIR_PATRON_UNIVERSITY.Class.Contains(strClass.Trim()));
            }
            if (!String.IsNullOrEmpty(strGrade.Trim()))
            {
                search = search.Where(a => a.CIR_PATRON_UNIVERSITY != null && a.CIR_PATRON_UNIVERSITY.Grade.Contains(strGrade.Trim()));
            }
            if (!String.IsNullOrEmpty(name))
            {
                search = search.Where(a => (a.FirstName.Trim() + " " + a.MiddleName.Trim() + " " + a.LastName.Trim()).Contains(name)
                        || (a.FirstName.Trim() + " " + a.LastName.Trim()).Contains(name));
            }
            if (!String.IsNullOrEmpty(strDOB))
            {
                search = search.Where(a => (SqlFunctions.DatePart("year", a.DOB) + "-" + SqlFunctions.DatePart("month", a.DOB) + "-" + SqlFunctions.DatePart("day", a.DOB)) == (strDOB)
                        || (SqlFunctions.DatePart("year", a.DOB) + "-" + SqlFunctions.DatePart("month", a.DOB) + "-0" + SqlFunctions.DatePart("day", a.DOB)) == (strDOB)
                        || (SqlFunctions.DatePart("year", a.DOB) + "-0" + SqlFunctions.DatePart("month", a.DOB) + "-" + SqlFunctions.DatePart("day", a.DOB)) == (strDOB)
                        || (SqlFunctions.DatePart("year", a.DOB) + "-0" + SqlFunctions.DatePart("month", a.DOB) + "-0" + SqlFunctions.DatePart("day", a.DOB)) == (strDOB));
            }
            if (!String.IsNullOrEmpty(strExpiredDate))
            {
                search = search.Where(a => (SqlFunctions.DatePart("year", a.ExpiredDate) + "-" + SqlFunctions.DatePart("month", a.ExpiredDate) + "-" + SqlFunctions.DatePart("day", a.ExpiredDate)) == (strExpiredDate)
                        || (SqlFunctions.DatePart("year", a.ExpiredDate) + "-" + SqlFunctions.DatePart("month", a.ExpiredDate) + "-0" + SqlFunctions.DatePart("day", a.ExpiredDate)) == (strExpiredDate)
                        || (SqlFunctions.DatePart("year", a.ExpiredDate) + "-0" + SqlFunctions.DatePart("month", a.ExpiredDate) + "-" + SqlFunctions.DatePart("day", a.ExpiredDate)) == (strExpiredDate)
                        || (SqlFunctions.DatePart("year", a.ExpiredDate) + "-0" + SqlFunctions.DatePart("month", a.ExpiredDate) + "-0" + SqlFunctions.DatePart("day", a.ExpiredDate)) == (strExpiredDate));
            }
            if (!String.IsNullOrEmpty(faculty))
            {
                search = search.Where(a => a.CIR_PATRON_UNIVERSITY != null && a.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY != null && a.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY.Faculty == (faculty));
            }
            if (!String.IsNullOrEmpty(intOccupationID))
            {
                search = search.Where(a => a.CIR_DIC_OCCUPATION != null && a.CIR_DIC_OCCUPATION.Occupation == intOccupationID);
            }
            if (model.search.value != null)
            {
                string searchValue = model.search.value;
                search = search.Where(a => a.Code.Contains(searchValue)
                        || (a.FirstName.Trim() + " " + a.MiddleName.Trim() + " " + a.LastName.Trim()).Contains(searchValue)
                        || (a.FirstName.Trim() +  " " + a.LastName.Trim()).Contains(searchValue)
                        || (SqlFunctions.DatePart("day", a.DOB) + "/" + SqlFunctions.DatePart("month", a.DOB) + "/" + SqlFunctions.DatePart("year", a.DOB)).Contains(searchValue)
                        || ("0"+SqlFunctions.DatePart("day", a.DOB) + "/" + SqlFunctions.DatePart("month", a.DOB) + "/" + SqlFunctions.DatePart("year", a.DOB)).Contains(searchValue)
                        || (SqlFunctions.DatePart("day", a.DOB) + "/0" + SqlFunctions.DatePart("month", a.DOB) + "/" + SqlFunctions.DatePart("year", a.DOB)).Contains(searchValue)
                        || ("0"+SqlFunctions.DatePart("day", a.DOB) + "/0" + SqlFunctions.DatePart("month", a.DOB) + "/" + SqlFunctions.DatePart("year", a.DOB)).Contains(searchValue)
                        || a.Sex.Contains(searchValue)
                        || (a.CIR_DIC_ETHNIC != null && a.CIR_DIC_ETHNIC.Ethnic.Contains(searchValue))
                        || (a.CIR_PATRON_UNIVERSITY != null && a.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE != null && a.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE.College.Contains(searchValue))
                        || (a.CIR_PATRON_UNIVERSITY != null && a.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY != null && a.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY.Faculty.Contains(searchValue))
                        || (a.CIR_PATRON_UNIVERSITY != null && a.CIR_PATRON_UNIVERSITY.Grade.Contains(searchValue))
                        || (a.CIR_PATRON_UNIVERSITY != null && a.CIR_PATRON_UNIVERSITY.Class.Contains(searchValue))
                        || a.Telephone.Contains(searchValue)
                        || a.Mobile.Contains(searchValue)
                        || a.Email.Contains(searchValue)
                        || (a.CIR_PATRON_GROUP != null && a.CIR_PATRON_GROUP.Name.Contains(searchValue))
                );
                
                
            }
            
            var sorting = search.OrderBy(a => a.ID);
            var paging = sorting.Skip(model.start).Take(model.length).ToList();
            var result = new List<CustomPatron>(paging.Count);
            foreach (var s in paging)
            {
                result.Add(new CustomPatron
                {
                    strCode = s.Code,
                    Name = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                    strDOB = Convert.ToDateTime(s.DOB).ToString("dd/MM/yyyy"),
                    strLastIssuedDate = Convert.ToDateTime(s.LastIssuedDate).ToString("dd/MM/yyyy"),
                    strExpiredDate = Convert.ToDateTime(s.ExpiredDate).ToString("dd/MM/yyyy"),
                    Sex = s.Sex == "1" ? "Nam": "Nữ",
                    intEthnicID = db.CIR_DIC_ETHNIC.Where(a => a.ID == s.EthnicID).Count() == 0? "" : db.CIR_DIC_ETHNIC.Where(a => a.ID == s.EthnicID).First().Ethnic,
                    intCollegeID = (s.CIR_PATRON_UNIVERSITY == null || s.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE == null) ? "" : s.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE.College,
                    intFacultyID = (s.CIR_PATRON_UNIVERSITY == null || s.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY == null) ? "" : s.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY.Faculty,
                    strGrade = s.CIR_PATRON_UNIVERSITY == null ? "" : s.CIR_PATRON_UNIVERSITY.Grade,
                    strClass =s.CIR_PATRON_UNIVERSITY == null ? "" : s.CIR_PATRON_UNIVERSITY.Class,
                    strAddress = s.CIR_PATRON_OTHER_ADDR.Count == 0 ? "" : s.CIR_PATRON_OTHER_ADDR.First().Address,
                    strTelephone = s.Telephone,
                    strMobile = s.Mobile,
                    strEmail = s.Email,
                    strNote = s.Note,
                    intOccupationID = s.CIR_DIC_OCCUPATION == null ? "" : s.CIR_DIC_OCCUPATION.Occupation,
                    intPatronGroupID = s.CIR_PATRON_GROUP == null ? "": s.CIR_PATRON_GROUP.Name
                });
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = patrons.Count(),
                recordsFiltered = search.Count(),
                data = result
            });
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "1")]
        public PartialViewResult PatronDetail(string strCode)
        {
            var patron = db.CIR_PATRON.Where(a => a.Code == strCode).First();
            ViewBag.PatronDetail = new CustomPatron
            {
                ID = patron.ID,
                strCode = patron.Code,
                Name = patron.FirstName + " " + patron.MiddleName + " " + patron.LastName,
                strDOB = Convert.ToDateTime(patron.DOB).ToString("dd/MM/yyyy"),
                strLastIssuedDate = Convert.ToDateTime(patron.LastIssuedDate).ToString("dd/MM/yyyy"),
                strExpiredDate = Convert.ToDateTime(patron.ExpiredDate).ToString("dd/MM/yyyy"),
                Sex = patron.Sex == "1" ? "Nam" : "Nữ",
                intEthnicID = db.CIR_DIC_ETHNIC.Where(a => a.ID == patron.EthnicID).Count() == 0 ? "" : db.CIR_DIC_ETHNIC.Where(a => a.ID == patron.EthnicID).First().Ethnic,
                intCollegeID = (patron.CIR_PATRON_UNIVERSITY == null || patron.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE == null) ? "" : patron.CIR_PATRON_UNIVERSITY.CIR_DIC_COLLEGE.College,
                intFacultyID = (patron.CIR_PATRON_UNIVERSITY == null || patron.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY == null) ? "" : patron.CIR_PATRON_UNIVERSITY.CIR_DIC_FACULTY.Faculty,
                strGrade = patron.CIR_PATRON_UNIVERSITY == null ? "" : patron.CIR_PATRON_UNIVERSITY.Grade,
                strClass = patron.CIR_PATRON_UNIVERSITY == null ? "" : patron.CIR_PATRON_UNIVERSITY.Class,
                strAddress = patron.CIR_PATRON_OTHER_ADDR.Count == 0 ? "" : patron.CIR_PATRON_OTHER_ADDR.First().Address,
                strTelephone = patron.Telephone,
                strMobile = patron.Mobile,
                strEmail = patron.Email,
                strNote = patron.Note,
                intOccupationID = patron.CIR_DIC_OCCUPATION == null ? "" : patron.CIR_DIC_OCCUPATION.Occupation,
                intPatronGroupID = patron.CIR_PATRON_GROUP == null ? "" : patron.CIR_PATRON_GROUP.Name
            };
            return PartialView("_SearchPatronDetail");
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "5")]
        public JsonResult DeletePatron(string strPatronID)
        {
            int id = Int32.Parse(strPatronID);
            db.SP_PATRON_BATCH_DELETE(strPatronID);
            if(db.CIR_PATRON.Where(a => a.ID == id).Count() > 0)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            
        }

        [AuthAttribute(ModuleID = 2, RightID = "33")]
        public ActionResult DeletePatronsByList()
        {
            return View();
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "33")]
        public JsonResult DeletePatrons(string strPatronCodes)
        {
            List<CustomPatron> listCanNotDel = new List<CustomPatron>();
            List<string> listDel = new List<string>();
            foreach (var patronCode in strPatronCodes.Split('\n'))
            {
                if (db.CIR_PATRON.Where(a => a.Code == patronCode).Count() < 1)
                {
                    if(!String.IsNullOrWhiteSpace(patronCode) && !String.IsNullOrEmpty(patronCode))
                    {
                        listCanNotDel.Add(new CustomPatron()
                        {
                            strCode = patronCode,
                            Name = "Số thẻ không có trong hệ thống"
                        });
                    }
                    
                }
                else
                {
                    listDel.Add(patronCode);
                }
            }

            foreach(var patronCode in listDel)
            {
                string id = db.CIR_PATRON.Where(a => a.Code == patronCode).First().ID+"";
                db.SP_PATRON_BATCH_DELETE(id);
                if (db.CIR_PATRON.Where(a => a.Code == patronCode).Count() > 0)
                {
                    listCanNotDel.Add(new CustomPatron()
                    {
                        strCode = patronCode,
                        Name = db.CIR_PATRON.Where(a => a.Code == patronCode).First().FirstName +" "+ db.CIR_PATRON.Where(a => a.Code == patronCode).First().LastName
                    });
                }
            }
            List<string[]> data = new List<string[]>();
            for(int i = 0; i < listCanNotDel.Count; i++)
            {
                string[] d = { listCanNotDel[i].strCode, listCanNotDel[i].Name };
                data.Add(d);
                
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthAttribute(ModuleID = 2, RightID = "34")]
        public ActionResult AdjournPatronExpiredDateByList()
        {
            return View();
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 2, RightID = "34")]
        public JsonResult AdjournPatronExpiredDate(string strPatronCodes,string newExpiredDate)
        {
            DateTime expiredDate = DateTime.Parse(newExpiredDate);
            List<CustomPatron> listCanNotAdjourn = new List<CustomPatron>();
            List<string> listAdjourn = new List<string>();
            foreach (var patronCode in strPatronCodes.Split('\n'))
            {
                if (db.CIR_PATRON.Where(a => a.Code == patronCode).Count() < 1)
                {
                    if (!String.IsNullOrWhiteSpace(patronCode) && !String.IsNullOrEmpty(patronCode))
                    {
                        listCanNotAdjourn.Add(new CustomPatron()
                        {
                            strCode = patronCode,
                            Name = "Số thẻ không có trong hệ thống"
                        });
                    }

                }
                else
                {
                    listAdjourn.Add(patronCode);
                }
            }

            foreach (var patronCode in listAdjourn)
            {
                db.CIR_PATRON.Where(a => a.Code == patronCode).First().ExpiredDate = expiredDate;
            }
            db.SaveChanges();
            List<string[]> data = new List<string[]>();
            for (int i = 0; i < listCanNotAdjourn.Count; i++)
            {
                string[] d = { listCanNotAdjourn[i].strCode, listCanNotAdjourn[i].Name };
                data.Add(d);

            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }

    
    class Result
    {
        public int CodeError { get; set; }
        public string Data { get; set; }
    }

    public class PatronFile
    {
        public int Line { get; set; }
        public string strCode { get; set; }
        public string FullName { get; set; }
        public string blnSex { get; set; }
        public DateTime strDOB { get; set; }
        public string strEmail { get; set; }
        public string strAddress { get; set; }
        public string Faculty { get; set; }
        public string strMobile { get; set; }
        public string strGrade { get; set; }
        public string College { get; set; }
        public string strCity { get; set; }
        public string strClass { get; set; }
        public string PatronGroup { get; set; }
        private bool isValid = true;
        public bool IsValid
        {
            get
            {
                
                if (String.IsNullOrEmpty(strCode) ||
                    String.IsNullOrEmpty(FullName) ||
                    String.IsNullOrEmpty(blnSex) ||
                    String.IsNullOrEmpty(strEmail) ||
                    String.IsNullOrEmpty(strAddress) ||
                    String.IsNullOrEmpty(Faculty) ||
                    String.IsNullOrEmpty(strMobile) ||
                    String.IsNullOrEmpty(strGrade) ||
                    String.IsNullOrEmpty(College) ||
                    String.IsNullOrEmpty(strCity) ||
                    String.IsNullOrEmpty(strClass) ||
                    String.IsNullOrEmpty(PatronGroup))
                {
                    isValid = false;
                }

                
                return isValid;
            }
            set
            {
                isValid = value;
            }
            
        }
    }

    public class AddDictionaryResult
    {
        public string Field { get; set; }
        public List<DictionarySelection> ListSelection { get; set; }
    }

    public class DictionarySelection
    {
        public int ID { get; set; }
        public string Data { get; set; }
    }

    public class CustomPatron
    {
        public int ID { get; set; }
        public string strCode { get; set; }
        public string Name { get; set; }
        public string strDOB { get; set; }
        public string strLastIssuedDate { get; set; }
        public string strExpiredDate { get; set; }
        public string Sex { get; set; }
        public string intEthnicID { get; set; }
        public string intCollegeID { get; set; }
        public string intFacultyID { get; set; }
        public string strGrade { get; set; }
        public string strClass { get; set; }
        public string strAddress { get; set; }
        public string strTelephone { get; set; }
        public string strMobile { get; set; }
        public string strEmail { get; set; }
        public string strNote { get; set; }
        public string intOccupationID { get; set; }
        public string intPatronGroupID { get; set; }
    }
}
