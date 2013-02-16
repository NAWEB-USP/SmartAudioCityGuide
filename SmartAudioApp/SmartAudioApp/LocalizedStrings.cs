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
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static Resources localizedResources = new Resources();

        public Resources Resources
        {
            get { return localizedResources; }
        }
    }
}
