using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranslatorService.Speech;
using System.Threading;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;
using System.Web.Script.Serialization;

namespace SmartAudioCityGuide.Controllers
{
    public class HomeController : Controller
    {
        private Users userSession = new Users();
        private ConnectController connectController = new ConnectController();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private ICommentServices commentServices;



        public HomeController()
        {
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
        }

        public HomeController(ICommentServices _commentServices)
        {
            commentServices = _commentServices;
        }


        [HttpGet]
        public ActionResult logIn()
        {
            return View();
        }

        //
        // GET: /Home/
        public ActionResult index()
        {
            try
            {
                if (userSession.currentUser != null && userSession.currentUser.authenticate != 0)
                    return RedirectToAction("index", "User");
            }
            catch (Exception)
            {
            }
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            Thread thread = new Thread(new ThreadStart((Action)(() =>
                    {
                        try
                        {
                            speech.Speak("Smart Audio City Guide", "en");
                        }
                        catch (Exception)
                        {
                        }

                    }
             )));

            //connectController.getJson("http://localhost:12729/LocationsWebServices/searchLocations?location={currentLocation:null,id:0,latitude:-23.60623,longitude:-46.63943}&radius=1&windowsPhoneId=76:166:78:202:174:128:119:95:49:74:176:162:244:201:82:84:214:139:57:0:&code=wonders");
            
            
            /*
            RouteServicesController target = new RouteServicesController();
            string waypointString = string.Empty;
            string expected = string.Empty;
            string actual;
            actual = target.CreateRoute("38.9265353232622, -77.073235809803; 38.9230918884277, -77.0788726806641","0");


            string actual2 = target.MakeGeocodeRequest("avenida padre pereira de andrade 545, sao paulo sp");
            string actual3 = target.MakeReverseGeocodeRequest(actual2);

            target = new RouteServicesController();
            string actual5 = target.MakeGeocodeRequest("Avenida Paulista, 1969 Sao Paulo Brazil");
            if (!actual5.Contains("No location found.") && !actual5.Contains("An exception occurred."))
            {
                string actual4 = target.CreateRoute("-23.5372009, -46.7198792;" + actual5, "0");
                List<Route> routes = serializer.Deserialize<List<Route>>(actual4);
            }
            */



            thread.Start();
            ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);


            return View();
        }

        public ActionResult indexWithoutVoice()
        {
            try
            {
                userSession.currentUser = null;
            }
            catch (Exception)
            {
            }
            ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            return View("index");
        }

        public ActionResult indexAfterRegister()
        {
            return View("index");
        }


    }
}
