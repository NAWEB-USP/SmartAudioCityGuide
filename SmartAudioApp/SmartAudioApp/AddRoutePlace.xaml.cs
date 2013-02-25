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
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using System.Text;

namespace SmartAudioApp
{
    public partial class AddRoutePlace : PhoneApplicationPage
    {
        private Sound sound = new Sound();
        private double lat;
        private double lon;
        private MyMicrophone myMicrophone;
        private string itemSelected = "";


        public AddRoutePlace()
        {
            InitializeComponent();
            Thread.Sleep(2000);
            sound.play("confirm");
            this.lat = (Application.Current as App).latShared;
            this.lon = (Application.Current as App).lonShared;
            this.myMicrophone = (Application.Current as App).myMicrophoneShare;
            Thread.Sleep(500);
            LayoutRoot.DoubleTap += DoubleTap;
        }

        private void DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (itemSelected == "yes")
            {
                yesHold(sender, e);
            }
            else if (itemSelected == "no")
            {
                noHold(sender, e);
            }
            else if (itemSelected == "listen")
            {
                listenAgainHold(sender, e);
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("confirm");
        }


        private void sound_yes(object sender, MouseEventArgs e)
        {
            sound.play("yes");
            itemSelected = "yes";
        }

        private void sound_listenAgain(object sender, MouseEventArgs e)
        {
            sound.play("listen");
            itemSelected = "listen";
        }

        private void sound_no(object sender, MouseEventArgs e)
        {
            sound.play("no");
            itemSelected = "no";
        }

        private void listenAgainHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Thread.Sleep(1500);
            Microphone mic = Microphone.Default;
            SoundEffect soundEffect = new SoundEffect(myMicrophone.streamMicrophone.ToArray(), mic.SampleRate, AudioChannels.Mono);
            myMicrophone.streamMicrophone.Seek(0, System.IO.SeekOrigin.Begin);
            soundEffect.Play(1.0f, 0.0f, 0.0f);
        }

        private void noHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            sound.play("routemode");
            NavigationService.GoBack();
        }

        private string convertBytesToString(byte[] bytes)
        {
            string stringResult = "";
            stringResult = Convert.ToBase64String(bytes);
            return stringResult;
        }

        private void yesHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            coordinatesAndSound item = new coordinatesAndSound();
            Database db = new Database();
            string soundText = convertBytesToString(myMicrophone.streamMicrophone.ToArray());
            item = new coordinatesAndSound(lat, lon, soundText);
            db.addItemAndReturnId(item);
            sound.play("routemode");
            NavigationService.GoBack();
        }



    }
}