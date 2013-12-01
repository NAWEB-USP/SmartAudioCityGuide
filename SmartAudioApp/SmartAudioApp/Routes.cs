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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace SmartAudioApp
{
    [DataContract]
    public class Routes
    {
        #region .:.Propriedades.:.
        private string baseWebServer;

        public string coordinates = null;

        public string street = null;

        public string city;

        public string country;

        [DataMember]
        public string description { get; set; }
        
        [DataMember]
        public double lat { get; set; }

        [DataMember]
        public double lon { get; set; }

        [DataMember]
        public double dist = 0.0;

        [DataMember]
        public double time = 0.0;

        [DataMember]
        public List<string> hint = null;

        public bool routeDone= false;

        public List<Routes> route;
        #endregion
        
        #region .:.Inicializadores.:.
        public Routes(string baseWebServer)
        {
            this.baseWebServer = baseWebServer;
        }
        #endregion

        #region .:.Métodos Públicos.:.
        public void getCoordinatesFromStreet(string address)
        {
            routeDone = false;
            string localBaseWebServer;
            localBaseWebServer = baseWebServer + "RouteServices/MakeGeocodeRequest?address=" + address;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(localBaseWebServer);

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(localBaseWebServer);
            webRequest.BeginGetResponse(new AsyncCallback(requestCallBackGetCoordinates), webRequest);
        }

        public void getAddressFromCoordinates(string coordinates)
        {
            routeDone = false;
            string localBaseWebServer;
            localBaseWebServer = baseWebServer + "RouteServices/MakeReverseGeocodeRequest?coordinates=" + coordinates;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(localBaseWebServer);

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(localBaseWebServer);
            webRequest.BeginGetResponse(new AsyncCallback(requestCallBackGetAddressFromCoordinates), webRequest);
        }

        public void Route(string input)
        {
            string localBaseWebServer;
            routeDone = false;
            localBaseWebServer = baseWebServer + "RouteServices/coordinateRoute?input=" + input;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(localBaseWebServer);

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(localBaseWebServer);
            webRequest.BeginGetResponse(new AsyncCallback(requestCallBackRoute), webRequest);

        }

        #endregion

        #region .:.Métodos Privados.:.
        private void requestCallBackGetCoordinates(IAsyncResult result)
        {
            try
            {
                var webRequest = result.AsyncState as HttpWebRequest;
                var response = (HttpWebResponse)webRequest.EndGetResponse(result);
                var baseStream = response.GetResponseStream();
                string finalResult;

                using (var reader = new StreamReader(baseStream))
                {
                    finalResult = reader.ReadToEnd();
                }

                this.coordinates = finalResult;
                routeDone = true;
            }
            catch (Exception)
            {
            }

        }

        private void requestCallBackGetAddressFromCoordinates(IAsyncResult result)
        {
            try
            {
                var webRequest = result.AsyncState as HttpWebRequest;
                var response = (HttpWebResponse)webRequest.EndGetResponse(result);
                var baseStream = response.GetResponseStream();
                string finalResult;

                using (var reader = new StreamReader(baseStream))
                {
                    finalResult = reader.ReadToEnd();
                }

                string[] aux = finalResult.Split(';');


                this.street = aux[0];
                this.country = aux[1];
                this.city = aux[2];
                routeDone = true;
            }
            catch (Exception)
            {
            }

        }


        private void requestCallBackRoute(IAsyncResult result)
        {
            try
            {
                var webRequest = result.AsyncState as HttpWebRequest;
                var response = (HttpWebResponse)webRequest.EndGetResponse(result);
                var baseStream = response.GetResponseStream();
                var finalResult = "";
                using (var reader = new StreamReader(baseStream))
                {
                    finalResult = reader.ReadToEnd();
                }
                MemoryStream stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(finalResult));
                if (finalResult.Contains("No location found.")) ;
                else if (finalResult.Contains("An exception occurred.")) ;
                else
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<Routes>));
                    route = (List<Routes>)deserializer.ReadObject(stream);
                }
                this.routeDone = true;
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
