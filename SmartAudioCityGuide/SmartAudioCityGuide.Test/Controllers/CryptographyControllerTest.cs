using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for CryptographyControllerTest and is intended
    ///to contain all CryptographyControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CryptographyControllerTest
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
        ///A test for getMD5Hash
        ///</summary>
        [TestMethod()]
        public void getMD5HashTest()
        {
            CryptographyController target = new CryptographyController(); // TODO: Initialize to an appropriate value
            string enter = string.Empty;
            string expected = string.Empty;
            string actual;
            actual = target.getMD5Hash("TesteSmartAudioCityGuide");
            Assert.AreEqual("7DE352BC35E966716789926415D69D96", actual);
            actual = target.getMD5Hash("");
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for randomString
        ///</summary>
        [TestMethod()]
        public void randomStringTest()
        {
            CryptographyController target = new CryptographyController();
            int size = 0;
            string expected = string.Empty;
            string actual;
            actual = target.randomString(size);
            Assert.AreEqual(expected, actual);
            size = 3;
            actual = target.randomString(size);
            Assert.AreEqual(3,actual.Length);
        }
    }
}
