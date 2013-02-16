using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TranslatorService.Speech;
using System.Threading;
using SmartAudioCityGuide.Services;
using SmartAudioCityGuide.Models;
using System.Web.Routing;
using System.Text;
using System.IO;


namespace SmartAudioCityGuide.Controllers
{
    public class MapsController : Controller
    {
        private JavaScriptSerializer javaSerializer = new JavaScriptSerializer();
        private ILocationServices locationsServices;
        private ICommentServices commentServices;
        private LocationsController locationController;
        private SpeechWebServicesController speechWebServicesController;
        private Users user = new Users();

        public MapsController()
        {
            locationsServices = new LocationServices(new SmartAudioCityGuideEntities());
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
            locationController = new LocationsController();
            speechWebServicesController = new SpeechWebServicesController();

        }

        public MapsController(ILocationServices _locationServices, ICommentServices _commentServices)
        {
            locationsServices = _locationServices;
            commentServices = _commentServices;
            locationController = new LocationsController(_locationServices, _commentServices);
            speechWebServicesController = new SpeechWebServicesController(_commentServices);
        }


        //
        // GET: /Maps/
        public ActionResult index()
        {
            try
            {
                if (user.currentLocation == null)
                {
                    Locations location = new Locations();
                    location.latitude = 42.345573;
                    location.longitude = -71.098326;
                    user.currentLocation = location;
                }
            }
            catch
            {
            }

            /*EXEMPLO REQUESICAO EXTERNA !!!! */
            /*string result2;
            Stream result;
            Locations locationsUser = user.currentLocation;
            locationsUser.currentLocation = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ConnectController connectController = new ConnectController();
            dynamic userLocation = serializer.Serialize(locationsUser);
            dynamic result4 = speechWebServicesController.sendLastCommentFromIdLocation("18", "wonders");
            result2 = connectController.getJson("http://ec2-177-71-137-221.sa-east-1.compute.amazonaws.com/smartaudiocityguide/SpeechWebServices/sendLastCommentFromIdLocation?idLocation=18&code=wonders");
            */
            /*List<Locations> test = serializer.Deserialize<List<Locations>>(result2);
            user.currentLocation = locationsUser;
            */

            List<string> list = new List<string>();
            list.Add("Normal Mode");
            list.Add("Exploration Mode");
            ViewData["selectList"] = new SelectList(list);



            return View();
        }

        public ActionResult beforeIndex()
        {
            return View();
        }

        [Permissions]
        public void youAreAt(object[] street)
        {
            if (street != null)
            {
                string resultSearch = street.FirstOrDefault().ToString();
                string talk = "You are at " + resultSearch;
                SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
                try
                {
                    speech.Speak(talk, "en");
                }
                catch (Exception)
                {
                }
            }
        }

        [Permissions]
        public ActionResult positionOfUser(object[] posUser)
        {
            if (posUser[0] != null)
            {
                try
                {
                    var result = javaSerializer.Deserialize<dynamic>(posUser.FirstOrDefault().ToString());
                    var coorder = result["coords"];
                    user.currentLocation = new Locations();
                    user.currentLocation.latitude = Convert.ToDouble(coorder["latitude"]);
                    user.currentLocation.longitude = Convert.ToDouble(coorder["longitude"]);

                }
                catch(Exception)
                { 

                }
                return View("index");

            }

            return View();
        }


    }
}
