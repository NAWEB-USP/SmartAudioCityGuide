using SmartAudioCityGuide.Controllers;
using SmartAudioCityGuide.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Services;
using Moq;
using System.Collections.Generic;
using System.Web.Script.Serialization;


namespace SmartAudioCityGuide.Test
{


    /// <summary>
    ///This is a test class for CommentsWebServicesControllerTest and is intended
    ///to contain all CommentsWebServicesControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommentsWebServicesControllerTest
    {


        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
        private static Mock<ILocationServices> iLocationServices = new Mock<ILocationServices>();
        private static Mock<ICodeServices> iCodeServices = new Mock<ICodeServices>();
        /*private TestContext testContextInstance;*/

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
       /* public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }*/

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
        ///A test for findAllCommentsArround
        ///</summary>
        [TestMethod()]
        public void findAllCommentsArroundTest()
        {
            Locations location = new Locations();
            Codes code = new Codes();
            Comments comment = new Comments();
            List<Comments> comments = new List<Comments>();
            location.id = 20;
            location.latitude = 25;
            location.longitude = 20;
            code.code = "7DE352BC35E966716789926415D69D96";
            code.id = 5;
            comment.archiveDescription = "archive";
            comment.description = "descr";
            comment.id = 30;
            comment.isText = true;
            comment.locationsId = 23;
            comment.userId = 10;
            comments.Add(comment);
            iLocationServices.Setup(m => m.findLocationByLatAndLng(It.IsAny<string>(), It.IsAny<string>())).Returns(location);
            iCommentServices.Setup(m => m.findCommentByIdLocation(It.IsAny<int>())).Returns(comments);
            iCodeServices.Setup(m => m.findFirstCode()).Returns("code");
            CommentsWebServicesController target = new CommentsWebServicesController(iCodeServices.Object, iLocationServices.Object, iCommentServices.Object);
            string actual = target.findAllCommentsArround(null, "3", "7DE352BC35E966716789926415D69D96");
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for addCommentoToLocation
        ///</summary>
        [TestMethod()]
        public void addCommentoToLocationTest()
        {
            iCodeServices.Setup(m => m.findFirstCode()).Returns("code");
            CommentsWebServicesController target = new CommentsWebServicesController(iCodeServices.Object, iLocationServices.Object, iCommentServices.Object);
            string location = string.Empty;
            string comment = string.Empty;
            string code = string.Empty;
            string expected = string.Empty;
            string actual;
            actual = target.addCommentoToLocation(null, null, "7DE352BC35E966716789926415D69D96");
            Assert.AreEqual("0", actual);
        }
    }
}
