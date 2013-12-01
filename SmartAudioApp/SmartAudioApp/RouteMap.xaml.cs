using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Threading;
using System.Globalization;
using Microsoft.Xna.Framework.Audio;
using TranslatorService.Speech;
using System.Device.Location;
using System.Text.RegularExpressions;




namespace SmartAudioApp
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public partial class RouteMap : PhoneApplicationPage
    {
        #region .:.Propriedades.:.
        private List<Routes> routes;
        private Sound sound = new Sound();
        private string address;
        public event ChangedEventHandler Changed;
        public Thread thread;
        MapLayer myRouteLayer;
        private double currentLatitude, currentLongitude;
        private double epsilon = 5;
        bool endThread;
        #endregion

        #region .:.Inicializadores.:.
        public RouteMap()
        {
            InitializeComponent();
            Changed += new ChangedEventHandler(routeService_CalculateRouteCompleted);
            endThread = false;
            getRoute();
        }
        #endregion
        
        #region .:.Métodos Protected.:.
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            endThread = true;
            sound.play("selectlocation");
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            address = string.Empty;

            if (NavigationContext.QueryString.TryGetValue("address", out address))
            {
                if (address.Length == 0) address = "18e18e9qs91js197217s19s12h2";
                getRoute();
            }
        }
        #endregion

        #region .:.Métodos Privados.:.
        private void error()
        {
            sound.play("error");
            System.Threading.Thread.Sleep(750);
            sound.play("selectlocation");
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                NavigationService.GoBack();
            }
            );
        }


        private double distance(double latitude, double longitude)
        {
            GeoCoordinate coord = new GeoCoordinate(Convert.ToDouble(currentLatitude),Convert.ToDouble(currentLongitude));
            GeoCoordinate dest_coord = new GeoCoordinate(latitude,longitude);
            return coord.GetDistanceTo(dest_coord);
        }



        private void getRoute()
        {
            Routes routesServices = new Routes(Properties.getServerIP());
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");                            
            MyGPS gps = new MyGPS();
            CultureInfo culture = new CultureInfo("en-US");
            double dist,mindist;
            string orientation;
            double destLat = (Application.Current as App).latShared;
            double destLon = (Application.Current as App).lonShared;
            int cont = 0;
            thread = new Thread(new ThreadStart((Action)(() =>
            {
                try
                {

                    do
                    {
                        orientation = gps.actualLocation.Position.Location.Course.ToString(culture);
                        if (orientation == "NaN") orientation = "0";

                        currentLatitude = gps.actualLocation.Position.Location.Latitude;
                        currentLongitude = gps.actualLocation.Position.Location.Longitude;

                        routesServices.getAddressFromCoordinates(currentLatitude.ToString(culture) + "," + currentLongitude.ToString(culture));
                        while (routesServices.routeDone == false && cont < 100)
                        {
                            cont++;
                            Thread.Sleep(70);
                        }
                        if (routesServices.country == null || routesServices.city == null)
                        {
                            error();
                            return;
                        }
                        cont = 0;
                        routesServices.Route(currentLatitude.ToString(culture) + "," + currentLongitude.ToString(culture) + ";" + destLat.ToString(culture) + "," + destLon.ToString(culture) + " ; " + orientation + " ; " + CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                        while (routesServices.routeDone == false && cont<100)
                        {
                            cont++;
                            Thread.Sleep(70);
                        }
                        if (routesServices.route == null)
                        {
                            error();
                            return;
                        }
                        routes = routesServices.route;

                        dist = distance(routesServices.route[0].lat, routesServices.route[0].lon);
                        mindist = dist;

                        while (((mindist + epsilon) > dist) && routes.Count > 0 && endThread == false)
                        {

                            currentLatitude = gps.actualLocation.Position.Location.Latitude;
                            currentLongitude = gps.actualLocation.Position.Location.Longitude;

                            dist = distance(routesServices.route[0].lat, routesServices.route[0].lon);
                            if (mindist > dist) mindist = dist;

                            routesServices.getAddressFromCoordinates(currentLatitude.ToString(culture) + "," + currentLongitude.ToString(culture));

                            if (dist < epsilon)
                            {
                                routesServices.route.RemoveAt(0);
                                if (routesServices.route.Count > 0)
                                {
                                    dist = distance(routesServices.route[0].lat, routesServices.route[0].lon);
                                    mindist = dist;
                                }
                                if (routesServices.route.Count == 0)
                                {
                                    sound.play("success");
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        NavigationService.GoBack();
                                    }
                                    );
                                    return;
                                }
                            }
                            /* Threads comuns nao podem alterar a UI, entao é necessario criar o evento como a thread de UI */
                            if (endThread == false)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    OnChanged(EventArgs.Empty);
                                }
                                );
                                speech.SpeakAsync(routesServices.street + ". " + routesServices.route[0].description + ". In " + Convert.ToInt32(dist).ToString()  + " meters", CultureInfo.CurrentCulture.ToString());
                                Thread.Sleep(15000);
                            }
                        }
                    } while (routes.Count > 0 && endThread == false);
                    if (endThread == false)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                sound.play("success");
                                System.Threading.Thread.Sleep(750);
                                sound.play("selectlocation");
                                NavigationService.GoBack();
                            }
                            );
                        return;
                    }

                }
                /* Mandar mensagem de erro e voltar para pagina anterior */
                catch (Exception)
                {
                    if(endThread == false) error();
                    return;
                }
            })));
            thread.Start();
        }

        // This is the callback method for the CalculateRoute request.
        private void routeService_CalculateRouteCompleted(object sender, EventArgs e)
        {
            try
            {
                // Set properties of the route line you want to draw.
                Color routeColor = Colors.Blue;
                SolidColorBrush routeBrush = new SolidColorBrush(routeColor);
                MapPolyline routeLine = new MapPolyline();
                CultureInfo culture = new CultureInfo("en-US");
                routeLine.Locations = new LocationCollection();
                routeLine.Stroke = routeBrush;
                routeLine.Opacity = 0.65;
                routeLine.StrokeThickness = 5.0;


                try
                {
                    mapR.Children.Remove(myRouteLayer);
                }
                catch (Exception)
                {
                }



                routeLine.Locations.Add(new System.Device.Location.GeoCoordinate(currentLatitude,currentLongitude));
                // Retrieve the route points that define the shape of the route.

                for (int i = 0; i < routes.Count; i++ )
                {
                    routeLine.Locations.Add(new System.Device.Location.GeoCoordinate(routes[i].lat, routes[i].lon));
                }


                // Add a map layer in which to draw the route.
                myRouteLayer = new MapLayer();
                mapR.Children.Add(myRouteLayer);

                // Add the route line to the new layer.
                myRouteLayer.Children.Add(routeLine);

                // Figure the rectangle which encompasses the route. This is used later to set the map view.
                LocationRect rect = new LocationRect(new System.Device.Location.GeoCoordinate(currentLatitude, currentLongitude), 0.0020562484860420227, 0.0041788816452026367);

                Ellipse point = new Ellipse();
                point.Width = 10;
                point.Height = 10;
                point.Fill = new SolidColorBrush(Colors.Blue);
                point.Opacity = 0.65;
                Location location = new Location();
                location.Latitude = currentLatitude;
                location.Longitude = currentLongitude;
                MapLayer.SetPosition(point, location);
                MapLayer.SetPositionOrigin(point, PositionOrigin.Center);

                Ellipse pointn = new Ellipse();
                pointn.Width = 10;
                pointn.Height = 10;
                pointn.Fill = new SolidColorBrush(Colors.Yellow);
                pointn.Opacity = 0.65;
                Location locationn = new Location();
                locationn.Latitude = routes[0].lat;
                locationn.Longitude = routes[0].lon;
                MapLayer.SetPosition(pointn, locationn);
                MapLayer.SetPositionOrigin(pointn, PositionOrigin.Center);

                Ellipse pointf = new Ellipse();
                pointf.Width = 10;
                pointf.Height = 10;
                pointf.Fill = new SolidColorBrush(Colors.Red);
                pointf.Opacity = 0.65;
                Location locationf = new Location();
                locationf.Latitude = routes[routes.Count-1].lat;
                locationf.Longitude = routes[routes.Count - 1].lon;
                MapLayer.SetPosition(pointf, locationf);
                MapLayer.SetPositionOrigin(pointf, PositionOrigin.Center);

                // Add the drawn point to the route layer.                    
                myRouteLayer.Children.Add(point);
                if(routes.Count-1 != 0) myRouteLayer.Children.Add(pointn);
                myRouteLayer.Children.Add(pointf);

                // Set the map view using the rectangle which bounds the rendered route.
                mapR.SetView(rect);
                //map.Center = new System.Device.Location.GeoCoordinate(routes[0].lat, routes[0].lon);
            }
            catch (Exception)
            {
                NavigationService.GoBack();
            }
        }
        #endregion

    }
}