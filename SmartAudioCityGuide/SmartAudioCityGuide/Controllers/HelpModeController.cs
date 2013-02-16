using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;

namespace SmartAudioCityGuide.Controllers
{
    public class HelpModeController : Controller
    {

        private IUserLocationServices userLocationServices;
        public HelpModeController()
        {
            userLocationServices = new UserLocationServices(new SmartAudioCityGuideEntities());
        }

        public HelpModeController(IUserLocationServices _userLocationsServices)
        {
            userLocationServices = _userLocationsServices;

        }

        //
        // GET: /HelpMode/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult follow(string hash)
        {
            UserLocations userLocation = userLocationServices.findUserLocationByHash(hash);
            return View();
        }

        public string getLatitudeFrom(string hash)
        {
            UserLocations userLocation = userLocationServices.findUserLocationByHash(hash);
            return userLocation.latitude.ToString();
        }

        public string getLongitudeFrom(string hash)
        {
            UserLocations userLocation = userLocationServices.findUserLocationByHash(hash);
            return userLocation.longitude.ToString();
        }

    }
}
