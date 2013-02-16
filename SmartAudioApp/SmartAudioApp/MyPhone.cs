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
using Microsoft.Phone.Info;

namespace SmartAudioApp
{
    public class MyPhone
    {
        private byte[] deviceUniqueId;

        public MyPhone()
        {
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                deviceUniqueId = (byte[])uniqueId;
        }

        public string serializedDeviceUniqueId()
        {
            string serialized = "";

            foreach (byte b in deviceUniqueId)
            {
                serialized += b.ToString() + ":";
            }

            return serialized;
        }
    }
}
