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
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using System.Globalization;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using SQLiteClient;
using System.IO;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;

namespace SmartAudioApp
{
    public partial class Route : PhoneApplicationPage
    {
        private MyMicrophone myMicrophone = new MyMicrophone();
        private Sound sound = new Sound();
        private MyGPS myGPS = new MyGPS();
        private Database database = new Database();
        private string itemSelected = "";
        public Route()
        {
            myMicrophone.initializeMicrophone();
            InitializeComponent();
            sound.play("routemode");
            Menu.DoubleTap += DoubleTap;
            Thread.Sleep(500);
           
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            myMicrophone.removeMicrophone();
            sound.play("menu");
        }


        private void sound_Location(object sender, MouseEventArgs e)
        {
            sound.play("registerlocation");
            itemSelected = "registerlocation";
        }

        private void sound_ListLocations(object sender, MouseEventArgs e)
        {
            sound.play("locationlist");
            itemSelected = "locationlist";
        }

        private void DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (itemSelected == "locationlist")
            {
                holdListLocations(sender, e);
            }
            if (itemSelected == "registerlocation")
            {
                holdOnAddLocation(sender, e);
            }
        }
        
        public void initializeGameTimer()
        {
            Microsoft.Xna.Framework.GameTimer gameTimer = new Microsoft.Xna.Framework.GameTimer();
            gameTimer.UpdateInterval = TimeSpan.FromMilliseconds(33);

            gameTimer.Update += delegate { try { FrameworkDispatcher.Update(); } catch { } };

            gameTimer.Start();

            FrameworkDispatcher.Update();
        }

        
        private void holdOnAddLocation(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (myMicrophone.microphone.State == MicrophoneState.Stopped)
            {
                myMicrophone.streamMicrophone = new MemoryStream();

                myMicrophone.microphone.BufferDuration = TimeSpan.FromMilliseconds(1000);
                myMicrophone.bufferMicrophone = new byte[myMicrophone.microphone.GetSampleSizeInBytes(myMicrophone.microphone.BufferDuration)];

                sound.play("recording");

                Thread.Sleep(1000);

                myMicrophone.microphone.Start();
            }
            else
            {
                holdOffAddLocation(sender, e);
            }
        }

        private void holdOffAddLocation(object sender, System.Windows.Input.GestureEventArgs e)
        {           
            if (myMicrophone.microphone.State == MicrophoneState.Started)
            {
                myMicrophone.microphone.Stop();
                sound.play("recordingend");

                if (myMicrophone.streamMicrophone.Length != 0)
                {
                    (Application.Current as App).latShared = myGPS.actualLocation.Position.Location.Latitude;
                    (Application.Current as App).lonShared = myGPS.actualLocation.Position.Location.Longitude;
                    (Application.Current as App).myMicrophoneShare = myMicrophone;
                    NavigationService.Navigate(new Uri("/AddRoutePlace.xaml", UriKind.Relative));

                }
            }
        }

        private void addLocation(object sender, System.Windows.Input.GestureEventArgs e)
        {

            
            /* Record the microphone */
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (myMicrophone.microphone.State == MicrophoneState.Stopped)
                {
                    myMicrophone.streamMicrophone = new MemoryStream();

                    myMicrophone.microphone.BufferDuration = TimeSpan.FromMilliseconds(1000);
                    myMicrophone.bufferMicrophone = new byte[myMicrophone.microphone.GetSampleSizeInBytes(myMicrophone.microphone.BufferDuration)];

                    sound.play("recording");

                    Thread.Sleep(1000);

                    myMicrophone.microphone.Start();
                }
                /* Stop the record */
                else if (myMicrophone.microphone.State == MicrophoneState.Started)
                {
                    myMicrophone.microphone.Stop();
                    sound.play("recordingend");

                    if (myMicrophone.streamMicrophone.Length != 0)
                    {
                        (Application.Current as App).latShared = myGPS.actualLocation.Position.Location.Latitude;
                        (Application.Current as App).lonShared = myGPS.actualLocation.Position.Location.Longitude;
                        (Application.Current as App).myMicrophoneShare = myMicrophone;
                        NavigationService.Navigate(new Uri("/AddRoutePlace.xaml", UriKind.Relative));

                    }
                }
            }
            else sound.play("internet");
        }

        private void holdListLocations(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ListRoute.xaml", UriKind.Relative));
        }

    }
}