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

namespace SmartAudioApp
{
    public partial class Login : PhoneApplicationPage
    {
        private Sound sound = new Sound();

        public Login()
        {
            InitializeComponent();
            sound.play("login_mode");
            Thread.Sleep(500);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("menu");
        }

        private void register(object sender, RoutedEventArgs e)
        {
            sound.play("register");
            //NavigationService.Navigate(new Uri("/LoginUser.xaml", UriKind.Relative));
        }

        private void registerWithFB(object sender, RoutedEventArgs e)
        {
            sound.play("registerwithfb");
        }

        private void registerWithFBHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
        }
    }
}