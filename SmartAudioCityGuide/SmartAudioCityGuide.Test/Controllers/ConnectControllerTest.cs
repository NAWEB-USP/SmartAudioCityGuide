using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Collections.Specialized;
using System.IO;

namespace SmartAudioCityGuide.Test
{


    /// <summary>
    ///This is a test class for ConnectControllerTest and is intended
    ///to contain all ConnectControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConnectControllerTest
    {


       /* private TestContext testContextInstance;

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
        ///A test for postJson
        ///</summary>
        [TestMethod()]
        public void postJsonTest()
        {
            try
            {
                ConnectController target = new ConnectController();
                string url = "http://us.blizzard.com/en-us/";
                NameValueCollection parametros = new NameValueCollection();
                string actual;
                actual = target.postJson(url, parametros);
            }
            catch (Exception e)
            {
                Assert.AreEqual("The remote server returned an error: (404) Not Found.",e.Message);
            }
        }

        /// <summary>
        ///A test for getMemoryStream
        ///</summary>
        [TestMethod()]
        public void getMemoryStreamTest()
        {
            ConnectController target = new ConnectController();
            string url = "http://www.google.com.br/images/srpr/logo3w.png";
            Stream actual = target.getMemoryStream(url);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for getJson
        ///</summary>
        [TestMethod()]
        public void getJsonTest()
        {
            ConnectController target = new ConnectController();
            //string url = "http://www.uol.com.br";
            string url = "http://www.google.com.br";
            string actual = target.getJson(url);
            Assert.IsNotNull(actual);
        }
    }
}