using OPAC.Dao;
using OPAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OPAC.Controllers
{
    public class InformationPatronController : Controller
    {
        private OpacEntities dbContext = new OpacEntities();

        public ActionResult PatronAfterLoginPage()
        {
            var patron = (SP_OPAC_CHECK_PATRON_CARD_Result)Session["Info"];

            return View(patron);
        }

        public ActionResult BookBorrowingPage()
        {
            if (Session["OnHolding"] == null)
            {
                return View();
            }

            string studentCode = Session["OnHolding"].ToString();
            var listBookOnHolding = dbContext.SP_OPAC_GET_ONHOLDING(studentCode).ToList();
            string[] specialCharacterList = { "$a", "$b", "$c", "$p", "$e", "$n" };
            foreach (var item in listBookOnHolding)
            {
                foreach (var specialCharacter in specialCharacterList)
                {
                    item.Content = item.Content.Replace(specialCharacter, 
                        item.Content.Contains(specialCharacter[0]) ? "" : " ");
                }

                item.CODate = item.CODate.Replace("0:0", "").Trim();
                item.CIDate = item.CIDate.Replace("0:0", "").Trim();
            }

            return View(listBookOnHolding);
        }
    }
}