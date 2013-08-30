using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;
using System.Web.Script.Serialization;
using System.Web.Routing;
using TranslatorService.Speech;
using System.Threading;

namespace SmartAudioCityGuide.Controllers
{
    public class LocationsController : Controller
    {
        private Users userSession = new Users();
        private JavaScriptSerializer javaSerializer = new JavaScriptSerializer();
        private ILocationServices locationServices;
        private ICommentServices commentServices;

        public LocationsController()
        {
            locationServices = new LocationServices(new SmartAudioCityGuideEntities());
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
        }

        public LocationsController(ILocationServices _locationServices, ICommentServices _commentServices)
        {
            locationServices = _locationServices;
            commentServices = _commentServices;
    }


        public void currentLocation(string lat, string lng)
        {
            Locations location = new Locations();
            location.latitude = Convert.ToDouble(lat);
            location.longitude = Convert.ToDouble(lng);
            try
            {
                location.currentLocation = location;
            }
            catch (Exception)
            {
            }
        }

        public List<Locations> locationsOf(string lat, string lng)
        {
            List<Locations> locations = new List<Locations>();

            locations = locationServices.findLocationsAround(lat, lng, 3);
            return locations;
        }

        public ActionResult tellAboutLocation(string lat, string lng)
        {
            Locations location = new Locations();

            try
            {
                if (location.currentLocation != null)
                {
                    lat = location.currentLocation.latitude.ToString();
                    lng = location.currentLocation.longitude.ToString();
                }

                location.currentLocation = locationServices.findLocationByLatAndLng(lat, lng);
                return RedirectToActionPermanent("showComments", new { idLocation = location.currentLocation.id });
            }
            catch (Exception) 
            { 
                return null; 
            }
        }

        public ActionResult showComments(int idLocation)
        {
            Locations location = new Locations();
            string lat, lng;
            try
            {
                if (location.currentLocation != null)
                {
                    lat = location.currentLocation.latitude.ToString();
                    lng = location.currentLocation.longitude.ToString();
                    location.currentLocation = locationServices.findLocationByLatAndLng(lat, lng);
                    idLocation = location.currentLocation.id;
                }
            }
            catch (Exception) 
            { 
            }


            List<Comments> comments = new List<Comments>();
            comments = commentServices.findCommentByIdLocation(idLocation);
            byte[] bytes;
            foreach (Comments comment in comments)
            {
                bytes = convertStringToBytes(comment.sound);
            }
            ViewData["locationId"] = idLocation;
            ViewData["comments"] = comments;
            return View();
        }

        private byte[] convertStringToBytes(string description)
        {
            byte[] bytes;
            string[] chars = description.Split(';');
            bytes = new byte[chars.Length];
            int i  = 0;
            foreach (string s in chars)
            {
                bytes[i] = Convert.ToByte(s);
                i = i + 1;
            }

            return bytes;
        }
        


    }
}
