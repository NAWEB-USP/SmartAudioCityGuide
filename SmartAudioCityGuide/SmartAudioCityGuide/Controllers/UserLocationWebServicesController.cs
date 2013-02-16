using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Services;
using System.Web.Script.Serialization;
using SmartAudioCityGuide.Models;
using System.Globalization;

namespace SmartAudioCityGuide.Controllers
{
    public class UserLocationWebServicesController : Controller
    {
        private IUserLocationServices userLocationServices;
        private ICodeServices codeServices;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private SpeechWebServicesController speechWebServices;

        public UserLocationWebServicesController()
        {
            codeServices = new CodeServices(new SmartAudioCityGuideEntities());
            userLocationServices = new UserLocationServices(new SmartAudioCityGuideEntities());
            speechWebServices = new SpeechWebServicesController();
        }

        public UserLocationWebServicesController(ICodeServices _codeServices, IUserLocationServices _userLocationServices,ICommentServices _commentServices)
        {
            codeServices = _codeServices;
            userLocationServices = _userLocationServices;
            speechWebServices = new SpeechWebServicesController(_commentServices);
        }


        public bool addUserLocation(string userId, string windowsPhoneId, string latitude, string longitude,string code)
        {
            string firstCode = codeServices.findFirstCode();
            double lat, lng;
            int idOfUser = 0;
            UserLocations userLocation = new UserLocations();
            DateTime requestTime;
            requestTime = DateTime.Now;

            latitude = latitude.Replace(',','.');
            longitude = longitude.Replace(',', '.');

            if (code == firstCode)
            {
                lat = serializer.Deserialize<double>(latitude);
                lng = serializer.Deserialize<double>(longitude);
                idOfUser = serializer.Deserialize<int>(userId);
                userLocation.latitude = lat;
                userLocation.longitude = lng;
                userLocation.requestTime = requestTime;
                userLocation.windowsPhoneId = windowsPhoneId;
                userLocationServices.addUserLocationOrUpdatetime(userLocation);

                return true;
            }

            return false;
            
        }

        public string findClosestUserLocationByUserLocation(string userLocationJson ,string code)
        {
            UserLocations userLocation = new UserLocations();
            UserLocations userLocationMin = new UserLocations();
            userLocation = serializer.Deserialize<UserLocations>(userLocationJson);
            double distanceMin = 0 ,distanceAux = 0;
            string talk="";
            string audioString = "";


            double distance = 1;

            List<UserLocations> usersLocation = userLocationServices.findUserByUserLocationAndDistance(userLocation, distance);

            if (usersLocation.Count > 0)
            {
                userLocationMin = usersLocation.First();
                foreach(UserLocations user in usersLocation)
                {
                    distanceAux = distanceTo(userLocation.latitude, userLocation.longitude, user.longitude, user.latitude);
                    if (distanceAux < distanceMin)
                    {
                        distanceMin = distanceAux;
                        userLocationMin = user;
                    }
                }

                if(distanceMin == 0)
                {
                    talk = "There are no close users";
                }
                else
                {
                    talk = "The closest user is " + distanceMin.ToString() + " meters";
                }

                audioString = speechWebServices.talkThis(talk,"wonders");
                
                return audioString;

            }

            return "";

        }

        public double distanceTo(double longitude, double latitude ,double otherLongitude , double otherLatitude)
        {
            double distance = (longitude - otherLongitude) * (longitude - otherLongitude);
            distance += (latitude - otherLatitude) * (latitude - otherLatitude);

            return Math.Sqrt(distance);
        }
    }
}
