using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Services;
using Moq;
using SmartAudioCityGuide.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for UserLocationWebServicesControllerTest and is intended
    ///to contain all UserLocationWebServicesControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserLocationWebServicesControllerTest
    {


        /*private TestContext testContextInstance;*/
        private static Mock<ICodeServices> iCodeServices = new Mock<ICodeServices>();
        private static Mock<IUserLocationServices> iUserLocationServices = new Mock<IUserLocationServices>();
        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

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
        ///A test for addUserLocation
        ///</summary>
        [TestMethod()]
        public void addUserLocationTest()
        {
            Random randomString = new Random();
            string stringRandom = randomString.Next().ToString();
            iUserLocationServices.Setup(m => m.addUserLocation(It.IsAny<UserLocations>()));
            iCodeServices.Setup(m => m.findFirstCode()).Returns(stringRandom);
            UserLocationWebServicesController target = new UserLocationWebServicesController(iCodeServices.Object,iUserLocationServices.Object,iCommentServices.Object);
            string userId = "253525";
            string windowsPhoneId = "7123";
            string latitude = "2";
            string longitude = "3";
            string code = stringRandom;
            bool actual;
            actual = target.addUserLocation(userId, windowsPhoneId, latitude, longitude, code);
            Assert.AreEqual(true, actual);
            actual = target.addUserLocation(userId, windowsPhoneId, latitude, longitude, "12313");
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///A test for distanceTo
        ///</summary>
        [TestMethod()]
        public void distanceToTest()
        {
            UserLocationWebServicesController target = new UserLocationWebServicesController(iCodeServices.Object, iUserLocationServices.Object, iCommentServices.Object);
            double longitude = 0F;
            double latitude = 0F;
            double otherLongitude = 0F;
            double otherLatitude = 0F;
            double actual;
            actual = target.distanceTo(longitude, latitude, otherLongitude, otherLatitude);
            Assert.AreEqual(0, actual);
            longitude = 10F;
            latitude = 0F;
            otherLongitude = 0F;
            otherLatitude = 0F;
            actual = target.distanceTo(longitude, latitude, otherLongitude, otherLatitude);
            Assert.AreEqual(10, actual);
            longitude = -5F;
            latitude = -10F;
            otherLongitude = 5F;
            otherLatitude = 10F;
            actual = target.distanceTo(longitude, latitude, otherLongitude, otherLatitude);
            Assert.AreEqual(actual, Math.Sqrt(500));
        }

        /// <summary>
        ///A test for findClosestUserLocationByUserLocation
        ///</summary>
        [TestMethod()]
        public void findClosestUserLocationByUserLocationTest()
        {
            List<UserLocations> list = new List<UserLocations>();
            UserLocations user = new UserLocations();
            user.latitude = 10;
            user.longitude = 20;
            user.id = 3;
            user.userId = 3;
            user.windowsPhoneId = "3";
            iCodeServices.Setup(m => m.findFirstCode()).Returns("1");
            iUserLocationServices.Setup(m => m.findUserByUserLocationAndDistance(It.IsAny<UserLocations>(), It.IsAny<double>())).Returns(list);
            iCommentServices.Setup(m => m.findCommentByIdComment(It.IsAny<int>()));
            UserLocationWebServicesController target = new UserLocationWebServicesController(iCodeServices.Object, iUserLocationServices.Object, iCommentServices.Object);
            string userLocationJson = serializer.Serialize(user);
            string code = "1";
            string actual;
            actual = target.findClosestUserLocationByUserLocation(userLocationJson, code);
            Assert.AreEqual("", actual);
            user.latitude = 15;
            list.Add(user);
            iUserLocationServices.Setup(m => m.findUserByUserLocationAndDistance(It.IsAny<UserLocations>(), It.IsAny<double>())).Returns(list);
            target = new UserLocationWebServicesController(iCodeServices.Object, iUserLocationServices.Object, iCommentServices.Object);
            actual = target.findClosestUserLocationByUserLocation(userLocationJson, code);
            Assert.IsNotNull(actual);
        }
    }
}
