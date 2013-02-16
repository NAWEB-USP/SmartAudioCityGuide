using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;
using Moq;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for ContactControllerTest and is intended
    ///to contain all ContactControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContactControllerTest
    {

        private static Mock<IContactServices> iCommentServices = new Mock<IContactServices>();

        /*private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        */
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for index
        ///</summary>
        [TestMethod()]
        public void indexTest()
        {
            ContactController target = new ContactController(iCommentServices.Object);
            ActionResult actual;
            actual = target.index() as ActionResult;
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for add
        ///</summary>
        [TestMethod()]
        public void addTest()
        {
            ContactController target = new ContactController(iCommentServices.Object);
            Contacts contact = new Contacts() ;
            contact.city = "City";
            contact.country = "Country";
            contact.eMail = "email";
            contact.id = 10;
            contact.name = "name";
            contact.phone = null;
            ViewResult actual = target.add(contact) as ViewResult;
            TempDataDictionary tempData = actual.TempData as TempDataDictionary;
            ModelStateDictionary modelState = tempData["ModelState"] as ModelStateDictionary;
            Assert.AreEqual(actual.ViewName, "index");
            Assert.IsTrue(modelState.ContainsKey("phone"));
            contact.name = null;
            actual = target.add(contact) as ViewResult;
            tempData = actual.TempData as TempDataDictionary;
            modelState = tempData["ModelState"] as ModelStateDictionary;
            Assert.AreEqual(actual.ViewName, "index");
            Assert.AreEqual(2,modelState.Count);
        }

    }
}
