using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using System.Web.Script.Serialization;
using SmartAudioCityGuide.Services;

namespace SmartAudioCityGuide.Controllers
{
    public class LocationsWebServicesController : Controller
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private UserLocationWebServicesController userLocationWebServices;
        private ILocationServices locationServices;
        private ICodeServices codeServices;
        private IUserLocationServices userLocationServices;
        private ICommentServices commentServices;
   
        public LocationsWebServicesController()
        {
            locationServices = new LocationServices(new SmartAudioCityGuideEntities());
            codeServices = new CodeServices(new SmartAudioCityGuideEntities());
            userLocationServices = new UserLocationServices(new SmartAudioCityGuideEntities());
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
            userLocationWebServices = new UserLocationWebServicesController();
        }

        public LocationsWebServicesController(ILocationServices _locationServices, ICodeServices _codeServices, IUserLocationServices _userLocationServices, ICommentServices _commentServices)
        {
            locationServices = _locationServices;
            codeServices = _codeServices;
            userLocationServices = _userLocationServices;
            commentServices = _commentServices;
            userLocationWebServices = new UserLocationWebServicesController(_codeServices, _userLocationServices, _commentServices);
        }


        //
        // GET: /LocationsWebServices/

        public ActionResult Index()
        {
            return View();
        }

        public void addLocations(string location, string code)
        {
            string firstCode = codeServices.findFirstCode();
            Locations locationPassed = new Locations();
            if (code == firstCode)
            {
                locationPassed = serializer.Deserialize<Locations>(location);
                locationServices.addLocations(locationPassed);
            }
        }

        public string searchLocations(string location, string radius, string windowsPhoneId, string code)
        {
            string firstCode =  codeServices.findFirstCode();
            Locations locationPassed = new Locations();
            List<Locations> locationsResult = new List<Locations>();
            string locationsResultJson = "";
            if (code == firstCode)
            {
                locationPassed = serializer.Deserialize<Locations>(location);

                locationsResult = locationServices.findLocationsAround(locationPassed.latitude.ToString(), locationPassed.longitude.ToString(), Convert.ToDouble(radius));
                try
                {
                    locationsResultJson = serializer.Serialize(locationsResult);
                }
                catch
                {

                }
                userLocationWebServices.addUserLocation("0", windowsPhoneId, locationPassed.latitude.ToString(), locationPassed.longitude.ToString(), code);

                return locationsResultJson;
            }
            return null;
        }

    }
}
