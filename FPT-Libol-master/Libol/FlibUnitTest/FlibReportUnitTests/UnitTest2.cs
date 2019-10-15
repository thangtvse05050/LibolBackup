using System;
using System.Web.Mvc;
using Libol.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibReportUnitTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void AcquisitionIndexUT()
        {
            AcquireReportController controller = new AcquireReportController();
            // Act
            ViewResult result = controller.AcquisitionIndex() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AcquireStatisticIndexUT()
        {
            AcquireReportController controller = new AcquireReportController();
            // Act
            ViewResult result = controller.AcquireStatisticIndex() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetLanguageStatsUT()
        {
            AcquireReportController controller = new AcquireReportController();
            // Act
            PartialViewResult result = controller.GetLanguageStats("81") as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetLanguageStats");
        }

        [TestMethod]
        public void GetLiquidationStatsUT()
        {
            AcquireReportController controller = new AcquireReportController();
            // Act
            PartialViewResult result = controller.GetLiquidationStats("") as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetLiquidationStats");
        }

        [TestMethod]
        public void GetTop20StatsUT()
        {
            AcquireReportController controller = new AcquireReportController();
            // Act
            PartialViewResult result = controller.GetTop20Stats("1") as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetTop20Stats");
        }

        [TestMethod]
        public void GetStatTaskbarUT()
        {
            AcquireReportController controller = new AcquireReportController();
            // Act
            PartialViewResult result = controller.GetStatTaskbar("81","103","01/01/2018","01/01/2019") as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetStatTaskbar");
        }
    }
}
