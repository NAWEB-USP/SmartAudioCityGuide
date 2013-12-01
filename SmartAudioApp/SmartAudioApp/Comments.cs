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
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using SmartAudioApp.ServicesReference;
using System.ServiceModel;

namespace SmartAudioApp
{
    [DataContract]
    public class Comments
    {
        #region .:.Propriedade.:.
        [DataMember]
        public int id = 0;

        [DataMember]
        public int userId = 0;

        [DataMember]
        public int locationsId = 0;

        [DataMember]
        public int typeOfCommentsId;

        [DataMember]
        public string description;

        [DataMember]
        public string archieveDescription = null;        
                
        [DataMember]
        public bool isText = true;

        [DataMember]
        public string sound;

        public byte[] soundForComment;

        private string baseWebServer;

        private bool playSound = true;

        private bool enterComplete = true;

        private Microphone mic = Microphone.Default;

        #endregion

        #region .:.Inicializadores.:.

        public Comments(string description, bool isText)
        {
            this.description = description;
            this.isText = isText;
        }

        public Comments(string baseWebServer)
        {
            this.baseWebServer = baseWebServer;
        }

        #endregion

        #region .:.Métodos Públicos.:.
        public void getCommentFromLocationIdAndTypeOfComment(int locationId, int typeOfCommentId)
        {
            var client = new WebService1SoapClient(
                new BasicHttpBinding(BasicHttpSecurityMode.None)
                {
                    MaxReceivedMessageSize = 2147483647,
                    MaxBufferSize = 2147483647
                },
                new EndpointAddress("http://smartaudiocityguide.azurewebsites.net/WebServices.asmx"));
            WebService1SoapClient webService = new WebService1SoapClient();
            client.getSoundCommentFromLocationAndTypeOfCommentAsync(locationId, typeOfCommentId, "wonders");
            client.getSoundCommentFromLocationAndTypeOfCommentCompleted += new EventHandler<getSoundCommentFromLocationAndTypeOfCommentCompletedEventArgs>(webService_getSoundCommentFromLocationAndTypeOfCommentCompleted);
        }

        void webService_getSoundCommentFromLocationAndTypeOfCommentCompleted(object sender, getSoundCommentFromLocationAndTypeOfCommentCompletedEventArgs e)
        {
            try
            {
                while (!enterComplete) ;
                enterComplete = false;
                byte[] bytes = convertStringToBytes(e.Result.ToString());
                SoundEffect sound = new SoundEffect(bytes, mic.SampleRate, AudioChannels.Mono);
                enterComplete = true;
                while (!playSound) ;
                playSound = false;
                sound.Play();
                Thread.Sleep(TimeSpan.FromSeconds(sound.Duration.Seconds + 1));
                playSound = true;
            }
            catch (Exception exce)
            {
                enterComplete = true;
            }
        }


        public void getComment(int locationId)
        {
            /*
            string uri = baseWebServer + "SpeechWebServices/sendLastCommentFromIdLocation?idLocation=" + locationId + "&code=wonders";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            var webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            webRequest.BeginGetResponse(new AsyncCallback(requestCallBackSoundComment), webRequest);
            */
            
            WebService1SoapClient webService = new WebService1SoapClient();
            webService.getSoundCommentFromLocationAsync(locationId, "wonders");
            webService.getSoundCommentFromLocationCompleted += new EventHandler<getSoundCommentFromLocationCompletedEventArgs>(webService_getSoundCommentFromLocationCompleted);

        }


        void webService_getSoundCommentFromLocationCompleted(object sender, getSoundCommentFromLocationCompletedEventArgs e)
        {
            try
            {
                while (!enterComplete);
                enterComplete = false;
                byte[] bytes = convertStringToBytes(e.Result.ToString());
                SoundEffect sound = new SoundEffect(bytes,mic.SampleRate,AudioChannels.Mono);
                enterComplete = true;
                while (!playSound) ;
                playSound = false;
                sound.Play();
                Thread.Sleep(TimeSpan.FromSeconds(sound.Duration.Seconds + 1));
                playSound = true;
            }
            catch(Exception)
            {
                enterComplete = true;
            }
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

        #endregion

        #region .:.Métodos Privados.:.
        private void requestCallBackSoundComment(IAsyncResult result)
        {
            var webRequest = result.AsyncState as HttpWebRequest;

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)webRequest.EndGetResponse(result);
            }
            catch (Exception)
            {
                return;
            }

            var baseStream = response.GetResponseStream();

            var finalResult = "";

            // if you want to read string response
            using (var reader = new StreamReader(baseStream))
            {
                finalResult = reader.ReadToEnd();
            }

            MemoryStream memoryStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(finalResult));
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<string>));
            List<string> listBytesForStreamAudio = (List<string>)deserializer.ReadObject(memoryStream);

            byte[] buffer = new byte[listBytesForStreamAudio.Count];
            for (int i = 0; i < listBytesForStreamAudio.Count; i++)
            {
                buffer[i] = Convert.ToByte(listBytesForStreamAudio[i]);
            }

            MemoryStream stream = new MemoryStream(buffer, true);
            SoundEffect sound = SoundEffect.FromStream(stream);
            while (!playSound) ;
            playSound = false;
            sound.Play();
            Thread.Sleep(TimeSpan.FromSeconds(sound.Duration.Seconds + 1));
            playSound = true;
        }

        private void requestCallBackComment(IAsyncResult result)
        {
            var webRequest = result.AsyncState as HttpWebRequest;
            
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)webRequest.EndGetResponse(result);
            }
            catch (Exception)
            {
                return;
            }

            var baseStream = response.GetResponseStream();

            var finalResult = "";

            // if you want to read string response
            using (var reader = new StreamReader(baseStream))
            {
                finalResult = reader.ReadToEnd();
            }

            MemoryStream memoryStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(finalResult));
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<string>));
            List<string> listBytesForStreamAudio = (List<string>)deserializer.ReadObject(memoryStream);

            byte[] buffer = new byte[listBytesForStreamAudio.Count];
            for (int i = 0; i < listBytesForStreamAudio.Count; i++)
            {
                buffer[i] = Convert.ToByte(listBytesForStreamAudio[i]);
            }

            MemoryStream stream = new MemoryStream(buffer, true);
            SoundEffect sound = SoundEffect.FromStream(stream);
            while(!playSound);
            playSound = false;
            sound.Play();
            Thread.Sleep(TimeSpan.FromSeconds(sound.Duration.Seconds + 1));
            playSound = true;
        }
        private string UTF8toASCII(string json)
        {
            while (true)
            {
                int index = json.IndexOf("\"");

                if (index < 0)
                    break;

                json = json.Remove(index, 1);
            }

            int indexAux = json.IndexOf("description:");
            indexAux += 12;

            json = json.Insert(indexAux, "\"");

            indexAux = json.IndexOf("id:");
            indexAux -= 1;

            json = json.Insert(indexAux, "\"");

            return json;
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
        #endregion
    }
}
