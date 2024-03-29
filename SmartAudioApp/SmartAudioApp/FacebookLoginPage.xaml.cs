﻿using System;
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
using Facebook;

namespace SmartAudioApp
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        #region .:.Propriedades.:.

        private const string AppId = "341770699195775";
        private Sound sound = new Sound();

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,read_stream,email";

        private CookieContainer _cookieContainer = new CookieContainer();
        private Uri loginUrl;
        private readonly FacebookClient _fb = new FacebookClient();

        #endregion

        #region .:.Inicializadores.:.
        public FacebookLoginPage()
        {
            InitializeComponent();
        }
        #endregion

        #region .:.Métodos Protected.:.
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            sound.play("login_mode");
        }
        #endregion

        #region .:.Métodos Privados.:.
        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
            webBrowser1.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var id = (string)result["id"];
                var email = (string)result["email"];
                var name = (string)result["name"];

                var url = string.Format("/SelectTheVisionProblem.xaml?access_token={0}&id={1}&name={2}&email={3}", accessToken, id, name, email);

                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri(url, UriKind.Relative)));
            };

            fb.GetAsync("me");
        }

        public void ClearCookies(Uri uri)
        {
            var cookies = this._cookieContainer.GetCookies(uri);

            foreach (Cookie cookie in cookies)
            {
                cookie.Discard = true;
                cookie.Expired = true;
            }

        }
        #endregion
    }
}