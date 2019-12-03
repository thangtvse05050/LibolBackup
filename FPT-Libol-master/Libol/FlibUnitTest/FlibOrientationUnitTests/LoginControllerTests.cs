using System;
using System.Transactions;
using System.Web.Mvc;
using Libol.Controllers;
using Libol.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibOrientationUnitTests
{
    

    [TestClass]
    public class LoginControllerTests
    {

        [TestMethod]
        public void Index()
        {
            // Arrange
            LoginController controller = new LoginController();
            // Act
            ViewResult result = controller.Index("Nhatnh","abc") as ViewResult;
            // Assert
            Assert.AreEqual(result.ViewData["Notification"], "Tên đăng nhập/mật khẩu không đúng!");
        }
    }

}
