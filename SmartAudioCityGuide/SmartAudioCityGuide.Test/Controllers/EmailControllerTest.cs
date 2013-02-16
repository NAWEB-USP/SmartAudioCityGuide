using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for EmailControllerTest and is intended
    ///to contain all EmailControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EmailControllerTest
    {


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
        ///A test for sendEmailToAuthenticateAUser
        ///</summary>
        [TestMethod()]
        public void sendEmailToAuthenticateAUserTest()
        {
            EmailController target = new EmailController(); // TODO: Initialize to an appropriate value
            Users user = new Users();
            user.name = "greganati";
            target.sendEmailToAuthenticateAUser(user);
            Assert.IsNotNull(user);
        }
    }
}
