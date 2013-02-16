using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Models;
using System.Web.Mvc;
using SmartAudioCityGuide.Services;
using Moq;
using System.Web;
using System.Security.Principal;
using System.Web.Routing;
using System.IO;
using System.Web.SessionState;
using System.Reflection;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for CommentControllerTest and is intended
    ///to contain all CommentControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommentControllerTest
    {


        /*private TestContext testContextInstance;*/
        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
        private static Mock<ILocationServices> iLocationServices = new Mock<ILocationServices>();
        private static Mock<IContactServices> iContactServices = new Mock<IContactServices>();
        private static Mock<IUserLocationServices> iUserLocationServices = new Mock<IUserLocationServices>();
        private static Mock<IUserServices> iUserServices = new Mock<IUserServices>();
        private static Mock<Users> users = new Mock<Users>();


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
        ///A test for editCommentWithId
        ///</summary>
        [TestMethod()]
        public void editCommentWithIdTest()
        {
            iCommentServices.Setup(m=>m.editCommentWithIdAndComment(It.IsAny<int>(),It.IsAny<string>()));
            iCommentServices.Setup(m => m.findCommentByIdComment(It.IsAny<int>())).Returns(new Comments());
            CommentController target = new CommentController(iCommentServices.Object,iLocationServices.Object);
            int commentId = 5; 
            Comments comment = new Comments(); 
            comment.description = "Teste";
            comment.id = 1;
            var actual = target.editCommentWithId(commentId, comment,"Normal Mode") as RedirectToRouteResult;
            Assert.AreEqual(actual.RouteValues["action"], "index");
            Assert.AreEqual(actual.RouteValues["controller"], "Maps");
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for editComment
        ///</summary>
        [TestMethod()]
        public void editCommentTest()
        {
            iCommentServices.Setup(m=>m.findCommentByIdComment(It.IsAny<int>())).Returns(new Comments());
            CommentController target = new CommentController(iCommentServices.Object, iLocationServices.Object);
            int idComment = 1;
            var actual = target.editComment(idComment);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for deleteComment
        ///</summary>
        [TestMethod()]
        public void deleteCommentTest()
        {
            iCommentServices.Setup(m => m.deleteComment(It.IsAny<int>()));
            CommentController target = new CommentController(iCommentServices.Object, iLocationServices.Object);
            Random random = new Random();
            int commentId = random.Next();
            var actual = target.deleteComment(commentId) as RedirectToRouteResult;
            Assert.AreEqual(actual.RouteValues["action"], "index");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for addTextCommentWithLocation
        ///</summary>
        [TestMethod()]
        public void addTextCommentWithLocationTest()
        {
            iCommentServices.Setup(m => m.addComment(It.IsAny<Comments>()));
            CommentController target = new CommentController(iCommentServices.Object, iLocationServices.Object);
            Comments comment = new Comments();
            Random random = new Random();
            int locationId = random.Next();
            comment.description = "Teste";
            var actual = target.addTextCommentWithLocation(comment, locationId) as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "index");
            Assert.AreEqual(actual.RouteValues["controller"], "Maps");
        }

        /// <summary>
        ///A test for addTextComment
        ///</summary>
        [TestMethod()]
        public void addTextCommentTest()
        {
            iCommentServices.Setup(m => m.addComment(It.IsAny<Comments>()));
            CommentController target = new CommentController(iCommentServices.Object, iLocationServices.Object);
            Comments comment = new Comments();
            comment.description = "teste";
            var actual = target.addTextComment(comment, "Normal Mode") as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "index");
            Assert.AreEqual(actual.RouteValues["controller"], "Maps");
        }
    }
}
