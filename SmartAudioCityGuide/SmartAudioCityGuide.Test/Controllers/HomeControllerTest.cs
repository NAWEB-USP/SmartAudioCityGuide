using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Services;
using System.Web.Mvc;
using Moq;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for HomeControllerTest and is intended
    ///to contain all HomeControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HomeControllerTest
    {

        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
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
        /// <summary>
        ///A test for index
        ///</summary>
        [TestMethod()]
        public void indexTest()
        {
            HomeController target = new HomeController(iCommentServices.Object);
            ViewResult actual;
            actual = target.logIn() as ViewResult;
            Assert.AreEqual("", actual.ViewName);
        }

        /// <summary>
        ///A test for indexAfterRegister
        ///</summary>
        [TestMethod()]
        public void indexAfterRegisterTest()
        {
            HomeController target = new HomeController(iCommentServices.Object);
            ViewResult actual;
            actual = target.indexAfterRegister() as ViewResult;
            Assert.AreEqual("index", actual.ViewName);
        }

        /// <summary>
        ///A test for indexWithoutVoice
        ///</summary>
        [TestMethod()]
        public void indexWithoutVoiceTest()
        {
            HomeController target = new HomeController(iCommentServices.Object);
            ViewResult actual;
            actual = target.indexWithoutVoice() as ViewResult;
            Assert.AreEqual("index", actual.ViewName);
        }

        /// <summary>
        ///A test for logIn
        ///</summary>
        [TestMethod()]
        public void logInTest()
        {
            HomeController target = new HomeController(iCommentServices.Object);
            ViewResult actual;
            actual = target.logIn() as ViewResult;
            Assert.AreEqual("",actual.ViewName);
        }
    }
}
