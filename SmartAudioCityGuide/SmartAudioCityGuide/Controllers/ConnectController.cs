using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace SmartAudioCityGuide.Controllers
{
    public class ConnectController : Controller
    {
        public string getJson(String url)
        {
            byte[] buffer = new byte[8192];
            string stringTime = null;
            int count = 0;

            StringBuilder stringBuilder = new StringBuilder();
            String stringBuilder2 = "";


            HttpWebRequest request = (HttpWebRequest)

            //request.Method ="POST" , para POST   

            WebRequest.Create(url);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();

                do
                {

                    count = resStream.Read(buffer, 0, buffer.Length);


                    if (count != 0)
                    {

                        stringTime = Encoding.ASCII.GetString(buffer, 0, count);

                        stringBuilder.Append(stringTime);
                    }
                }
                while (count > 0);

                stringBuilder2 = stringBuilder.ToString();

                return (stringBuilder2);
            }

            catch (WebException)
            {

                return null;
            }
        }

        public Stream getMemoryStream(string url)
        {
            byte[] buffer = new byte[8192];
            StringBuilder stringBuilder = new StringBuilder();
            Stream resStream;
            HttpWebRequest request = (HttpWebRequest)

            //request.Method ="POST" , para POST   

            WebRequest.Create(url);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                resStream = response.GetResponseStream();

            }
            catch (WebException)
            {

                return null;
            }

            return resStream;
        }

        public string postJson(String url, NameValueCollection parametros)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            WebClient client = new WebClient();
            var results = client.UploadValues(url, parametros);

            var reponse = System.Text.Encoding.Default.GetString(results);

            return reponse;
        }

    }
}
