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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Controls;
using System.Windows.Threading;
using System.IO;

namespace SmartAudioApp
{
    public class MyMicrophone
    {
        public Microphone microphone = Microphone.Default;
        public byte[] bufferMicrophone;
        public MemoryStream streamMicrophone = new MemoryStream();
        public SoundEffect soundMicrophone;

        public void initializeMicrophone()
        {
            // Timer to simulate the XNA Game Studio game loop (Microphone is from XNA Game Studio)
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();

            microphone.BufferReady += new EventHandler<EventArgs>(microphone_BufferReady);
        }

        public void removeMicrophone()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick -= delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Stop();
            microphone.BufferReady -= new EventHandler<EventArgs>(microphone_BufferReady);
        }

        private void microphone_BufferReady(object sender, EventArgs e)
        {
            microphone.GetData(bufferMicrophone);
            streamMicrophone.Write(bufferMicrophone, 0, bufferMicrophone.Length);
        }


    }
}
