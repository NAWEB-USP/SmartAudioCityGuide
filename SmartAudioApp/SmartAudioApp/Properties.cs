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

namespace SmartAudioApp
{
    public class Properties
    {
        static public string getServerIP()
        {
            //return "http://localhost:41682/";
            //return "http://187.37.25.191:81/SmartAudioCityGuide/";
            return "http://smartaudiocityguide.azurewebsites.net/";
        }

        static public string getEndPoint()
        {
            //return "http://localhost:41682/Webservices.asmx";
            return "http://smartaudiocityguide.azurewebsites.net/WebServices.asmx";
        }
    }
}
