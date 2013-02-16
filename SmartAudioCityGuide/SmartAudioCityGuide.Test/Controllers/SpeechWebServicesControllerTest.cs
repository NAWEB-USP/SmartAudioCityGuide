using SmartAudioCityGuide.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SmartAudioCityGuide.Services;
using System.IO;
using Moq;
using SmartAudioCityGuide.Models;
using System.Collections.Generic;

namespace SmartAudioCityGuide.Test
{
    
    
    /// <summary>
    ///This is a test class for SpeechWebServicesControllerTest and is intended
    ///to contain all SpeechWebServicesControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpeechWebServicesControllerTest
    {

        private static Mock<ICommentServices> iCommentServices = new Mock<ICommentServices>();
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
        ///A test for convertStreamToByteBuffer
        ///</summary>
        [TestMethod()]
        public void convertStreamToByteBufferTest()
        {
            SpeechWebServicesController target = new SpeechWebServicesController(iCommentServices.Object);
            Random random = new Random();
            int rand = random.Next(0,10);
            MemoryStream theStream = new MemoryStream();
            for (int i = 0; i < rand; i++)
                theStream.WriteByte(1);
            theStream.Position = 0;
            var actual = target.convertStreamToByteBuffer(theStream);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for sendLastCommentFromIdLocation
        ///</summary>
        [TestMethod()]
        public void sendLastCommentFromIdLocationTest()
        {
            List<Comments> comments = new List<Comments>();
            Comments comment = new Comments();
            comment.description = "Teste";
            comments.Add(comment);
            iCommentServices.Setup(m => m.findCommentByIdLocation(It.IsAny<int>())).Returns(comments);
            SpeechWebServicesController target = new SpeechWebServicesController(iCommentServices.Object);
            string idLocation = "1"; 
            string code = "teste"; 
            var actual = target.sendLastCommentFromIdLocation(idLocation, code);
            Assert.IsNotNull(actual);            
        }

        /// <summary>
        ///A test for talkThis
        ///</summary>
        [TestMethod()]
        public void talkThisTest()
        {
            SpeechWebServicesController target = new SpeechWebServicesController(iCommentServices.Object);
            string talk = "teste";
            string code = "teste";
            var actual = target.talkThis(talk, code);
            Assert.IsNotNull(actual);
        }
    }
}
