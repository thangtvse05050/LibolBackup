using Libol.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FlibUnitTest.FlibOrientationUnitTests
{
    [TestClass]
    public class AcquisitionControllerTests
    {
        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LISTTests()
        {
            // Arrange
            AcquisitionBusiness business = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> result = business.FPT_GET_LIQUIDBOOKS_LIST("",0,0,"","",49);

            // Assert
            Assert.AreEqual(64722, result.Count());
        }


        [TestMethod]
        public void FPT_SP_GET_ITEM_INFOR_LISTTests()
        {
            // Arrange
            AcquisitionBusiness business = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_INFOR_Result> result = business.FPT_SP_GET_ITEM_INFOR_LIST(516,0,0);

            // Assert
            Assert.AreEqual(8, result.Count());
        }

        [TestMethod]
        public void CheckFPT_SP_GET_ITEM_INFOR_LISTTests()
        {
            // Arrange
            AcquisitionBusiness business = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_INFOR_Result> result = business.FPT_SP_GET_ITEM_INFOR_LIST(516, 20, 0);

            // Assert
            Assert.AreEqual(8, result.Count());
        }

        [TestMethod]
        public void CheckRightFPT_SP_GET_ITEM_INFOR_LISTTests()
        {
            // Arrange
            AcquisitionBusiness business = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_INFOR_Result> result = business.FPT_SP_GET_ITEM_INFOR_LIST(0, 15,81);

            // Assert
            Assert.AreEqual(2, result.Count());
        }
    }
}
