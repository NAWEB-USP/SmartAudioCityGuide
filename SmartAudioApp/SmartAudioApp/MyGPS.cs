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
using System.Device.Location;

namespace SmartAudioApp
{
    public class MyGPS
    {
        #region .:.Propriedades.:.

        public GeoCoordinateWatcher actualLocation;

        #endregion

        #region .:.Inicializadores.:.
        public MyGPS()
        {
            initializeGeoCoordinateWatcher();
        }
        #endregion

        #region .:.Métodos Públicos.:.
        public void initializeGeoCoordinateWatcher()
        {
            if (actualLocation == null)
            {
                actualLocation = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                actualLocation.MovementThreshold = 0.5f;
            }
            if (actualLocation.Status == GeoPositionStatus.Disabled)
            {
                MessageBox.Show("Location services must be enabled on your phone.");

                return;
            }
            actualLocation.Start();
        }
        #endregion

    }
}
