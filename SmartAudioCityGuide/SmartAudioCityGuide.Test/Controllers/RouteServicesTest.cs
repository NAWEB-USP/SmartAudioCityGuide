using SmartAudioCityGuide.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Script.Serialization;
using Moq;
using SmartAudioCityGuide.RouteService;
using System.Collections.Generic;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for RouteServicesTest and is intended
    ///to contain all RouteServicesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RouteServicesTest
    {
        private static Mock<IRouteService> iRouteServiceClient = new Mock<IRouteService>();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
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
        ///A test for CreateRoute
        ///</summary>
        /*[TestMethod()]
        public void CreateRouteTest()
        {
            RouteServicesController target = new RouteServicesController(iRouteServiceClient.Object);
            string waypointString = string.Empty;
            string expected = string.Empty;
            string actual;
            actual = target.CreateRoute("38.9265353232622, -77.073235809803; 38.9230918884277, -77.0788726806641");
            Assert.Equals(1, 1);
        }*/
    }
}
