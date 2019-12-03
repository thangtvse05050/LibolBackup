using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libol.Models;
using Libol.SupportClass;
using XCrypt;

namespace Libol.Controllers
{
    public class ManagementController : Controller
    {
        private LibolEntities db = new LibolEntities();

        [AuthAttribute(ModuleID = 6, RightID = "0")]
        public ActionResult Account(string username)
        {
            if (!String.IsNullOrEmpty(username))
            {
                if (db.SYS_USER.Where(a => a.Username == username).Count() > 0)
                {
                    SYS_USER user = db.SYS_USER.Where(a => a.Username == username).First();
                    ViewBag.GoogleAccount = db.SYS_USER_GOOGLE_ACCOUNT.Where(a => a.ID == user.ID).FirstOrDefault();
                    return View(user);
                }
                else
                {
                    return View(new SYS_USER());
                }
            }
            else
            {
                return View(new SYS_USER());
            }

        }

        [HttpPost]
        [AuthAttribute(ModuleID = 6, RightID = "0")]
        public JsonResult Account(DataTableAjaxPostModel model)
        {
            var users = db.SYS_USER;
            var search = users.Where(a => true);
            int CurrentUser = (int)Session["UserID"];
            if(CurrentUser != 1)
            {
                search = search.Where(a => a.ID != 1);
            }
            if (model.search.value != null)
            {
                string searchValue = model.search.value;
                search = search.Where(a => a.Username.Contains(searchValue) || a.Name.Contains(searchValue));
            }
            if (model.columns[1].search.value != null)
            {
                string searchValue = model.columns[1].search.value;
                search = search.Where(a => a.Username.Contains(searchValue));
            }
            if (model.columns[2].search.value != null)
            {
                string searchValue = model.columns[2].search.value;
                search = search.Where(a => a.Name.Contains(searchValue));
            }

            var sorting = search.OrderBy(a => a.ID);
            if (model.order[0].column == 2)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Username);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Username);
                }

            }
            else if (model.order[0].column == 3)
            {
                if (model.order[0].dir.Equals("asc"))
                {
                    sorting = search.OrderBy(a => a.Name);
                }
                else
                {
                    sorting = search.OrderByDescending(a => a.Name);
                }

            }
            var paging = sorting.Skip(model.start).Take(model.length).ToList();
            var result = new List<CustomUser>(paging.Count);
            foreach (var s in paging)
            {
                result.Add(new CustomUser
                {
                    ID = s.ID,
                    Name = s.Name,
                    Username = s.Username
                });
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = users.Count(),
                recordsFiltered = search.Count(),
                data = result
            });
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 6, RightID = "0")]
        public JsonResult AddNewUser(string Name, string Username, string Email, string Password, string RepeatPassword,
            int module1, int module2, int module3, int module4, int module5, int module8, int module9, int module6, string rights,
            string locRights1, string locRights2, string locRights3)
        {
            if (db.SYS_USER.Where(a => a.Username == Username).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Người dùng với tên đăng nhập <strong style='color:black; '>" + Username + "</strong> đã tồn tại!"
                }, JsonRequestBehavior.AllowGet);
            }
            if (!String.IsNullOrEmpty(Email) && db.SYS_USER_GOOGLE_ACCOUNT.Where(a => a.Email == Email).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Người dùng với email <strong style='color:black; '>" + Email + "</strong> đã tồn tại!"
                }, JsonRequestBehavior.AllowGet);
            }
            string InvalidFields = "";
            if (String.IsNullOrEmpty(Name))
            {
                InvalidFields += "txtName-";
            }
            //if (String.IsNullOrEmpty(Email))
            //{
            //    InvalidFields += "txtEmail-";
            //}
            if (String.IsNullOrEmpty(Username))
            {
                InvalidFields += "txtUsername-";
            }
            if (String.IsNullOrEmpty(Password))
            {
                InvalidFields += "txtPassword-";
            }
            if (!String.IsNullOrEmpty(Password) && Password != RepeatPassword)
            {
                InvalidFields += "txtRepeatPassword-";
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
            {
                string passEncrypt = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(Password, "pl");
                var intNewUID = new ObjectParameter("intNewUID", typeof(int));
                db.SP_ADMIN_ADD_USER(0,Name,Username, passEncrypt, module1, module2, module3, module4, module5, module8, module9, module6, 
                    Int32.Parse(Session["UserID"].ToString()),null, intNewUID, new ObjectParameter("intOutVal", typeof(int)));
                int ID = (int)intNewUID.Value;
                if (ID > 0)
                {
                    var rightsSplit = rights.Split(',');
                    foreach(var r in rightsSplit)
                    {
                        if (!String.IsNullOrEmpty(r))
                        {
                            db.FPT_SP_ADMIN_GRANT_RIGHTS(ID, Int32.Parse(r));
                        }
                        
                    }
                    var locRights1Split = locRights1.Split(',');
                    foreach (var r in locRights1Split)
                    {
                        if (!String.IsNullOrEmpty(r))
                        {
                            db.SP_ADMIN_GRANT_LOCATIONS(ID, Int32.Parse(r), 1);
                        }
                        
                    }
                    var locRights2Split = locRights2.Split(',');
                    foreach (var r in locRights2Split)
                    {
                        if (!String.IsNullOrEmpty(r))
                        {
                            db.SP_ADMIN_GRANT_LOCATIONS(ID, Int32.Parse(r), 2);
                        }
                        
                    }
                    var locRights3Split = locRights3.Split(',');
                    foreach (var r in locRights3Split)
                    {
                        if (!String.IsNullOrEmpty(r))
                        {
                            db.SP_ADMIN_GRANT_LOCATIONS(ID, Int32.Parse(r), 3);
                        }
                        
                    }
                    if (!String.IsNullOrEmpty(Email))
                    {
                        var userGoogleAccount = db.SYS_USER_GOOGLE_ACCOUNT.Create();
                        userGoogleAccount.ID = ID;
                        userGoogleAccount.Email = Email;
                        db.SYS_USER_GOOGLE_ACCOUNT.Add(userGoogleAccount);
                        db.SaveChanges();
                    }
                    
                    return Json(new Result()
                    {
                        CodeError = 0,
                        Data = "Tài khoản <strong style='color:black;'>" + Username + " </strong> đã được thêm mới thành công cho <strong style='color:black;'>" + Name + "</strong>"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new Result()
                    {
                        CodeError = 2,
                        Data = "Có lỗi vui lòng kiểm tra lại!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
                
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 6, RightID = "0")]
        public JsonResult UpdateUser(int ID, string Name, string Username, string Email, string Password, string RepeatPassword,
            int module1, int module2, int module3, int module4, int module5, int module8, int module9, int module6, string rights,
            string locRights1, string locRights2, string locRights3)
        {
            if (db.SYS_USER.Where(a => a.ID != ID).Where(a => a.Username == Username).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Người dùng với tên đăng nhập <strong style='color:black; '>" + Username + "</strong> đã tồn tại!"
                }, JsonRequestBehavior.AllowGet);
            }
            if (!String.IsNullOrEmpty(Email) && db.SYS_USER_GOOGLE_ACCOUNT.Where(a => a.ID != ID).Where(a => a.Email == Email).Count() > 0)
            {
                return Json(new Result()
                {
                    CodeError = 2,
                    Data = "Người dùng với email <strong style='color:black; '>" + Email + "</strong> đã tồn tại!"
                }, JsonRequestBehavior.AllowGet);
            }
            string InvalidFields = "";
            if (String.IsNullOrEmpty(Name))
            {
                InvalidFields += "txtName-";
            }
            //if (String.IsNullOrEmpty(Email))
            //{
            //    InvalidFields += "txtEmail-";
            //}
            if (String.IsNullOrEmpty(Username))
            {
                InvalidFields += "txtUsername-";
            }
            if (!String.IsNullOrEmpty(Password) && Password != RepeatPassword)
            {
                InvalidFields += "txtRepeatPassword-";
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
            {
                string passEncrypt = "";
                if (!String.IsNullOrEmpty(Password))
                {
                    passEncrypt = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(Password, "pl");
                }
                var intOutVal = new ObjectParameter("intOutVal", typeof(int));
                db.FPT_SP_ADMIN_UPDATE_USER(ID, 0, Name, Username, passEncrypt, module1, module2, module3, module4, module5, module8, module9, module6,
                    Int32.Parse(Session["UserID"].ToString()), intOutVal);
                var rightsSplit = rights.Split(',');
                foreach (var r in rightsSplit)
                {
                    if (!String.IsNullOrEmpty(r))
                    {
                        db.FPT_SP_ADMIN_GRANT_RIGHTS(ID, Int32.Parse(r));
                    }
                    
                }
                var locRights1Split = locRights1.Split(',');
                foreach (var r in locRights1Split)
                {
                    if (!String.IsNullOrEmpty(r))
                    {
                        db.SP_ADMIN_GRANT_LOCATIONS(ID, Int32.Parse(r), 1);
                    }
                    
                }
                var locRights2Split = locRights2.Split(',');
                foreach (var r in locRights2Split)
                {
                    if (!String.IsNullOrEmpty(r))
                    {
                        db.SP_ADMIN_GRANT_LOCATIONS(ID, Int32.Parse(r), 2);
                    }
                    
                }
                var locRights3Split = locRights3.Split(',');
                foreach (var r in locRights3Split)
                {
                    if (!String.IsNullOrEmpty(r))
                    {
                        db.SP_ADMIN_GRANT_LOCATIONS(ID, Int32.Parse(r), 3);
                    }
                    
                }
                if (!String.IsNullOrEmpty(Email))
                {
                    if (db.SYS_USER_GOOGLE_ACCOUNT.Where(a => a.ID == ID).Count() > 0)
                    {
                        var userGoogleAccountDel = db.SYS_USER_GOOGLE_ACCOUNT.Where(a => a.ID == ID).First();
                        db.Entry(userGoogleAccountDel).State = EntityState.Deleted;

                        var userGoogleAccount = db.SYS_USER_GOOGLE_ACCOUNT.Create();
                        userGoogleAccount.ID = ID;
                        userGoogleAccount.Email = Email;
                        db.SYS_USER_GOOGLE_ACCOUNT.Add(userGoogleAccount);
                    }
                    else
                    {

                        var userGoogleAccount = db.SYS_USER_GOOGLE_ACCOUNT.Create();
                        userGoogleAccount.ID = ID;
                        userGoogleAccount.Email = Email;
                        db.SYS_USER_GOOGLE_ACCOUNT.Add(userGoogleAccount);


                    }
                }
                    
                db.SaveChanges();
                return Json(new Result()
                {
                    CodeError = 0,
                    Data = "Tài khoản <strong style='color:black;'>" + Username + " </strong> đã được cập nhật thành công cho <strong style='color:black;'>" + Name + "</strong>"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthAttribute(ModuleID = 6, RightID = "0")]
        public JsonResult DeleteUser(string strUIDs)
        {
            try
            {
                int result = db.SP_ADMIN_DELETE_USER(strUIDs);
                if (result > 0)
                {
                    var IDs = strUIDs.Split(',');
                    foreach (var ID in IDs)
                    {
                        if (db.SYS_USER_GOOGLE_ACCOUNT.Where(a => a.ID.ToString() == ID).Count() > 0)
                        {
                            var googleAcc = db.SYS_USER_GOOGLE_ACCOUNT.Where(a => a.ID.ToString() == ID).First();
                            db.SYS_USER_GOOGLE_ACCOUNT.Remove(googleAcc);
                        }
                    }
                    db.SaveChanges();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [AuthAttribute(ModuleID = 6, RightID = "0")]
        public JsonResult GetRightInModule(int module, int UserID)
        {
            var locRights = db.SP_GETUSERLOCATIONS(1, Int32.Parse(Session["UserID"].ToString())).ToList();
            if (UserID > 0)
            {
                Rights rights = new Rights();

                rights.Accept = new List<FunctionRight>();
                rights.Deny = new List<FunctionRight>();

                if(UserID == 1)
                {
                    foreach (var r in db.FPT_ADMIN_GET_RIGHTS_ACCEPT(module, UserID).ToList())
                    {
                        rights.Accept.Add(new FunctionRight()
                        {
                            ID = r.ID,
                            Right = r.Right
                        });
                    }
                    foreach (var r in db.FPT_ADMIN_GET_RIGHTS_DENY_ADMIN(module).ToList())
                    {
                        rights.Deny.Add(new FunctionRight()
                        {
                            ID = r.ID,
                            Right = r.Right
                        });
                    }
                }
                else
                {
                    foreach (var r in db.FPT_ADMIN_GET_RIGHTS_ACCEPT(module, UserID).ToList())
                    {
                        rights.Accept.Add(new FunctionRight()
                        {
                            ID = r.ID,
                            Right = r.Right
                        });
                    }
                    foreach (var r in db.FPT_ADMIN_GET_RIGHTS_DENY(module, UserID, Int32.Parse(Session["UserID"].ToString())).ToList())
                    {
                        rights.Deny.Add(new FunctionRight()
                        {
                            ID = r.ID,
                            Right = r.Right
                        });
                    }
                }
                

                rights.AcceptLoc = new List<LocRight>();
                rights.DenyLoc = new List<LocRight>();

                var strCirLocs = new ObjectParameter("strCirLocs", typeof(string));
                var strAcqLocs = new ObjectParameter("strAcqLocs", typeof(string));
                var strSerLocs = new ObjectParameter("strSerLocs", typeof(string));
                db.SP_ADMIN_GET_LOCATION_INFOR(UserID, strCirLocs, strAcqLocs, strSerLocs);
                if(module == 3)
                {
                    foreach (var r in locRights)
                    {
                        bool checkDeny = true;
                        foreach (var l in strCirLocs.Value.ToString().Split(','))
                        {
                            if (l == r.ID.ToString())
                            {
                                checkDeny = false;
                            }
                        }
                        if (checkDeny)
                        {
                            rights.DenyLoc.Add(new LocRight()
                            {
                                ID = r.ID,
                                LocName = r.LOCNAME
                            });
                        }
                        else
                        {
                            rights.AcceptLoc.Add(new LocRight()
                            {
                                ID = r.ID,
                                LocName = r.LOCNAME
                            });
                        }
                    }
                }
                if (module == 4)
                {
                    foreach (var r in locRights)
                    {
                        bool checkDeny = true;
                        foreach (var l in strAcqLocs.Value.ToString().Split(','))
                        {
                            if (l == r.ID.ToString())
                            {
                                checkDeny = false;
                            }
                        }
                        if (checkDeny)
                        {
                            rights.DenyLoc.Add(new LocRight()
                            {
                                ID = r.ID,
                                LocName = r.LOCNAME
                            });
                        }
                        else
                        {
                            rights.AcceptLoc.Add(new LocRight()
                            {
                                ID = r.ID,
                                LocName = r.LOCNAME
                            });
                        }
                    }
                }
                if (module == 5)
                {
                    foreach (var r in locRights)
                    {
                        bool checkDeny = true;
                        foreach (var l in strSerLocs.Value.ToString().Split(','))
                        {
                            if (l == r.ID.ToString())
                            {
                                checkDeny = false;
                            }
                        }
                        if (checkDeny)
                        {
                            rights.DenyLoc.Add(new LocRight()
                            {
                                ID = r.ID,
                                LocName = r.LOCNAME
                            });
                        }
                        else
                        {
                            rights.AcceptLoc.Add(new LocRight()
                            {
                                ID = r.ID,
                                LocName = r.LOCNAME
                            });
                        }
                    }
                }
                return Json(rights, JsonRequestBehavior.AllowGet);
            }
            else
            {
                RightsWhenCreate rights = new RightsWhenCreate();
                rights.Accept = new List<FunctionRight>();
                rights.Deny = new List<FunctionRight>();

                foreach(var r in db.FPT_ADMIN_GET_RIGHTS_WHEN_CREATE(module, Int32.Parse(Session["UserID"].ToString()), 1).ToList())
                {
                    rights.Accept.Add(new FunctionRight()
                    {
                        ID = r.ID,
                        Right = r.Right
                    });
                }
                foreach (var r in db.FPT_ADMIN_GET_RIGHTS_WHEN_CREATE(module, Int32.Parse(Session["UserID"].ToString()), 0).ToList())
                {
                    rights.Deny.Add(new FunctionRight()
                    {
                        ID = r.ID,
                        Right = r.Right
                    });
                }
                rights.AcceptLoc = new List<LocRight>();
                rights.DenyLoc = new List<LocRight>();
                foreach(var l in locRights)
                {
                    rights.DenyLoc.Add(new LocRight() {
                        ID = l.ID,
                        LocName = l.LOCNAME
                    });
                }
                return Json(rights, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChangePassword(string oldPassword, string newPassword, string repeatNewPassword)
        {
            int CurrentUser = (int)Session["UserID"];
            if (oldPassword == null || oldPassword == "" || newPassword == null || newPassword == "" || repeatNewPassword == null || repeatNewPassword == "")
            {
                return View();
            }
            string oldPassEncrypt = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(oldPassword.Trim(), "pl");
            string newPassEncrypt = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(newPassword.Trim(), "pl");
            if (newPassword != repeatNewPassword)
            {
                ViewData["Notification"] = "Mật khẩu không khớp.";
                return View();
            }
            else
            {
                if (db.SYS_USER.Where(a => a.ID == CurrentUser).Where(a => a.Password == oldPassEncrypt).Count() > 0)
                {
                    SYS_USER sysUser = db.SYS_USER.Single(a => a.ID == CurrentUser);
                    sysUser.Password = newPassEncrypt;
                    db.SaveChanges();
                    ViewData["NotificationSuccess"] = "Thay đổi mật khẩu thành công!";
                    return View();
                }
                else
                {
                    ViewData["Notification"] = "Mật khẩu cũ không đúng.";
                    return View();
                }
            }
        }
    }
    public class CustomUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
    }

    public class Rights
    {
        public List<FunctionRight> Accept { get; set; }
        public List<FunctionRight> Deny { get; set; }
        public List<LocRight> AcceptLoc { get; set; }
        public List<LocRight> DenyLoc { get; set; }
    }

    public class RightsWhenCreate
    {
        public List<FunctionRight> Accept { get; set; }
        public List<FunctionRight> Deny { get; set; }
        public List<LocRight> AcceptLoc { get; set; }
        public List<LocRight> DenyLoc { get; set; }
    }

    public class FunctionRight
    {
        public int ID { get; set; }
        public string Right { get; set; }
    }

    public class LocRight
    {
        public int ID { get; set; }
        public string LocName { get; set; }
    }
    
}