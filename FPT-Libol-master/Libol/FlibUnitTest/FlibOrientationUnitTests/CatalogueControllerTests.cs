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
    public class CatalogueControllerTests
    {
        [TestMethod]
        public void GetComplatedFormTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<GET_CATALOGUE_FIELDS_Result> result = business.GetComplatedForm(1, "", 12);

            // Assert
            Assert.AreEqual(21, result.Count());
        }

        [TestMethod]
        public void CheckErrorGetComplatedFormTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<GET_CATALOGUE_FIELDS_Result> result = business.GetComplatedForm(1, "", -10);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CheckErrorFormIDGetComplatedFormTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<GET_CATALOGUE_FIELDS_Result> result = business.GetComplatedForm(1, "Ahihi", 11);

            // Assert
            Assert.AreEqual(0, result.Count());
        }
        [TestMethod]
        public void GET_CATALOGUE_FIELDSTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<GET_CATALOGUE_FIELDS_Result> result = business.GET_CATALOGUE_FIELDS(1, 12, "020,020$a,040,040$a,041,041$a,044,044$a,082,082$a,090,090$a,100,100$a,100$e,110,110$a,110$b,245,245$a,245$b,245$c,245$n,245$p,246,246$a,246$b,250,250$a,260,260$a,260$b,260$c,300,300$a,300$b,300$c,300$e,490,490$a,500,500$a,520,520$a,650,650$a,653,653$a,700,700$a,700$e,700$n,852,852$a,852$p,900,911,925,926,927,", "",0);

            // Assert
            Assert.AreEqual(21, result.Count());
        }

        [TestMethod]
        public void CheckErrorFormIDGET_CATALOGUE_FIELDSTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<GET_CATALOGUE_FIELDS_Result> result = business.GET_CATALOGUE_FIELDS(1, 11, "020,020$a,040,040$a,041,041$a,044,044$a,082,082$a,090,090$a,100,100$a,100$e,110,110$a,110$b,245,245$a,245$b,245$c,245$n,245$p,246,246$a,246$b,250,250$a,260,260$a,260$b,260$c,300,300$a,300$b,300$c,300$e,490,490$a,500,500$a,520,520$a,650,650$a,653,653$a,700,700$a,700$e,700$n,852,852$a,852$p,900,911,925,926,927,", "", 0);

            // Assert
            Assert.AreEqual(21, result.Count());
        }

        [TestMethod]
        public void SearchCodeTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.SearchCode("FPT100016688", "","");

            // Assert
            Assert.AreEqual(21, result.Count());
        }

        [TestMethod]
        public void CheckErrorSearchCodeTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.SearchCode("FPT100016688abc", "", "");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CheckNullSearchCodeTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.SearchCode("", "", "");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CheckSearchCodeByTitleTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.SearchCode("", "", "Đắc nhân tâm");

            // Assert
            Assert.AreEqual(143, result.Count());
        }

        [TestMethod]
        public void CheckErrorSearchCodeByTitleTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.SearchCode("", "", "abcxyz");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CheckSearchCodeByCopyNumberTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.SearchCode("", "TK/DNCA000198", "");

            // Assert
            Assert.AreEqual(21, result.Count());
        }

        [TestMethod]
        public void CheckErrorSearchCodeByCopyNumberTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.SearchCode("", "abcxyz", "");

            // Assert
            Assert.AreEqual(0, result.Count());
        }
        [TestMethod]
        public void GetContentByIDTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> result = business.GetContentByID("516");

            // Assert
            Assert.AreEqual(20, result.Count());
        }

        [TestMethod]
        public void CheckErrorGetContentByIDTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> result = business.GetContentByID("51609090");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CheckNullGetContentByIDTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_CONTENTS_OF_ITEMS_Result> result = business.GetContentByID("");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CheckTitleTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.CheckTitle("Đắc nhân tâm");

            // Assert
            Assert.AreEqual(143, result.Count());
        }

        [TestMethod]
        public void CheckErrorTitleTests()
        {
            // Arrange
            CatalogueBusiness business = new CatalogueBusiness();
            // Act
            List<FPT_SP_CATA_GET_DETAILINFOR_OF_ITEM_Result> result = business.CheckTitle("abcxyz");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

    }
}
