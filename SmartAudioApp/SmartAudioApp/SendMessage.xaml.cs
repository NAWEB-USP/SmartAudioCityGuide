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

namespace SmartAudioApp
{
    public partial class SendMessage : PhoneApplicationPage
    {
        private Sound sound = new Sound();
        private CommentAndLocation commentLocation;
        private MyMicrophone myMicrophone;


        public SendMessage()
        {
            InitializeComponent();
            Thread.Sleep(2000);
            sound.play("confirm");
            this.commentLocation = (Application.Current as App).commentAndLocationShared;
            this.myMicrophone = (Application.Current as App).myMicrophoneShare;
            Thread.Sleep(500);
        }


        private void sound_yes(object sender, MouseEventArgs e)
        {
            sound.play("yes");
        }

        private void sound_listenAgain(object sender, MouseEventArgs e)
        {
            sound.play("listen");
        }

        private void sound_no(object sender, MouseEventArgs e)
        {
            sound.play("no");
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
            sound.play("map");
            NavigationService.GoBack();
        }

        private void yesHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            sound.play("map");
            commentLocation.sendCommentAndSoundToActualLocationToSave(" ", myMicrophone);
            NavigationService.GoBack();
        }



    }
}