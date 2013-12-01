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

        #region .:.Propriedades.:.

        private byte[] deviceUniqueId;

        #endregion

        #region .:.Incializadores.:.
        public MyPhone()
        {
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                deviceUniqueId = (byte[])uniqueId;
        }
        #endregion

        #region .:.Métodos Públicos.:.
        public string serializedDeviceUniqueId()
        {
            string serialized = "";

            foreach (byte b in deviceUniqueId)
            {
                serialized += b.ToString() + ":";
            }

            return serialized;
        }
        #endregion
    }
}
