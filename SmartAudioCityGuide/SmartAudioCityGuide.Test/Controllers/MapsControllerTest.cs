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
    ///This is a test class for MapsControllerTest and is intended
    ///to contain all MapsControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MapsControllerTest
    {


        /*private TestContext testContextInstance;*/
        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
        private static Mock<ILocationServices> iLocationServices = new Mock<ILocationServices>();
        private static Mock<IContactServices> iContactServices = new Mock<IContactServices>();
        private static Mock<IUserLocationServices> iUserLocationServices = new Mock<IUserLocationServices>();
        private static Mock<IUserServices> iUserServices = new Mock<IUserServices>();


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        /*public TestContext TestContext
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
        ///A test for beforeIndex
        ///</summary>
        [TestMethod()]
        public void beforeIndexTest()
        {
            MapsController target = new MapsController(iLocationServices.Object, iCommentServices.Object);
            var actual = target.beforeIndex() as ViewResult;
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for index
        ///</summary>
        [TestMethod()]
        public void indexTest()
        {
            MapsController target = new MapsController(iLocationServices.Object,iCommentServices.Object);
            var actual = target.index() as ViewResult;
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for positionOfUser
        ///</summary>
        [TestMethod()]
        public void positionOfUserTest()
        {
            MapsController target = new MapsController(iLocationServices.Object, iCommentServices.Object);
            object positionOfUser = "teste";
            object[] positionsOfUser = new object[1];
            positionsOfUser.SetValue(positionOfUser, 0);
            var actual = target.positionOfUser(positionsOfUser) as ViewResult;
            Assert.AreEqual(actual.ViewName,"index");
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void positionOfUser2Test()
        {
            MapsController target = new MapsController(iLocationServices.Object, iCommentServices.Object);
            object positionOfUser = "teste";
            object[] positionsOfUser = new object[1];
            positionsOfUser.SetValue(null, 0);
            var actual = target.positionOfUser(positionsOfUser) as ViewResult;
            Assert.AreEqual(actual.ViewName, "");
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for youAreAt
        ///</summary>
        [TestMethod()]
        public void youAreAtTest()
        {
            MapsController target = new MapsController(iLocationServices.Object, iCommentServices.Object);
            object[] street = new object[1];
            street.SetValue("teste", 0);
            target.youAreAt(street);
        }
    }
}
