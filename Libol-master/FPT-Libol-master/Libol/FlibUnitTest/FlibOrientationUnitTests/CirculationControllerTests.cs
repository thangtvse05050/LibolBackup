using System;
using System.Web.Mvc;
using Libol.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibOrientationUnitTests
{
    [TestClass]
    public class CirculationControllerTests
    {
        [TestMethod]
        public void GetJsonResultByRightLocidTests()
        {
            // Arrange
            ShelfController controller = new ShelfController();
            // Act
            JsonResult result = controller.GenCopyNumber(49) as JsonResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetJsonResultByWrongLocidTests()
        {
            // Arrange
            ShelfController controller = new ShelfController();
            // Act
            JsonResult result = controller.GenCopyNumber(-10) as JsonResult;
            // Assert
            Assert.AreEqual(result.Data.ToString(),"");
        }
    }
}
