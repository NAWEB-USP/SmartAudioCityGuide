using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Services;
using SmartAudioCityGuide.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;

namespace SmartAudioCityGuide.Test
{
   


    /// <summary>
    ///This is a test class for LocationsControllerTest and is intended
    ///to contain all LocationsControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocationsControllerTest
    {

        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
        private static Mock<ILocationServices> iLocationServices = new Mock<ILocationServices>();


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
        ///A test for currentLocation
        ///</summary>
        [TestMethod()]
        public void currentLocationTest()
        {
            LocationsController target = new LocationsController(iLocationServices.Object, iCommentServices.Object);
            string lat = "103.87";
            string lng = "102.456";
            target.currentLocation(lat, lng);
            Assert.IsTrue(true);
        }

        /// <summary>
        ///A test for locationsOf
        ///</summary>
        [TestMethod()]
        public void locationsOfTest()
        {
            List<Locations> locations = new List<Locations>();
            List<Locations> expected = locations;
            Locations location = new Locations();
            location.latitude = 10;
            location.longitude = 20;
            location.id = 20;
            locations.Add(location);
            iLocationServices.Setup(m => m.findLocationsArround(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Double>())).Returns(locations);
            LocationsController target = new LocationsController(iLocationServices.Object,iCommentServices.Object);
            List <Locations> actual = target.locationsOf("10", "20");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for showComments
        ///</summary>
        [TestMethod()]
        public void showCommentsTest()
        {
            List<Locations> locations = new List<Locations>();
            Locations location = new Locations();
            Comments comment = new Comments();
            List<Comments> comments = new List<Comments>();
            comment.archiveDescription = "Teste";
            comment.description = "Teste";
            comment.id = 256;
            comment.isText = true;
            comment.locationsId = 20;
            comment.userId = 20;
            comment.sound = "1;0;1;0;1;1;1;1;1;0;0;0";
            comments.Add(comment);
            location.latitude = 10;
            location.longitude = 20;
            location.id = 20;
            locations.Add(location);
            iLocationServices.Setup(m => m.findLocationsArround(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Double>())).Returns(locations);
            iCommentServices .Setup(m => m.findCommentByIdLocation(location.id)).Returns(comments);
            int idLocation = 20;
            LocationsController target = new LocationsController(iLocationServices.Object, iCommentServices.Object);
            ViewResult res;
            res = target.showComments(idLocation) as ViewResult;
            ViewDataDictionary tempData = res.ViewData as ViewDataDictionary;
            Assert.AreEqual(20, tempData["locationid"]);
            Assert.AreEqual(comments, tempData["comments"]);
            Assert.IsNotNull(res);
        }

        /// <summary>
        ///A test for tellAboutLocation
        ///</summary>
        [TestMethod()]
        public void tellAboutLocationTest()
        {
            LocationsController target = new LocationsController(iLocationServices.Object,iCommentServices.Object);
            string lat = "15";
            string lng = "17";
            ActionResult actual;
            actual = target.tellAboutLocation(lat, lng);
            Assert.IsNull(actual);
        }

    }
}
