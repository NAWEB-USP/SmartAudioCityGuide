using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.RouteService;
using SmartAudioCityGuide.GeocodeService;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Drawing;
using SmartAudioCityGuide.Models;
using System.Web.Mvc;
using System.Net;
using System.IO;
using TranslatorService.Speech;

namespace SmartAudioCityGuide.Controllers
{
    public class RouteServicesController : Controller
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private List<Route> routeList = new List<Route>();
        private IRouteService routeService;
        string key = "At48uqJw8iwp0FOZl8lT1mW3sw26dylKaJMjD_mThDoDzLvUx6v9jMZ7eX7Vuam_";

        public RouteServicesController()
        {
            routeService = new RouteServiceClient("BasicHttpBinding_IRouteService");
        }

        public RouteServicesController(IRouteService _routeService)
        {
            routeService = _routeService;
        }

        /*private string TranslateMethod(string text,string from,string to)
        {
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            WebResponse response = null;
            try
            {
                response = httpWebRequest.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                    string translation = (string)dcs.ReadObject(stream);
                    return translation;
                }
            }
            catch(Exception e)
            {
            }
            return text;
        }*/

        private string TranslateMethod(string text,string from,string to)
        {
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            text = speech.Translate(text, to);
            Regex regex = new Regex("cabeça",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
            text = regex.Replace(text, "dirija-se");
            return text;
        }

        public string CreateRoute(string waypointString,string orientation,string language)
        {
            try
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                string results = "";
                RouteRequest routeRequest = new RouteRequest();
                // Set the credentials using a valid Bing Maps key
                routeRequest.Credentials = new RouteService.Credentials();
                routeRequest.Credentials.ApplicationId = key;
                routeRequest.Options = new RouteOptions();
                routeRequest.Options.Mode = TravelMode.Walking;
                routeRequest.Options.RoutePathType = RoutePathType.Points;
                routeRequest.UserProfile = new SmartAudioCityGuide.RouteService.UserProfile();
                routeRequest.UserProfile.DistanceUnit = SmartAudioCityGuide.RouteService.DistanceUnit.Kilometer;
                routeRequest.UserProfile.DeviceType = SmartAudioCityGuide.RouteService.DeviceType.Mobile;
                routeRequest.UserProfile.CurrentHeading = new SmartAudioCityGuide.RouteService.Heading();
                routeRequest.UserProfile.CurrentHeading.Orientation = double.Parse(orientation.Trim(), culture);


                //Parse user data to create array of waypoints
                string[] points = waypointString.Split(';');
                Waypoint[] waypoints = new Waypoint[points.Length];

                int pointIndex = -1;
                foreach (string point in points)
                {
                    pointIndex++;
                    waypoints[pointIndex] = new Waypoint();
                    string[] digits = point.Split(','); waypoints[pointIndex].Location = new RouteService.Location();

                    waypoints[pointIndex].Location.Latitude = double.Parse(digits[0].Trim(), culture);
                    waypoints[pointIndex].Location.Longitude = double.Parse(digits[1].Trim(), culture);

                    if (pointIndex == 0)
                        waypoints[pointIndex].Description = "Start";
                    else if (pointIndex == points.Length)
                        waypoints[pointIndex].Description = "End";
                    else
                        waypoints[pointIndex].Description = string.Format("Stop #{0}", pointIndex);
                }

                routeRequest.Waypoints = waypoints;

                // Make the calculate route request
                RouteResponse routeResponse = routeService.CalculateRoute(routeRequest);
                string tes = serializer.Serialize(routeResponse);

                // Iterate through each itinerary item to get the route directions
                StringBuilder directions = new StringBuilder("");

                if (routeResponse.Result.Legs.Length > 0)
                {
                    int instructionCount = 0;
                    int legCount = 0;

                    foreach (RouteLeg leg in routeResponse.Result.Legs)
                    {
                        legCount++;
                        directions.Append(string.Format("Leg #{0}\n", legCount));

                        foreach (ItineraryItem item in leg.Itinerary)
                        {
                            instructionCount++;
                            directions.Append(string.Format("{0}. {1}\n",
                                instructionCount, item.Text));
                        }
                    }
                    //Remove all Bing Maps tags around keywords.  
                    //If you wanted to format the results, you could use the tags
                    /*Regex regex = new Regex("<[/a-zA-Z:1-9=\\\"]*>",*/
                    Regex regex = new Regex("<[^>]*>",
                      RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    results = regex.Replace(directions.ToString(), string.Empty);

                    for (int i = 0; i < routeResponse.Result.Legs[0].Itinerary.Length; i++ )
                    {
                        Route routeInfo = new Route();

                        routeInfo.description = regex.Replace(routeResponse.Result.Legs[0].Itinerary[i].Text,string.Empty);
                        routeInfo.lat = routeResponse.Result.Legs[0].Itinerary[i].Location.Latitude;
                        routeInfo.lon = routeResponse.Result.Legs[0].Itinerary[i].Location.Longitude;
                        routeInfo.time = routeResponse.Result.Legs[0].Itinerary[i].Summary.TimeInSeconds;
                        routeInfo.dist = routeResponse.Result.Legs[0].Itinerary[i].Summary.Distance;
                        for(int j = 0 ; j < routeResponse.Result.Legs[0].Itinerary[i].Hints.Length ; j++)
                        {
                            routeInfo.hint.Add(regex.Replace(routeResponse.Result.Legs[0].Itinerary[i].Hints[j].Text,string.Empty));
                        }
                        if(!language.Contains("en"))
                        {
                            routeInfo.description = TranslateMethod(routeInfo.description,"en",language);
                        }
                        routeList.Add(routeInfo);
                        
                    }
                    
                }
                else
                    return "No Route found";

                return serializer.Serialize(routeList);
            }
            catch (Exception)
            {
                return "An exception occurred.";
            }

        }

        /// <summary>
        /// Converts the address into GPS coordinates (latitude and longitude)
        /// </summary>
        /// <param name="address">The address to be converted.</param>
        /// <returns>"latitude, longitude"</returns>
        public string MakeGeocodeRequest(string address)
        {
            try
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                // Set a Bing Maps key before making a request

                GeocodeService.GeocodeRequest geocodeRequest = new GeocodeService.GeocodeRequest();

                // Set the credentials using a valid Bing Maps Key
                geocodeRequest.Credentials = new GeocodeService.Credentials();
                geocodeRequest.Credentials.ApplicationId = key;

                // Set the full address query
                geocodeRequest.Query = address;

                // Set the options to only return high confidence results
                GeocodeService.ConfidenceFilter[] filters = new GeocodeService.ConfidenceFilter[1];
                filters[0] = new GeocodeService.ConfidenceFilter();
                filters[0].MinimumConfidence = GeocodeService.Confidence.High;

                GeocodeService.GeocodeOptions geocodeOptions = new GeocodeService.GeocodeOptions();
                geocodeOptions.Filters = filters;

                geocodeRequest.Options = geocodeOptions;

                // Make the geocode request
                GeocodeService.GeocodeServiceClient geocodeService =
                new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                GeocodeService.GeocodeResponse geocodeResponse = geocodeService.Geocode(geocodeRequest);

                if (geocodeResponse.Results.Count() == 0)
                {
                    return "No location found.";
                }
                return geocodeResponse.Results[0].Locations[0].Latitude.ToString(culture) + "," + geocodeResponse.Results[0].Locations[0].Longitude.ToString(culture);

            }
            catch (Exception)
            {
                return "An exception occurred.";
            }

        }

        /// <summary>
        /// Converts GPS coordinates into address.
        /// </summary>
        /// <param name="coordinates">"latitue, longitude"</param>
        /// <returns>address + ; + country + ; + city</returns>
        public string MakeReverseGeocodeRequest(string coordinates)
        {
            try
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                // Set a Bing Maps key before making a request

                GeocodeService.ReverseGeocodeRequest reverseGeocodeRequest = new GeocodeService.ReverseGeocodeRequest();

                // Set the credentials using a valid Bing Maps key
                reverseGeocodeRequest.Credentials = new GeocodeService.Credentials();
                reverseGeocodeRequest.Credentials.ApplicationId = key;

                string[] digits = coordinates.Split(',');
                double lat = double.Parse(digits[0].Trim(), culture);
                double lon = double.Parse(digits[1].Trim(), culture);

                // Set the point to use to find a matching address
                GeocodeService.Location point = new GeocodeService.Location();
                point.Latitude = lat;
                point.Longitude = lon;

                reverseGeocodeRequest.Location = point;

                // Make the reverse geocode request
                GeocodeService.GeocodeServiceClient geocodeService =
                new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                GeocodeService.GeocodeResponse geocodeResponse = geocodeService.ReverseGeocode(reverseGeocodeRequest);

                if (geocodeResponse.Results.Count() == 0)
                {
                    return "No location found.";
                }
                return geocodeResponse.Results[0].Address.AddressLine + ";" + geocodeResponse.Results[0].Address.CountryRegion + ";" + geocodeResponse.Results[0].Address.Locality;

            }
            catch (Exception)
            {
                return "An exception occurred.";

            }

        }


        public string addressRoute(string input)
        {
            string[] sInput = input.Split(';');
            string initialCoordinates = sInput[0];
            string orientation = sInput[2];
            string finalCoordinates = MakeGeocodeRequest(sInput[1]);
            string language = sInput[3];
            if (finalCoordinates == "No location found.") return "No location found.";
            if (finalCoordinates == "An exception occurred.") return "An exception occurred.";
            string result = CreateRoute(initialCoordinates + ";" + finalCoordinates, orientation, language);
            return result;

        }



        public string coordinateRoute(string input)
        {
            string[] sInput = input.Split(';');
            string initialCoordinates = sInput[0];
            string finalCoordinates = sInput[1];
            string orientation = sInput[2];
            string language = sInput[3];
            string result = CreateRoute(initialCoordinates + ";" + finalCoordinates, orientation, language);
            return result;

        }
    }
}