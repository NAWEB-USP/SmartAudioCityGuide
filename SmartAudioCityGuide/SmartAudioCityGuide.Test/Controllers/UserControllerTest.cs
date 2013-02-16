using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Services;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using Moq;
using System.Collections.Generic;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for UserControllerTest and is intended
    ///to contain all UserControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserControllerTest
    {


        /*private TestContext testContextInstance;*/
        private static Mock<ICodeServices> iCodeServices = new Mock<ICodeServices>();
        private static Mock<IUserLocationServices> iUserLocationServices = new Mock<IUserLocationServices>();
        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
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
        ///A test for Index
        ///</summary>
        [TestMethod()]
        public void IndexTest()
        {
            iCommentServices.Setup(m => m.findCommentsByIdUser(It.IsAny<int>())).Returns(new List<Comments>());
            UserController target = new UserController(iCommentServices.Object,iUserServices.Object);
            ViewResult actual = target.Index() as ViewResult;
            ViewDataDictionary viewData = actual.ViewData;
            Assert.IsNotNull(actual);
            Assert.IsNotNull(viewData["commentsFromUser"]);
        }

        /// <summary>
        ///A test for authenticateUser
        ///</summary>
        [TestMethod()]
        public void authenticateUserTest()
        {
            iUserServices.Setup(m => m.athenticateUserWithHash(It.IsAny<string>()));
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object); 
            Random random  = new Random();
            string hash = random.Next().ToString();
            var actual = target.authenticateUser(hash) as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "indexAfterRegister");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
        }


        /// <summary>
        ///A test for connect
        ///</summary>
        [TestMethod()]
        public void connectNoExistingUserTest()
        {
            Users users = null;
            iUserServices.Setup(m => m.findUserByUserName(It.IsAny<string>())).Returns(users);
            UserController target = new UserController(iCommentServices.Object,iUserServices.Object); 
            Users user = new Users(); 
            var actual = target.connect(user) as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "indexWithoutVoice");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
        }

        /// <summary>
        ///A test for connect
        ///</summary>
        [TestMethod()]
        public void connectExistingUserAndDiferentePasswordTest()
        {
            CryptographyController cryptoController = new CryptographyController();            
            Users user1 = new Users();
            Users user2 = new Users();
            user1.password = cryptoController.getMD5Hash("teste");
            iUserServices.Setup(m => m.findUserByUserName(It.IsAny<string>())).Returns(user1);
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object);
            UserController target2 = new UserController(iCommentServices.Object, iUserServices.Object);
            user2.password = "teste1";
            var actual = target.connect(user2) as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "indexWithoutVoice");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
        }

        /// <summary>
        ///A test for connect
        ///</summary>
        [TestMethod()]
        public void connectNotAuthenticatedTest()
        {
            CryptographyController cryptoController = new CryptographyController();
            Users user1 = new Users();
            Users user2 = new Users();
            user1.password = cryptoController.getMD5Hash("teste");
            iUserServices.Setup(m => m.findUserByUserName(It.IsAny<string>())).Returns(user1);
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object);
            user2.password = "teste";
            user2.authenticate = 0;
            var actual = target.connect(user2) as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "indexWithoutVoice");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
        }

        /// <summary>
        ///A test for connect
        ///</summary>
        [TestMethod()]
        public void connectTest()
        {
            CryptographyController cryptoController = new CryptographyController();
            Users user1 = new Users();
            Users user2 = new Users();
            user1.password = cryptoController.getMD5Hash("teste");
            user1.authenticate = 1;
            iUserServices.Setup(m => m.findUserByUserName(It.IsAny<string>())).Returns(user1);
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object);
            user2.password = "teste";
            var actual = target.connect(user2) as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "index");
            Assert.AreEqual(actual.RouteValues["controller"], "User");
        }

        /// <summary>
        ///A test for logOff
        ///</summary>
        [TestMethod()]
        public void logOffTest()
        {
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object);
            var actual = target.logOff() as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "Index");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
        }

        /// <summary>
        ///A test for logOut
        ///</summary>
        [TestMethod()]
        public void logOutTest()
        {
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object); 
            var actual = target.logOut() as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "index");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
        }

        /// <summary>
        ///A test for register
        ///</summary>
        [TestMethod()]
        public void register1Test()
        {
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object);  
            var actual = target.register() as ViewResult;
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for register
        ///</summary>
        [TestMethod()]
        public void registerDiferrentPasswordTest()
        {
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object);
            Users user = new Users();
            string confirmPassword = "teste";
            user.password = "teste1";
            ViewResult actual = target.register(user,confirmPassword) as ViewResult;
            TempDataDictionary tempData = actual.TempData as TempDataDictionary;
            ModelStateDictionary modelState = tempData["ModelState"] as ModelStateDictionary;
            Assert.IsNotNull(actual);
            Assert.IsTrue(modelState.ContainsKey("password"));
            Assert.IsTrue(modelState.ContainsKey("confirmPassword"));

        }


        /// <summary>
        ///A test for register
        ///</summary>
        [TestMethod()]
        public void registerAlreadyExistUserTest()
        {
            iUserServices.Setup(m => m.hasAUserWithThisEmail(It.IsAny<string>())).Returns(true);
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object); 
            Users user = new Users();
            Random random = new Random();
            user.password =  random.Next().ToString();
            ViewResult actual = target.register(user, user.password) as ViewResult;
            TempDataDictionary tempData = actual.TempData as TempDataDictionary;
            ModelStateDictionary modelState = tempData["ModelState"] as ModelStateDictionary;
            Assert.IsNotNull(actual);
            Assert.IsTrue(modelState.ContainsKey("userName"));
        }

        /// <summary>
        ///A test for register
        ///</summary>
        [TestMethod()]
        public void registerOkTest()
        {
            iUserServices.Setup(m => m.hasAUserWithThisEmail(It.IsAny<string>())).Returns(false);
            iUserServices.Setup(m => m.addUser(It.IsAny<Users>())); 
            UserController target = new UserController(iCommentServices.Object, iUserServices.Object);
            Users user = new Users();
            Random random = new Random();
            user.password = random.Next().ToString();
            RedirectToRouteResult actual = target.register(user, user.password) as RedirectToRouteResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.RouteValues["action"], "index");
            Assert.AreEqual(actual.RouteValues["controller"], "Home");
        }
    }
}
