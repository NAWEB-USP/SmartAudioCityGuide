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

namespace SmartAudioApp
{
    public partial class SelectTheVisionProblem : PhoneApplicationPage
    {
        #region .:.Propriedades.:.
        private Sound sound = new Sound();
        private MyPhone myPhone = new MyPhone();

        private string _phoneId = "";
        private string _accessToken ="";
        private string _userId = "";
        private string _name = "";
        private string _email = "";

        private WebService1SoapClient webService = new WebService1SoapClient(
        new BasicHttpBinding( BasicHttpSecurityMode.None )
        {
            MaxReceivedMessageSize = 2147483647,
            MaxBufferSize = 2147483647
        },
        new EndpointAddress( Properties.getServerIP() + "WebServices.asmx" ) );

        #endregion

        #region .:.Métodos Protected.:.
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _accessToken = NavigationContext.QueryString["access_token"];
            _userId = NavigationContext.QueryString["id"];
            _name = NavigationContext.QueryString["name"];
            _email = NavigationContext.QueryString["email"];
            _phoneId = myPhone.serializedDeviceUniqueId();

        }
        #endregion

        #region .:.Métodos Públicos.:.
        public SelectTheVisionProblem()
        {
            sound.play("selectvisiontypemenu");
            InitializeComponent();
        }
        #endregion

        #region .:.Métodos Privados.:.
        private void visionProblemPartialSighted(object sender, RoutedEventArgs e)
        {
            sound.play("partiallySighted");
        }

        private void visionProblemTotallyBlind(object sender, RoutedEventArgs e)
        {
            sound.play("totalBlindness");
        }

        private void holdVisionProblemTotallyBlind(object sender, System.Windows.Input.GestureEventArgs e)
        {
            webService.addNewUserForAppAsync(_userId, _accessToken, _name, _email, _phoneId, "totallyBlind");
            webService.addNewUserForAppCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(webService_addNewUserForAppCompleted);
        }

        private void holdVisionProblemPartiallySighted(object sender, System.Windows.Input.GestureEventArgs e)
        {
            webService.addNewUserForAppAsync(_userId, _accessToken, _name, _email, _phoneId, "partiallySighted");
            webService.addNewUserForAppCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(webService_addNewUserForAppCompleted);
        }

        void webService_addNewUserForAppCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            sound.play("map");
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        #endregion

    }
}