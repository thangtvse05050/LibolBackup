using System;
using System.Collections.Generic;
using Libol.EntityResult;
using Libol.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibReportUnitTests
{
    [TestClass]
    public class UnitTest4
    {
        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("",0,0,"","",1);
            // Assert
            Assert.AreEqual(64718, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("TL/HCM-Q3-2008", 0, 0, "", "", 1);
            // Assert
            Assert.AreEqual(147, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 81, 0, "", "", 1);
            // Assert
            Assert.AreEqual(5580, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 81, 103, "", "", 1);
            // Assert
            Assert.AreEqual(638, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully5()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 0, 0, "01/01/2018", "01/01/2019", 1);
            // Assert
            Assert.AreEqual(2077, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully6()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 81, 0, "01/01/2018", "01/01/2019", 1);
            // Assert
            Assert.AreEqual(831, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully7()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 81, 0, "01/01/2018", "", 1);
            // Assert
            Assert.AreEqual(1577, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Successfully8()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 81, 0, "", "01/01/2018", 1);
            // Assert
            Assert.AreEqual(4003, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Fail1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("109101", 0, 0, "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Fail2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", -1, 0, "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Fail3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 81, -1, "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_LIST_Fail4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_Result> actual = ab.FPT_GET_LIQUIDBOOKS_LIST("", 81, 103, "01/01/2019", "01/01/2018", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Successfully1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(0,0,"","",1);
            Assert.AreEqual(13, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Successfully2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(81, 0, "", "", 1);
            Assert.AreEqual(11, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Successfully3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(81, 103, "", "", 1);
            Assert.AreEqual(8, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Successfully4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(0, 0, "2017", "", 1);
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Successfully5()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(0, 0, "", "2010", 1);
            Assert.AreEqual(4, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Successfully6()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(0, 0, "2010", "2015", 1);
            Assert.AreEqual(6, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Fail1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(-1, 0, "", "", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Fail2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(81, -1, "", "", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Fail3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(81, 103, "2019", "2018", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Fail4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(81, 0, "2019", "2018", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_YEAR_STATISTIC_LIST_Fail5()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_YEAR_STATISTIC_Result> actual = ab.FPT_ACQ_YEAR_STATISTIC_LIST(0, 0, "2019", "2018", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Successfully1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(0, 0, "", 1);
            Assert.AreEqual(12, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Successfully2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 0, "", 1);
            Assert.AreEqual(12, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Successfully3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 103, "", 1);
            Assert.AreEqual(11, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Successfully4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 103, "2018", 1);
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Successfully5()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 0, "2018", 1);
            Assert.AreEqual(12, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Successfully6()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(0, 0, "2018", 1);
            Assert.AreEqual(12, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(-1, 0, "", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, -1, "", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(0, 0, "1999", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 0, "1999", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail5()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 103, "1999", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail6()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(0, 0, "2999", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail7()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 0, "2999", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_ACQ_MONTH_STATISTIC_LIST_Fail8()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_ACQ_MONTH_STATISTIC_Result> actual = ab.FPT_ACQ_MONTH_STATISTIC_LIST(81, 103, "2999", 1);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Successfully1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("","",0,81);
            Assert.AreEqual(5469, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Successfully2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("", "", 103, 81);
            Assert.AreEqual(1352, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Successfully3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("01/01/2018", "", 0, 81);
            Assert.AreEqual(542, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Successfully4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("", "01/01/2018", 0, 81);
            Assert.AreEqual(4927, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Successfully5()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("01/01/2017", "01/01/2018", 0, 81);
            Assert.AreEqual(377, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Fail1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("", "", 0, -81);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Fail2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("", "", -10, 81);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Fail3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("01/01/2019", "01/01/2018", 0, 81);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_GET_ITEM_LIST_Fail4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_SP_GET_ITEM_Result> actual = ab.FPT_SP_GET_ITEM_LIST("01/01/2019", "01/01/2018", 103, 81);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST_Successfully1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> actual = ab.FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(51,0,0);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST_Successfully2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> actual = ab.FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(514, 0, 81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST_Successfully3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> actual = ab.FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(51, 103, 81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST_Fail1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> actual = ab.FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(-51, 0, 0);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST_Fail2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> actual = ab.FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(514, 0, -81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST_Fail3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> actual = ab.FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(51, -103, 81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST_Fail4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_BY_ITEMID_Result> actual = ab.FPT_COUNT_COPYNUMBER_BY_ITEMID_LIST(0, 0, 0);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void SP_GET_ITEM_INFOR_LIST_Successfully1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<SP_GET_ITEM_INFOR_Result> actual = ab.SP_GET_ITEM_INFOR_LIST(515);
            Assert.AreEqual(7, actual.Count);
        }

        [TestMethod]
        public void SP_GET_ITEM_INFOR_LIST_Successfully2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<SP_GET_ITEM_INFOR_Result> actual = ab.SP_GET_ITEM_INFOR_LIST(513);
            Assert.AreEqual(8, actual.Count);
        }

        [TestMethod]
        public void SP_GET_ITEM_INFOR_LIST_Fail1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<SP_GET_ITEM_INFOR_Result> actual = ab.SP_GET_ITEM_INFOR_LIST(0);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void SP_GET_ITEM_INFOR_LIST_Fail2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<SP_GET_ITEM_INFOR_Result> actual = ab.SP_GET_ITEM_INFOR_LIST(-10);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_ONLOAN_LIST_Successfully1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> actual = ab.FPT_COUNT_COPYNUMBER_ONLOAN_LIST(51, 0, 0);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_ONLOAN_LIST_Successfully2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> actual = ab.FPT_COUNT_COPYNUMBER_ONLOAN_LIST(514, 0, 81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_ONLOAN_LIST_Successfully3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> actual = ab.FPT_COUNT_COPYNUMBER_ONLOAN_LIST(51, 103, 81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_ONLOAN_LIST_Fail1()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> actual = ab.FPT_COUNT_COPYNUMBER_ONLOAN_LIST(-51, 0, 0);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_ONLOAN_LIST_Fail2()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> actual = ab.FPT_COUNT_COPYNUMBER_ONLOAN_LIST(514, 0, -81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_ONLOAN_LIST_Fail3()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> actual = ab.FPT_COUNT_COPYNUMBER_ONLOAN_LIST(51, -103, 81);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_COUNT_COPYNUMBER_ONLOAN_LIST_Fail4()
        {
            // Arrange
            AcquisitionBusiness ab = new AcquisitionBusiness();
            // Act
            List<FPT_COUNT_COPYNUMBER_ONLOAN_Result> actual = ab.FPT_COUNT_COPYNUMBER_ONLOAN_LIST(0, 0, 0);
            Assert.AreEqual(1, actual.Count);
        }
    }
}
