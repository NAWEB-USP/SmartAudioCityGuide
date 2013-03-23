using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.IO;
using System.Text;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Threading;
using System.Windows.Resources;
using Hawaii.Services.Client.Speech;
using SpeechRecognitionTestClient;
using Hawaii.Services.Client;
using Microsoft.Phone.Info;
using TranslatorService.Speech;
using System.Globalization;
using SmartAudioApp.ServicesReference;
using System.ServiceModel;
using System.Net.NetworkInformation;
using Windows.Phone.Speech.Recognition;

namespace SmartAudioApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MyMicrophone myMicrophone = new MyMicrophone();
        private Sound sound = new Sound();
        private GlobalPositioningSystemForMap globalPositionSystemForMap;
        private MyPhone myPhone = new MyPhone();
        private string baseWebserver = Properties.getServerIP();
        private Comments comments;
        private Locations locations;
        private string itemSelected = "";
        private SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
      
        private WebService1SoapClient webService = new WebService1SoapClient(
        new BasicHttpBinding(BasicHttpSecurityMode.None)
        {
            MaxReceivedMessageSize = 2147483647,
            MaxBufferSize = 2147483647
        },
        new EndpointAddress(Properties.getServerIP() + "WebServices.asmx"));

        // Constructor
        public MainPage()
        {
           /*
            Database database = new Database();
            LocalDataBaseForLatLongAndSound item = new LocalDataBaseForLatLongAndSound(1.0, 1.0, "!@312312");
            database.createTableForLatLongAndSound();
            int idItem = database.addItemAndReturnId(item);
            database.getListItems();
            database.findLocalDataBaseForLatLongAndSoundById(idItem);
            database.removeLocalDataBaseForLatLongAndSound(idItem);
            */

            comments = new Comments(baseWebserver);

            InitializeComponent();

            initializeGameTimer();

            sound.play("welcome");

            System.Threading.Thread.Sleep(2000);

            sound.play("map");

            panorama.SelectionChanged += SelectionChanged;

            Menu.DoubleTap += DoubleTap;


            Thread thread = new Thread(new ThreadStart((Action)(() =>
            {
                try
                {
                    List<int> locationsAlreadySaid = new List<int>();
                    int count = 0;

                    while (true)
                    {
                        Thread.Sleep(2000);

                        while (globalPositionSystemForMap == null)
                        {
                            Thread.Sleep(1000);
                        }

                        if (globalPositionSystemForMap.userPushpin == null)
                            continue;

                        if (count == 0 || locations.locationsNearUser.Count == 0)
                        {
                            locations.getLocationsAround();
                        }


                        if (count++ > 100)
                            count = 0;

                        if (locations.locationsNearUser == null)
                            continue;

                        foreach (Locations location in locations.locationsNearUser)
                        {
                            double distance = location.distanceTo(globalPositionSystemForMap.actualLocation.Position.Location.Longitude, globalPositionSystemForMap.actualLocation.Position.Location.Latitude);

                            if (distance < 0.0004)
                            {
                                if (locationsAlreadySaid.Contains(location.id) == false)
                                {
                                    locationsAlreadySaid.Add(location.id);

                                    comments.getCommentFromLocationIdAndTypeOfComment(location.id,(Application.Current as App).idMessageType);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            })));
            thread.Start();
        }

        /*public void onCompletedSpeechRecognition(SpeechServiceResult speechResult)
        {
            CommentAndLocation commentAndLocation = new CommentAndLocation(globalPositionSystemForMap, baseWebserver);

            if (speechResult.Status != Status.Success)
                return;

            string str;
            try
            {
                str = speechResult.SpeechResult.Items[0];
            }
            catch (Exception)
            {
                return;
            }

            if (str == "locate people")
                sendRequestToLocatePeople();
            else
                commentAndLocation.sendCommentAndSoundToActualLocationToSave(str,myMicrophone);
        }

        public void sendRequestToLocatePeople()
        {
        }*/

        public void initializeGameTimer()
        {
            GameTimer gameTimer = new GameTimer();
            gameTimer.UpdateInterval = TimeSpan.FromMilliseconds(33);

            gameTimer.Update += delegate { try { FrameworkDispatcher.Update(); } catch { } };

            gameTimer.Start();

            FrameworkDispatcher.Update();
        }

        private void mapControl_Loaded(object sender, RoutedEventArgs e)
        {
            globalPositionSystemForMap = new GlobalPositioningSystemForMap(mapControl, map);

            locations = new Locations(globalPositionSystemForMap, baseWebserver);

            globalPositionSystemForMap.initializeGeoCoordinateWatcher();

        }

        private void DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(itemSelected == "login")
            {
                holdLogin(sender,e);
            }
            else if (itemSelected == "route")
            {
                holdRoute(sender, e);
            }
            else if (itemSelected == "friend")
            {
                holdHelp(sender, e);
            }
            else if (itemSelected == "mode")
            {
                holdMode(sender, e);
            }
            else if (itemSelected == "where")
            {
                holdWhere(sender, e);
            }

        
        }

        private void sound_login(object sender, MouseEventArgs e)
        {
            sound.play("login");
            itemSelected = "login";
        }
        
        private void sound_route(object sender, MouseEventArgs e)
        {
            sound.play("route");
            itemSelected = "route";
        }

        private void sound_help(object sender, MouseEventArgs e)
        {
            sound.play("friend");
            itemSelected = "friend";
        }

        private void sound_mode(object sender, MouseEventArgs e)
        {
            if ((Application.Current as App).idMessageType == 1) sound.play("explore");
            else sound.play("normal");
            itemSelected = "mode";
        }

        private void sound_Where(object sender, MouseEventArgs e)
        {
            sound.play("where");
            itemSelected = "where";
        }

        private void SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (panorama.SelectedIndex == 0)
            {
                sound.play("map");
                globalPositionSystemForMap = new GlobalPositioningSystemForMap(mapControl, map);
                globalPositionSystemForMap.initializeGeoCoordinateWatcher();
            }
            else if (panorama.SelectedIndex == 1)
            {
                globalPositionSystemForMap.removeGeoCoordinateWatcher();
                sound.play("menu");
            }
        }

        private void map_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            /* Record the microphone */
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (myMicrophone.microphone.State == MicrophoneState.Stopped)
                {
                    myMicrophone.initializeMicrophone();
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
                    CommentAndLocation commentAndLocation = new CommentAndLocation(globalPositionSystemForMap, baseWebserver);
                    sound.play("recordingend");

                    //if (myMicrophone.streamMicrophone.Length != 0) SpeechService.RecognizeSpeechAsync(HawaiiClient.HawaiiApplicationId, SpeechService.DefaultGrammar , myMicrophone.streamMicrophone.ToArray(), onCompletedSpeechRecognition);
                    if (myMicrophone.streamMicrophone.Length != 0)
                    {
                        (Application.Current as App).commentAndLocationShared = commentAndLocation;
                        (Application.Current as App).myMicrophoneShare = myMicrophone;
                        NavigationService.Navigate(new Uri("/SendMessage.xaml", UriKind.Relative));
                    }
                    myMicrophone.removeMicrophone();
                }
            }
            else sound.play("internet");
        }

        private void holdRoute(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NavigationService.Navigate(new Uri("/Route.xaml", UriKind.Relative));
            }
            else sound.play("internet");
        }

        
        private void holdLogin(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                webService.existAUserForThatIdPhoneAsync(myPhone.serializedDeviceUniqueId());
                webService.existAUserForThatIdPhoneCompleted += new EventHandler<existAUserForThatIdPhoneCompletedEventArgs>(webService_existAUserForThatIdPhoneCompleted);
            }
            else sound.play("internet");
            
        }

        void webService_existAUserForThatIdPhoneCompleted(object sender, existAUserForThatIdPhoneCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                NavigationService.Navigate(new Uri("/ShowUser.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        
        private void holdMode(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((Application.Current as App).idMessageType == 1)
            {
                (Application.Current as App).idMessageType = 2;
                ((mode.Content as Grid).Children[0] as TextBlock).Text = SmartAudioApp.Resources.normal;
                sound.play("changedexplore");
            }
            else
            {
                (Application.Current as App).idMessageType = 1;
                ((mode.Content as Grid).Children[0] as TextBlock).Text = SmartAudioApp.Resources.explore;
                sound.play("changednormal");
            }
            locations.getLocationsAround(); 
        }

        private void holdHelp(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NavigationService.Navigate(new Uri("/HelpModeContact.xaml", UriKind.Relative));
            }
            else sound.play("internet");
        }


        private void holdWhere(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                Thread thread = new Thread(new ThreadStart((Action)(() =>
                {
                    try
                    {
                        SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
                        Routes routesServices = new Routes(Properties.getServerIP());
                        MyGPS gps = new MyGPS();
                        CultureInfo culture = new CultureInfo("en-US");
                        double currentLatitude = gps.actualLocation.Position.Location.Latitude;
                        double currentLongitude = gps.actualLocation.Position.Location.Longitude;
                        int esc = 60;
                        routesServices.getAddressFromCoordinates(currentLatitude.ToString(culture) + "," + currentLongitude.ToString(culture));
                        while (routesServices.routeDone == false && esc > 0)
                        {
                            Thread.Sleep(70);
                            esc--;
                        }
                        if (routesServices.country == null || routesServices.city == null || esc<0)
                        {
                            sound.play("error");
                            return;
                        }
                        speech.SpeakAsync(routesServices.street, CultureInfo.CurrentCulture.ToString());
                    }
                    catch(Exception)
                    {
                    }
                }))); thread.Start();
            }
            else
            {
                sound.play("internet");
            }

        }

        private async void recognitionMethod(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SpeechRecognizerUI recoWithUI = new SpeechRecognizerUI();
            recoWithUI.Recognizer.Grammars.AddGrammarFromList("answer", new string[] { "Help", "Record", "Where", "Change mode", "Quick start", "Promotions" });
            recoWithUI.Recognizer.AudioCaptureStateChanged += myRecognizer_AudioCaptureStateChanged;
            SpeechRecognitionUIResult recoResult = await recoWithUI.RecognizeWithUIAsync();
            if (recoResult.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
            {
                if (recoResult.RecognitionResult.Text == "Where")
                {
                    holdWhere(sender, e);
                }
                else if (recoResult.RecognitionResult.Text == "Change mode")
                {
                    holdMode(sender, e);
                }
                else if (recoResult.RecognitionResult.Text == "Quick start")
                {
                    playSound("In that page you can touch for a time in the screen and make a mesage to another person and to finish the message you need to touch in the screen for a time again. After that, you will be at a confirmation page, which you have itens and to select these itens, you need to pass the finger from top to bottom and when you listen the item that you wana to select you need to double tap the screen. If you are on map, you can change to menu screen making a slice from left to right on screnn and you can select and chose the item passing the finger from top to bottom and if you wana to select the item you make a double tap on screen.");
                }
                else if (recoResult.RecognitionResult.Text == "Promotions")
                {
                    playSound("We have a promotion near here for 10% dicount Pizza Park, it is 0.5 km from here. If you wana to have the discount, only say you are a Smart Audio user.");
                }
                else if (recoResult.RecognitionResult.Text == "Help")
                {
                    playSound("Actions that can be made: Record, Where, Change Mode, Quick Start and Promotions");
                }
            }
            else if (recoResult.ResultStatus == SpeechRecognitionUIStatus.Cancelled)
            {
                sound.play("map");
            }

        }

        // Detect capture state changes and write the capture state to the text block.
        void myRecognizer_AudioCaptureStateChanged(SpeechRecognizer sender, SpeechRecognizerAudioCaptureStateChangedEventArgs args)
        {
            if (args.State == SpeechRecognizerAudioCaptureState.Capturing)
            {
                //playSound("Say an action or say help for more information");
            }
            else if (args.State == SpeechRecognizerAudioCaptureState.Inactive)
            {
                //playSound("No word processing");
            }
        }


        private void playSound(string sound)
        {
            speech.SpeakAsync(sound, CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString());
        }
    }
}