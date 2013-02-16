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
using Microsoft.Xna.Framework.Audio;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using SmartAudioApp.ServicesReference;
using System.Windows.Navigation;
using System.ServiceModel;

namespace SmartAudioApp
{
    public class CommentAndLocation
    {
        private GlobalPositioningSystemForMap globalPositionSystemForMap;
        private string baseWebServer;
        private Sound sound = new Sound();
        private Microphone mic = Microphone.Default;

        public CommentAndLocation(GlobalPositioningSystemForMap globalPositionSystemForMap, string baseWebServer)
        {
            this.globalPositionSystemForMap = globalPositionSystemForMap;
            this.baseWebServer = baseWebServer;
        }
        
        public void sendCommentAndActualLocationToSave(string description)
        {
            Locations location;
            try
            {
                location = new Locations(globalPositionSystemForMap.actualLocation.Position.Location.Longitude, globalPositionSystemForMap.actualLocation.Position.Location.Latitude);
            }
            catch (Exception)
            {
                return;
            }

            string locationJson = location.serialize();

            Comments comment = new Comments(description, true);

            ServicesReference.Comments commentWeb = new ServicesReference.Comments();
            commentWeb.description = comment.description;
            commentWeb.isText = true;

            ServicesReference.Locations locationWeb = new ServicesReference.Locations();
            locationWeb.latitude = location.latitude;
            locationWeb.longitude = location.longitude;

            string commentJson = comment.serialize();

            string uri = baseWebServer + "CommentsWebServices/addCommentoToLocation?location=" + locationJson + "&comment=" + commentJson + "&code=wonders";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            webRequest.BeginGetResponse(new AsyncCallback(requestCallBackSaveCommentAndLocation), webRequest);
        }

        private void requestCallBackSaveCommentAndLocation(IAsyncResult result)
        {
            var webRequest = result.AsyncState as HttpWebRequest;
            var response = (HttpWebResponse)webRequest.EndGetResponse(result);
            var baseStream = response.GetResponseStream();

            sound.play("success");
        }

        public void sendCommentAndSoundToActualLocationToSave(string description, MyMicrophone myMicrophone)
        {
            Locations location;
            byte[] soundStream = myMicrophone.streamMicrophone.ToArray();

            try
            {
                location = new Locations(globalPositionSystemForMap.actualLocation.Position.Location.Longitude, globalPositionSystemForMap.actualLocation.Position.Location.Latitude);
            }
            catch (Exception)
            {
                return;
            }

            string locationJson = location.serialize();

            Comments comment = new Comments(description, false);

            ServicesReference.Comments commentWeb = new ServicesReference.Comments();
            commentWeb.description = comment.description;
            commentWeb.isText = false;
            commentWeb.typeOfCommentsId = (Application.Current as App).idMessageType;
            commentWeb.archiveDescription = " ";
            commentWeb.sound = null;

            ServicesReference.Locations locationWeb = new ServicesReference.Locations();
            locationWeb.latitude = location.latitude;
            locationWeb.longitude = location.longitude;

            string commentJson = comment.serialize();

            var client = new WebService1SoapClient(new BasicHttpBinding(BasicHttpSecurityMode.None)
                {MaxReceivedMessageSize = 2147483647, MaxBufferSize = 2147483647 
                },new EndpointAddress(Properties.getEndPoint()));

            client.addComentAndSoundToLocationAsync(locationJson, soundStream, commentWeb, "wonders");
            client.addComentAndSoundToLocationCompleted +=new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(webService_addComentAndSoundToLocationCompleted);

        }

        void webService_addComentAndSoundToLocationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                sound.play("messagesuccess");
            }

        }

        private void requestCallBackSaveCommentAndSoundToLocation(IAsyncResult result)
        {
            var webRequest = result.AsyncState as HttpWebRequest;
            var response = (HttpWebResponse)webRequest.EndGetResponse(result);
            var baseStream = response.GetResponseStream();

            sound.play("messagesuccess");
        }

        private string convertBytesToString(byte[] bytes)
        {
            string stringResult = "";
            foreach (byte b in bytes)
            {
                stringResult = stringResult + b + ";";
            }
            stringResult = stringResult.Remove(stringResult.Length - 1, 1);

            return stringResult;
        }

        private byte[] convertStringToBytes(string stringResult)
        {
            string[] strings = stringResult.Split(';');
            byte[] bytes = new byte[strings.Length];
            int i = 0;
            foreach (string s in strings)
            {
                bytes[i] = Convert.ToByte(s);
                i++;
            }

            return bytes;
        }
    }
}
