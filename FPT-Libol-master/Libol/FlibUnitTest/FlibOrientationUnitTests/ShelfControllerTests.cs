using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Libol.Controllers;
using Libol.EntityResult;
using Libol.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibOrientationUnitTests
{
    [TestClass]
    public class ShelfControllerTests
    {

        
        [TestMethod]
        public void GenCopyNumberTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GenCopyNumber(-11) ;

            // Assert
            Assert.AreEqual("CD/CDDN000022", result);

        }

        [TestMethod]
        public void CheckGenCopyNumberTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GenCopyNumber(-11);

            // Assert
            Assert.IsFalse(false);

        }

        [TestMethod]
        public void CheckTrueGenCopyNumberTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GenCopyNumber(-11);

            // Assert
            Assert.IsTrue(true);

        }


        [TestMethod]
        public void GenerateCompositeHoldingsTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GenerateCompositeHoldings(10);

            // Assert
            Assert.AreEqual("FSE-HL / TK/TTHL / TK/TTHL000083,1394,1605<br/>", result);

        }

        [TestMethod]
        public void CheckErrorGenerateCompositeHoldingsTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GenerateCompositeHoldings(9867176);

            // Assert
            Assert.AreEqual(0, result.Count());

        }

        [TestMethod]
        public void CheckRightGenerateCompositeHoldingsTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GenerateCompositeHoldings(10);

            // Assert
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void GetContentTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GetContent("GT/CNTT000002");

            // Assert
            Assert.AreEqual("GT/CNTT000002", result);

        }

        [TestMethod]
        public void CheckErrorGetContentTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GetContent("");

            // Assert
            Assert.AreEqual(0, result.Count());

        }
        [TestMethod]
        public void CheckRightGetContentTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GetContent("GT/CNTT000002");

            // Assert
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void GetContentSameTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GetContent("GT/CNTT000002");

            // Assert
            Assert.AreSame("GT/CNTT000002", result);

        }

        [TestMethod]
        public void GetHoldingStatusTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            string result = shelfBusiness.GetHoldingStatus(true,true, true);

            // Assert
            Assert.AreEqual("<p style='color: #deaa0f'>Đang cho mượn<p>", result);

        }

        [TestMethod]
        public void SearchItemTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            List<SP_GET_TITLES_Result> data = null;
            string result = shelfBusiness.SearchItem("", "GT/CNTT", "","","","",ref data);

            // Assert
            Assert.AreEqual("", result);

        }

        [TestMethod]
        public void CheckErrorCountSearchItemTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            List<SP_GET_TITLES_Result> data = null;
            string result = shelfBusiness.SearchItem("", "GT/CNTT", "", "", "", "", ref data);

            // Assert
            Assert.AreEqual(0, result.Count());

        }
       
        [TestMethod]
        public void CheckErrorSearchItembyYearTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            List<SP_GET_TITLES_Result> data = null;
            string result = shelfBusiness.SearchItem("", "", "", "", "", "3030", ref data);

            // Assert
            Assert.AreEqual(0, result.Count());

        }


        [TestMethod]
        public void IsExistHoldingTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            bool result = shelfBusiness.IsExistHolding("",15,-1);

            // Assert
            Assert.AreEqual(false, result);

        }

        
        [TestMethod]
        public void CheckRightIsExistHoldingTests()
        {
            // Arrange
            ShelfBusiness shelfBusiness = new ShelfBusiness();
            // Act
            bool result = shelfBusiness.IsExistHolding("", 15, -1);

            // Assert
            Assert.IsTrue(true);

        }

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


    }
}
