using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Services;
using System.Web.Mvc;
using Moq;
using SmartAudioCityGuide.Models;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for LocationsWebServicesControllerTest and is intended
    ///to contain all LocationsWebServicesControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocationsWebServicesControllerTest
    {


        /*private TestContext testContextInstance;*/
        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
        private static Mock<ILocationServices> iLocationServices = new Mock<ILocationServices>();
        private static Mock<IContactServices> iContactServices = new Mock<IContactServices>();
        private static Mock<IUserLocationServices> iUserLocationServices = new Mock<IUserLocationServices>();
        private static Mock<IUserServices> iUserServices = new Mock<IUserServices>();
        private static Mock<ICodeServices> iCodeServices = new Mock<ICodeServices>();
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
        ///A test for Index
        ///</summary>
        [TestMethod()]
        public void IndexTest()
        {
            LocationsWebServicesController target = new LocationsWebServicesController(iLocationServices.Object,iCodeServices.Object,iUserLocationServices.Object,iCommentServices.Object); 
            var actual = target.Index() as ViewResult;
            Assert.AreEqual(actual.ViewName, "");
        }

        /// <summary>
        ///A test for addLocations
        ///</summary>
        [TestMethod()]
        public void addLocationsTest()
        {
            iCodeServices.Setup(m => m.findFirstCode()).Returns("code");
            iLocationServices.Setup(m => m.addLocations(It.IsAny<Locations>()));
            LocationsWebServicesController target = new LocationsWebServicesController(iLocationServices.Object, iCodeServices.Object, iUserLocationServices.Object, iCommentServices.Object);
            Locations location = new Locations();
            string locationString;
            try
            {
                locationString = serializer.Serialize(location);
            }
            catch
            {
                locationString = "{'id':'1','longitude':'1','latitude':'1'}";
            }
            string code = "code"; 
            target.addLocations(locationString, code);
            Assert.IsTrue(true);
        }

        /// <summary>
        ///A test for searchLocations
        ///</summary>
        [TestMethod()]
        public void searchLocationsTest()
        {
            Locations location = new Locations();
            List<Locations> locations = new List<Locations>();
            location.id = 1;
            location.latitude = 1.2;
            location.longitude = 1.3;
            locations.Add(location);
            iCodeServices.Setup(m => m.findFirstCode()).Returns("code");
            iUserLocationServices.Setup(m => m.addUserLocation(It.IsAny<UserLocations>()));
            iLocationServices.Setup(m=>m.findLocationsAround(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<double>())).Returns(locations);
            LocationsWebServicesController target = new LocationsWebServicesController(iLocationServices.Object, iCodeServices.Object, iUserLocationServices.Object, iCommentServices.Object);
            string locationString = "{'id':'1','longitude':'1','latitude':'1'}";  
            string radius = "1"; 
            string windowsPhoneId = "teste"; 
            string code = "code";
            var actual = target.searchLocations(locationString, radius, windowsPhoneId, code);
            Assert.IsNotNull(actual, "");
        }
    }
}
