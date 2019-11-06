using System;
using System.Web.Mvc;
using Libol.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibOrientationUnitTests
{
    [TestClass]
    public class PatronControllerTests
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            PatronController controller = new PatronController();
            // Act
            ViewResult result = controller.PatronProfile() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
