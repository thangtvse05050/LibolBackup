using System;
using System.Web.Mvc;
using Libol.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibReportUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void GetLoanStatsUT()
        {
            // Arrange
            CirculationController controller = new CirculationController();
            // Act
            PartialViewResult result = controller.GetLoanStats() as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetLoanStats");
        }

        [TestMethod]
        public void GetFilteredLoanStatsUT()
        {
            // Arrange
            CirculationController controller = new CirculationController();
            // Act
            PartialViewResult result = controller.GetFilteredLoanStats() as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetFilteredLoanStats");
        }               

        [TestMethod]
        public void GetOnLoanStatsUT()
        {
            // Arrange
            CirculationController controller = new CirculationController();
            // Act
            PartialViewResult result = controller.GetOnLoanStats() as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetOnLoanStats");
        }

        [TestMethod]
        public void GetFilteredOnLoanStatsUT()
        {
            // Arrange
            CirculationController controller = new CirculationController();
            // Act
            PartialViewResult result = controller.GetFilteredOnLoanStats() as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetFilteredOnLoanStats");
        }

        [TestMethod]
        public void CopyNumberLiquidationStatsUT()
        {
            // Arrange
            CirculationController controller = new CirculationController();
            // Act
            ViewResult result = controller.CopyNumberLiquidationStats() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCopyNumberLiquidationStatsUT()
        {
            // Arrange
            CirculationController controller = new CirculationController();
            // Act
            PartialViewResult result = controller.GetCopyNumberLiquidationStats("TK/FAT1000001") as PartialViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "GetCopyNumberLiquidationStats");
        }
    }
}
