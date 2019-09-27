using System;
using System.Collections.Generic;
using Libol.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlibUnitTest.FlibReportUnitTests
{
    [TestClass]
    public class UnitTest5
    {
        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully1()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1","","","","","","","");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully2()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "10", "10", "", "", "81");
            Assert.AreEqual(10, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully3()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "10", "10", "", "103", "81");
            Assert.AreEqual(10, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully4()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "10", "", "", "", "81");
            Assert.AreEqual(10, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully5()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "01/01/2018", "", "10", "10", "", "103", "81");
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully6()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "01/01/2015", "10", "10", "", "103", "81");
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully7()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "01/01/2018", "02/02/2019", "10", "10", "", "103", "81");
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully8()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "01/01/2018", "02/02/2019", "10", "10", "", "", "81");
            Assert.AreEqual(10, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Successfully9()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "01/01/2015", "10", "10", "", "", "81");
            Assert.AreEqual(10, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail1()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "", "", "", "", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail2()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "", "", "", "103", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail3()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "", "10", "", "", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail4()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "", "10", "", "103", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail5()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "01/01/2018", "01/01/2017", "10", "10", "", "", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail6()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "01/01/2018", "01/01/2017", "10", "10", "", "103", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail7()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "01/01/2017", "01/01/2018", "10", "10", "", "-103", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail8()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "01/01/2017", "01/01/2018", "10", "10", "", "103", "-81");
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail9()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "10", "-10", "", "103", "81");
            Assert.AreEqual(10, actual.Count);
        }

        [TestMethod]
        public void FPT_SP_STAT_PATRONMAX_LIST_Fail10()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<FPT_SP_STAT_PATRONMAX_Result> actual = ab.FPT_SP_STAT_PATRONMAX_LIST("1", "", "", "10", "-10", "", "", "81");
            Assert.AreEqual(10, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Successfully1()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1","","","1","81");
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Successfully2()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "", "", "0", "81");
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Successfully3()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "01/01/2018", "", "0", "81");
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Successfully4()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "", "01/01/2018", "1", "81");
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Successfully5()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "01/01/2017", "01/01/2018", "1", "81");
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Successfully6()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "01/01/2017", "01/01/2018", "0", "81");
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Fail1()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "", "", "0", "0");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Fail2()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "", "", "1", "0");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Fail3()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "", "", "1", "-81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Fail4()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "", "", "0", "-81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Fail5()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "01/01/2018", "01/01/2017", "1", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_NOW_Fail6()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_NOW("1", "01/01/2018", "01/01/2017", "0", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Successfully1()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "", "", "1", "81");
            Assert.AreEqual(17, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Successfully2()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "", "", "0", "81");
            Assert.AreEqual(17, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Successfully3()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "01/01/2018", "", "0", "81");
            Assert.AreEqual(8, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Successfully4()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "", "01/01/2018", "1", "81");
            Assert.AreEqual(16, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Successfully5()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "01/01/2017", "01/01/2018", "1", "81");
            Assert.AreEqual(9, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Successfully6()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "01/01/2017", "01/01/2018", "0", "81");
            Assert.AreEqual(9, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Fail1()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "", "", "0", "0");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Fail2()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "", "", "1", "0");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Fail3()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "", "", "1", "-81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Fail4()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "", "", "0", "-81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Fail5()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "01/01/2018", "01/01/2017", "1", "81");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void PATRON_GROUP_PASS_Fail6()
        {
            // Arrange
            PatronBusiness ab = new PatronBusiness();
            // Act
            List<PATRON_GROUP> actual = ab.PATRON_GROUP_PASS("1", "01/01/2018", "01/01/2017", "0", "81");
            Assert.AreEqual(0, actual.Count);
        }

        //[TestMethod]
        //public void TOP_COPY_Successfully1()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1","","","0","0","0");
        //    Assert.AreEqual(0, actual.Count);
        //}
        
        //[TestMethod]
        //public void TOP_COPY_Successfully2()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "01/01/2018", "", "10", "0", "81");
        //    Assert.AreEqual(10, actual.Count);
        //}

        //[TestMethod]
        //public void TOP_COPY_Successfully3()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "01/01/2018", "", "10", "10", "81");
        //    Assert.AreEqual(10, actual.Count);
        //}

        //[TestMethod]
        //public void TOP_COPY_Successfully4()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "", "01/01/2018", "10", "10", "81");
        //    Assert.AreEqual(10, actual.Count);
        //}

        //[TestMethod]
        //public void TOP_COPY_Successfully5()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "01/01/2018", "01/01/2019", "10", "10", "81");
        //    Assert.AreEqual(10, actual.Count);
        //}

        //[TestMethod]
        //public void TOP_COPY_Fail1()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "", "", "0", "0", "0");
        //    Assert.AreEqual(0, actual.Count);
        //}

        //[TestMethod]
        //public void TOP_COPY_Fail2()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "", "", "0", "0", "-81");
        //    Assert.AreEqual(0, actual.Count);
        //}

        //[TestMethod]
        //public void TOP_COPY_Fail3()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "", "", "0", "-10", "81");
        //    Assert.AreEqual(0, actual.Count);
        //}

        //[TestMethod]
        //public void TOP_COPY_Fail4()
        //{
        //    // Arrange
        //    PatronBusiness ab = new PatronBusiness();
        //    // Act
        //    List<ITEMMAX> actual = ab.TOP_COPY("1", "01/01/2019", "01/01/2018", "10", "10", "81");
        //    Assert.AreEqual(0, actual.Count);
        //}
    }
}
