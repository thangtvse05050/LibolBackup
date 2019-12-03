using System;
using System.Web.Mvc;
using Libol.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibOrientationUnitTests
{
    [TestClass]
    public class CheckInControllerTests
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            CheckInController controller = new CheckInController();
            // Act
            ViewResult result = controller.Index("") as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
