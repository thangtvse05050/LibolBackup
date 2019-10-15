using System;
using System.Transactions;
using System.Web.Mvc;
using Libol.Controllers;
using Libol.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibOrientationUnitTests
{
    [TestClass]
    public class CheckOutControllerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                CheckOutController checkOutController = new CheckOutController();
                PartialViewResult result = checkOutController.CheckOutCardInfo("900047107");
                Assert.AreEqual("_showPatronInfo", result.ViewName);
            }

        }
        [TestMethod]
        public void TestMethod2()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                CheckOutController checkOutController = new CheckOutController();
                ViewResult result = checkOutController.Index("900047107") as ViewResult;
                Assert.AreEqual("", result.ViewName);
            }

        }
    }
}
