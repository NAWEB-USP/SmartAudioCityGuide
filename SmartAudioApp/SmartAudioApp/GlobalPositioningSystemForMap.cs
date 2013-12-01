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
using Microsoft.Phone.Controls.Maps;

namespace SmartAudioApp
{
    public class GlobalPositioningSystemForMap
    {
        #region .:.Propriedade.:.
        public GeoCoordinateWatcher actualLocation;
        public Pushpin userPushpin;
        private MapItemsControl mapControl = new MapItemsControl();
        private Map map = new Map();
        #endregion

        #region .:.Incializadores.:.
        public GlobalPositioningSystemForMap(MapItemsControl mapControl, Map map)
        {
            this.mapControl = mapControl;
            this.map = map;
        }
        #endregion

        #region .:.Métodos Públicos.:.
        public void initializeGeoCoordinateWatcher()
        {
            if (actualLocation == null)
            {
                actualLocation = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                actualLocation.MovementThreshold = 0.5f;
                actualLocation.StatusChanged += locationStatusChanged;
                actualLocation.PositionChanged += locationPositionChanged;
            }
            if (actualLocation.Status == GeoPositionStatus.Disabled)
            {
                MessageBox.Show("Location services must be enabled on your phone.");

                return;
            }

            actualLocation.Start();
        }


        public void removeGeoCoordinateWatcher()
        {
            try
            {
                mapControl.Items.Remove(userPushpin);
                actualLocation.StatusChanged -= locationStatusChanged;
                actualLocation.PositionChanged -= locationPositionChanged;
                actualLocation.Dispose();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region .:.Métodos Privados.:.
        private void locationStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                userPushpin = new Pushpin();
                addPushpin(userPushpin, actualLocation.Position.Location);
                map.SetView(actualLocation.Position.Location, 17.0);
            }
        }

        private void locationPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (userPushpin == null)
                return;

            mapControl.Items.Remove(userPushpin);

            addPushpin(userPushpin, actualLocation.Position.Location);
        }

        private void addPushpin(Pushpin pushpin, GeoCoordinate location)
        {
            pushpin.Location = location;
            mapControl.Items.Add(pushpin);
            map.SetView(actualLocation.Position.Location, 17.0);
        }
        #endregion

    }
}
