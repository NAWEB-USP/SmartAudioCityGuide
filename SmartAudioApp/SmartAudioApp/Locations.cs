using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Collections.Generic;

namespace SmartAudioApp
{
    [DataContract]
    public class Locations
    {
        [DataMember]
        public int id = 0;

        [DataMember]
        public double longitude;

        [DataMember]
        public double latitude;

        public Locations currentLocation = null;

        public List<Locations> locationsNearUser;

        private GlobalPositioningSystemForMap globalPositionSystemForMap;

        private string baseWebServer;

        public Locations(double longitude, double latitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public Locations(GlobalPositioningSystemForMap globalPositionSystemForMap, string baseWebServer)
        {
            this.globalPositionSystemForMap = globalPositionSystemForMap;
            this.baseWebServer = baseWebServer;
        }

        public void getLocationsAround()
        {
            Locations location = new Locations(globalPositionSystemForMap.actualLocation.Position.Location.Longitude, globalPositionSystemForMap.actualLocation.Position.Location.Latitude);
            MyPhone myPhone = new MyPhone();

            string locationJson = location.serialize();

            string uri = baseWebServer + "LocationsWebServices/searchLocations?location=" + locationJson + "&radius=1&windowsPhoneId=" + myPhone.serializedDeviceUniqueId() + "&code=wonders";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            webRequest.BeginGetResponse(new AsyncCallback(requestCallBackLocationsAround), webRequest);
        }

        public void requestCallBackLocationsAround(IAsyncResult result)
        {
            try
            {
                var webRequest = result.AsyncState as HttpWebRequest;
                var response = (HttpWebResponse)webRequest.EndGetResponse(result);
                var baseStream = response.GetResponseStream();

                var finalResult = "";

                // if you want to read string response
                using (var reader = new StreamReader(baseStream))
                {
                    finalResult = reader.ReadToEnd();
                }

                MemoryStream stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(finalResult));
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<Locations>));
                locationsNearUser = (List<Locations>)deserializer.ReadObject(stream);
            }
            catch (Exception)
            {
            }
        }


        public double distanceTo(double longitude, double latitude)
        {
            double distance = (longitude - this.longitude) * (longitude - this.longitude);
            distance += (latitude - this.latitude) * (latitude - this.latitude);

            return Math.Sqrt(distance);
        }

        public string serialize()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(this.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, this);

            byte[] jsonBytes = ms.ToArray();

            ms.Close();

            string json = UTF8Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);

            json = UTF8toASCII(json);

            return json;
        }

        private string UTF8toASCII(string json)
        {
            while (true)
            {
                int index = json.IndexOf("\"");

                if (index < 0)
                    return json;

                json = json.Remove(index, 1);
            }
        }
    }
}
