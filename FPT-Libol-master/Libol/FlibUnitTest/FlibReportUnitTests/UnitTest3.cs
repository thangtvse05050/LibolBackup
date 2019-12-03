using System;
using System.Collections.Generic;
using Libol.EntityResult;
using Libol.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibReportUnitTests
{
    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 81, "GT/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(6, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 81, "TK/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 81, "0", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(6, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 0, "0", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(17, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 0, "0", "", "01/01/2015", "01/01/2019", "", "", "", 1);
            // Assert
            Assert.AreEqual(14, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 0, "0", "", "", "", "01/01/2015", "01/01/2019", "", 1);
            // Assert
            Assert.AreEqual(17, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("", "FPT070013581", "", 0, "0", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(95, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_Successfully8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("", "", "TK/CNTT000038", 0, "0", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(21, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", -81, "TK/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 81, "-TK", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("-SE04480", "", "", 81, "TK/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 81, "TK/", "-1", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 0, "0", "", "01/01/2019", "01/01/2015", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("SE04480", "", "", 0, "0", "", "", "", "01/01/2019", "01/01/2015", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("", "-FPT070013581", "", 0, "0", "", "", "", "01/01/2019", "01/01/2015", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_LOAN_INFOR_LIST_UT_Fail8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_LOANINFOR_Result> actual = cb.GET_PATRON_LOAN_INFOR_LIST("", "", "-TK/CNTT000038", 0, "0", "", "", "", "01/01/2019", "01/01/2015", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("SE01989", "", "", 0, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(155, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "TK/CNTT000038", 0, "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "FPT070013581", "", 0, "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(81, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "", 81, "TK/", "103", "01/01/2019", "01/08/2019", "", "", 1);
            // Assert
            Assert.AreEqual(20, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "", 81, "TK/", "103", "", "", "01/01/2015", "01/08/2015", 1);
            // Assert
            Assert.AreEqual(14, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("se04477", "", "", 81, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(11, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "FPT070013581", "", 81, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(58, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Successfully8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "TK/XHHL003318", 81, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("SE04477", "", "", 81, "-TK", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("-SE04477", "", "", 81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("SE04477", "", "", -81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "AAAAAA", "", 81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "AAAAAA", 81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "", 81, "TK/", "-103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "", 81, "TK/", "103", "01/01/2019", "01/01/2018", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_LOAN_INFOR_LIST_Fail8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_LOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_LOAN_INFOR_LIST("", "", "", 81, "TK/", "103", "", "", "01/01/2019", "01/01/2018", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("HE140133", "", "", 81, "GT/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(6, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("SE04480", "", "", 81, "TK/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("HE140133", "", "", 81, "0", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(6, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_Successfully4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("HE140133", "", "", 0, "", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(7, actual.Count);
        }        

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_Successfully7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("", "FPT070013581", "", 0, "", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_Successfully8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("", "", "GT/TNHL000925", 0, "", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("SE04480", "", "", -81, "TK/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("SE04480", "", "", 81, "-TK", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("-SE04480", "", "", 81, "TK/", "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("SE04480", "", "", 81, "TK/", "-1", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("SE04480", "", "", 0, "0", "", "01/01/2019", "01/01/2015", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("SE04480", "", "", 0, "0", "", "", "", "01/01/2019", "01/01/2015", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("", "-FPT070013581", "", 0, "0", "", "", "", "01/01/2019", "01/01/2015", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_ONLOAN_INFOR_LIST_UT_Fail8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_ONLOANINFOR_Result> actual = cb.GET_PATRON_ONLOAN_INFOR_LIST("", "", "-TK/CNTT000038", 0, "0", "", "", "", "01/01/2019", "01/01/2015", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("HE140133", "", "", 0, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "", "GT/TNHL000925", 0, "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "FPT070013581", "", 0, "", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Successfully4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "", "", 81, "TK/", "103", "01/01/2018", "01/08/2019", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }


        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Successfully6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("se04477", "", "", 81, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Successfully7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "FPT070013581", "", 81, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Successfully8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "", "GT/TNHL000921", 81, "0", "", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(4, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("SE04477", "", "", 81, "-TK", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("-SE04477", "", "", 81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("SE04477", "", "", -81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Fail4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "AAAAAA", "", 81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Fail5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "", "AAAAAA", 81, "TK/", "103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Fail6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "", "", 81, "TK/", "-103", "", "", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_PATRON_RENEW_ONLOAN_INFOR_LIST_Fail7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<GET_PATRON_RENEW_ONLOAN_INFOR_Result> actual = cb.GET_PATRON_RENEW_ONLOAN_INFOR_LIST("", "", "", 81, "TK/", "103", "01/01/2019", "01/01/2018", "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81,103,1,1,"","",1);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 0, 1, 1, "", "", 1);
            // Assert
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(0, 0, 1, 1, "", "", 1);
            // Assert
            Assert.AreEqual(8, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 0, 2, 1, "", "", 1);
            // Assert
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 103, 2, 1, "", "", 1);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(0, 0, 2, 1, "", "", 1);
            // Assert
            Assert.AreEqual(8, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully7()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(0, 0, 3, 1, "", "", 1);
            // Assert
            Assert.AreEqual(8, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully8()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 0, 3, 1, "", "", 1);
            // Assert
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully9()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 103, 3, 1, "", "", 1);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully10()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 103, 3, 1, "2018", "2019", 1);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Successfully11()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(0, 0, 1, 1, "", "", 1);
            // Assert
            Assert.AreEqual(8, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(-81, 0, 3, 1, "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, -103, 3, 1, "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 103, -3, 1, "", "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Fail4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 103, 3, -1, "", "", 1);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_YEAR_STATISTIC_LIST_Fail5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_YEAR_STATISTIC_Result> actual = cb.GET_FPT_CIR_YEAR_STATISTIC_LIST(81, 103, 3, 1, "2019", "2018", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(0, 0, 1, 1, "", 1);
            // Assert
            Assert.AreEqual(12, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 0, 1, 1, "", 1);
            // Assert
            Assert.AreEqual(12, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 1, 1, "", 1);
            // Assert
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Successfully4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 2, 1, "", 1);
            // Assert
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Successfully5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 3, 1, "", 1);
            // Assert
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Successfully6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 1, 1, "2019", 1);
            // Assert
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(-81, 103, 1, 1, "", 1);
            // Assert
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, -103, 1, 1, "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 11, 1, "", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Fail4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 1, 11, "", 1);
            // Assert
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Fail5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 1, 1, "1000", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_FPT_CIR_MONTH_STATISTIC_LIST_Fail6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_MONTH_STATISTIC_Result> actual = cb.GET_FPT_CIR_MONTH_STATISTIC_LIST(81, 103, 1, 11, "3000", 1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "","","",0);
            // Assert
            Assert.AreEqual(9175, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("SE04477", "", "", "", 0);
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "qh", "", "", 0);
            // Assert
            Assert.AreEqual(933, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Successfully4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "", "07/07/2019", "", 0);
            // Assert
            Assert.AreEqual(121, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Successfully5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "", "", "01/01/2009", 0);
            // Assert
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Successfully6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "", "", "", 59);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("1", "1", "01/01/2010", "01/01/2011", 0);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("-SE04477", "", "", "", 0);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "2313qh", "", "", 0);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Fail4()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "", "07/07/2219", "", 0);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Fail5()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "", "", "01/01/2209", 0);
            // Assert
            Assert.AreEqual(9175, actual.Count);
        }

        [TestMethod]
        public void GET_SP_GET_LOCKEDPATRONS_LIST_Fail6()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<SP_GET_LOCKEDPATRONS_Result> actual = cb.GET_SP_GET_LOCKEDPATRONS_LIST("", "", "", "", -59);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> actual = cb.FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST("");
            // Assert
            Assert.AreEqual(64718, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> actual = cb.FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST("TK/TKBT000032");
            // Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> actual = cb.FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST("TK/TKBT000031");
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_Result> actual = cb.FPT_GET_LIQUIDBOOKS_BY_COPYNUMBER_LIST("1100032");
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_CIR_SP_STAT_TOP20_LIST_Successfully1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_SP_STAT_TOP20_Result> actual = cb.FPT_CIR_SP_STAT_TOP20_LIST(1,1,1,81);
            // Assert
            Assert.AreEqual(20, actual.Count);
        }

        [TestMethod]
        public void FPT_CIR_SP_STAT_TOP20_LIST_Successfully2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_SP_STAT_TOP20_Result> actual = cb.FPT_CIR_SP_STAT_TOP20_LIST(0, 1, 1, 81);
            // Assert
            Assert.AreEqual(20, actual.Count);
        }

        [TestMethod]
        public void FPT_CIR_SP_STAT_TOP20_LIST_Successfully3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_SP_STAT_TOP20_Result> actual = cb.FPT_CIR_SP_STAT_TOP20_LIST(1, 1, 1, 0);
            // Assert
            Assert.AreEqual(20, actual.Count);
        }

        [TestMethod]
        public void FPT_CIR_SP_STAT_TOP20_LIST_Fail1()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_SP_STAT_TOP20_Result> actual = cb.FPT_CIR_SP_STAT_TOP20_LIST(-1, 1, 1, 0);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_CIR_SP_STAT_TOP20_LIST_Fail2()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_SP_STAT_TOP20_Result> actual = cb.FPT_CIR_SP_STAT_TOP20_LIST(1, -1, 1, 0);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_CIR_SP_STAT_TOP20_LIST_Fail3()
        {
            // Arrange
            CirculationBusiness cb = new CirculationBusiness();
            // Act
            List<FPT_CIR_SP_STAT_TOP20_Result> actual = cb.FPT_CIR_SP_STAT_TOP20_LIST(1, 1, 1, -1);
            // Assert
            Assert.AreEqual(0, actual.Count);
        }
    }
}
