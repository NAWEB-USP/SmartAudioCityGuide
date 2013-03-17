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
using SmartAudioApp.ServicesReference;
using System.ServiceModel;
using TranslatorService.Speech;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace SmartAudioApp
{
    public partial class ShowUser : PhoneApplicationPage
    {
        private Users user = new Users();
        private MyPhone myPhone = new MyPhone();
        private Sound sound = new Sound();
        private int lvlOfUser = 1;

        private SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
        private WebService1SoapClient webService = new WebService1SoapClient(
        new BasicHttpBinding(BasicHttpSecurityMode.None)
        {
            MaxReceivedMessageSize = 2147483647,
            MaxBufferSize = 2147483647
        },
        new EndpointAddress(Properties.getServerIP() + "WebServices.asmx"));

        public ShowUser()
        {
            InitializeComponent();
            LoadFacebookData();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("menu");
        }


        private void LoadFacebookData()
        {
            webService.getNameByPhoneIdAsync(myPhone.serializedDeviceUniqueId());
            webService.getNameByPhoneIdCompleted += new EventHandler<getNameByPhoneIdCompletedEventArgs>(webService_getNameByPhoneIdCompleted);
            webService.getAcessTokenByPhoneIdAsync(myPhone.serializedDeviceUniqueId());
            webService.getAcessTokenByPhoneIdCompleted += new EventHandler<getAcessTokenByPhoneIdCompletedEventArgs>(webService_getAcessTokenByPhoneIdCompleted);
            webService.getFacebookIdByPhoneIdAsync(myPhone.serializedDeviceUniqueId());
            webService.getFacebookIdByPhoneIdCompleted += new EventHandler<getFacebookIdByPhoneIdCompletedEventArgs>(webService_getFacebookIdByPhoneIdCompleted);
            webService.getLvlByPhoneIdAsync(myPhone.serializedDeviceUniqueId());
            webService.getLvlByPhoneIdCompleted += new EventHandler<getLvlByPhoneIdCompletedEventArgs>(webService_getLvlByPhoneIdCompleted);
        }

        void webService_getLvlByPhoneIdCompleted(object sender, getLvlByPhoneIdCompletedEventArgs e)
        {
            int numberOfComments = Convert.ToInt32(e.Result);
            lvlOfUser = (numberOfComments / 5 + 1);
            badgeLvlText.Text = e.Result;
            levelOfUser.Content = "You are lvl " + lvlOfUser;
        }

        void webService_getFacebookIdByPhoneIdCompleted(object sender, getFacebookIdByPhoneIdCompletedEventArgs e)
        {
            user.idFacebook = e.Result;
            BitmapImage bitmapImage = new BitmapImage(new Uri("http://graph.facebook.com/" + user.idFacebook + "/picture?width=120&height=120", UriKind.Absolute));
            imageOfFacebook.Source = bitmapImage;
        }

        void webService_getAcessTokenByPhoneIdCompleted(object sender, getAcessTokenByPhoneIdCompletedEventArgs e)
        {
            user.acessTokenFacebook = e.Result;
        }

        void webService_getNameByPhoneIdCompleted(object sender, getNameByPhoneIdCompletedEventArgs e)
        {
            user.name = e.Result;
            userFacebook.Content = "Hello " + user.name;
        }

        private void tellNameOfUser(object sender, MouseEventArgs e)
        {

            speech.SpeakAsync("Hello " + user.name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString());
        }

        private void changeUserMouseEnter(object sender, MouseEventArgs e)
        {
            speech.SpeakAsync("Change user", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString());
        }

        private void tellLvlOfUser(object send, MouseEventArgs e)
        {
            speech.SpeakAsync("Your level is " + lvlOfUser, CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToString());
        }

        private void changeUserHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
        }
    }
}