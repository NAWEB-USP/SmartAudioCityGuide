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
using Windows.Phone.Speech.Recognition;
using System.Text;

namespace SmartAudioApp
{
    public partial class SendMessage : PhoneApplicationPage
    {
        private Sound sound = new Sound();
        private CommentAndLocation commentLocation;
        private MyMicrophone myMicrophone;
        private string itemSelected = "";

        public SendMessage()
        {
            InitializeComponent();
            Thread.Sleep(2000);
            sound.play("confirm");
            this.commentLocation = (Application.Current as App).commentAndLocationShared;
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

        private async void listenAgainHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            /*
            //SpeechRecognizerUI class, engine + ui.
            var recognizerWithUI = new SpeechRecognizerUI();
            // Activate recognition (with the dictation grammar by default). Get result
            SpeechRecognitionUIResult recognizerResult = await recognizerWithUI.RecognizeWithUIAsync();
            // Display the result in a TextBox.
            MessageBox.Show(string.Format("I heard {0}.", recognizerResult.RecognitionResult.Text));
            */

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
            sound.play("messagesuccess");

            Thread thread = new Thread(new ThreadStart((Action)(() =>
            {
                try
                {
                    Thread.Sleep(10000);
                    sound.play("arvoresLindas");
                    Thread.Sleep(10000);
                    sound.play("historicoParqueTrianon");
                }
                catch
                {
                }
            })));

            thread.Start();

            //Moq
            //commentLocation.sendCommentAndSoundToActualLocationToSave(" ", myMicrophone);
            NavigationService.GoBack();
        }



    }
}