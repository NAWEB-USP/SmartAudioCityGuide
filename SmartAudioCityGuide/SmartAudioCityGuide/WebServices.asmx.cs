using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SmartAudioCityGuide.Controllers;
using SmartAudioCityGuide.Models;
using System.Web.Script.Serialization;
using SmartAudioCityGuide.Services;

namespace SmartAudioCityGuide
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private IUserLocationServices userLocationServices;
        private IUserServices userServices;
        private ICommentServices commentServices;
        private CryptographyController cryptographyController = new CryptographyController();
        private EmailController emailController = new EmailController();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        public WebService1()
        {
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
            userLocationServices = new UserLocationServices(new SmartAudioCityGuideEntities());
            userServices = new UserServices(new SmartAudioCityGuideEntities());
        }

        public WebService1(IUserLocationServices _userLocationServices, IUserServices _userServices, 
            ICommentServices _commentServices)
        {
            commentServices = _commentServices;
            userLocationServices = _userLocationServices;
            userServices = _userServices;
        }

        [WebMethod]
        public void addComentAndSoundToLocation(string location, byte[] sound, Comments comments, string code)
        {
            Comments commentPassed = new Comments();
            commentPassed.sound = convertBytesToString(sound);
            commentPassed.typeOfCommentsId = comments.typeOfCommentsId;
            commentPassed.isText = comments.isText;
            commentPassed.userId = comments.userId;
            commentPassed.description = comments.description;
            string comment = serializer.Serialize(commentPassed);
            CommentsWebServicesController commentsWebServicesController = new CommentsWebServicesController();
            commentsWebServicesController.addCommentAndSoundToLocation(location, comment, code);
        }

        [WebMethod]
        public string getSoundCommentFromLocation(int idLocation, string code)
        {
            SpeechWebServicesController speechWebServicesController = new SpeechWebServicesController();
            return speechWebServicesController.sendLastSoundCommentFromIdLocation(idLocation, code);
        }

        [WebMethod]
        public string getSoundCommentFromLocationAndTypeOfComment(int idLocation,int idTypeOfComment, string code)
        {
            SpeechWebServicesController speechWebServicesController = new SpeechWebServicesController();
            return speechWebServicesController.sendLastSoundCommentFromIdLocationAndTypeOfComment(idLocation, idTypeOfComment, code);
        }

        [WebMethod]
        public void updateLoctionForUser(string windowsPhoneId,double latitude, double longitude)
        {
            userLocationServices.updateLatitudeAndLongitudeByPhoneId(windowsPhoneId, latitude, longitude);
        }

        [WebMethod]
        public void sendEmailForFolloUserByWindowsPhoneId(string windowsPhoneId,string email)
        {
            UserLocations userLocation = userLocationServices.findUserLocationByPhoneId(windowsPhoneId);
            if (userLocation == null)
            {
                userLocation = new UserLocations();
                userLocation.latitude = 0.0;
                userLocation.longitude = 0.0;
                userLocation.windowsPhoneId = windowsPhoneId;
                userLocation.requestTime = DateTime.Now;
                userLocationServices.addUserLocation(userLocation);
                userLocation = userLocationServices.findUserLocationByPhoneId(windowsPhoneId);
            }
            EmailController emailController = new EmailController();
            userLocation.hash = cryptographyController.getMD5Hash(userLocation.id.ToString());
            userLocationServices.updateHasById(userLocation.id, userLocation.hash);
            emailController.sendEmailToFollowUser(email, userLocation.hash);
        }

        [WebMethod]
        public void addNewUserForApp(string idFacebook, string accessToken, string name, string email, string phoneId, string visionProblem)
        {
            Models.Users user;
            Users newUser = new Users();
            user = userServices.findUserByPhoneId(phoneId);
            if (user != null)
            {
                newUser.acessTokenFacebook = accessToken;
                newUser.idFacebook = idFacebook;
                newUser.name = name;
                newUser.userName = email;
                userServices.updateUser(user.id, newUser);
                return;
            }

            user = new Users();
            string password = cryptographyController.randomString(6);
            user.acessTokenFacebook = accessToken;
            user.idFacebook = idFacebook;
            user.userName = email;
            user.name = name;

            switch (visionProblem)
            {
                case("totallyBlind"):
                    user.typeOfBlindness = 1;
                    break;

                case ("lowVision"):
                    user.typeOfBlindness = 2;
                    break;

                case ("partiallySighted"):
                    user.typeOfBlindness = 3;
                    break;

            }

            user.password = cryptographyController.getMD5Hash(password);
            user.hash = cryptographyController.getMD5Hash(user.name + user.password + DateTime.Now.ToString());
            user.phoneId = phoneId;
            userServices.addUser(user);

            emailController.sendPasswordToUser(user.userName, password);


        }

        [WebMethod]
        public bool existAUserForThatIdPhone(string phoneId)
        {
            Users user =  userServices.findUserByPhoneId(phoneId);
            if (user == null)
                return false;

            return true;
        }

        [WebMethod]
        public string getFacebookIdByPhoneId(string phoneId)
        {
            Users user = userServices.findUserByPhoneId(phoneId);
            if (user == null)
                return "";

            return user.idFacebook;
        }

        [WebMethod]
        public string getAcessTokenByPhoneId(string phoneId)
        {
            Users user = userServices.findUserByPhoneId(phoneId);
            if (user == null)
                return "";

            return user.acessTokenFacebook;
        }

        [WebMethod]
        public Users getUserByPhoneId(string phoneId)
        {
            Users user = userServices.findUserByPhoneId(phoneId);
            if (user == null)
                return null;

            return user;
        }

        [WebMethod]
        public string getNameByPhoneId(string phoneId)
        {
            Users user = userServices.findUserByPhoneId(phoneId);
            if (user == null)
                return "";

            return user.name;
        }
        [WebMethod]
        public string getLvlByPhoneId(string phoneId)
        {
            List<Comments> comments = commentServices.findCommentsByPhoneId(phoneId);

            return Convert.ToString(comments.Count/5 + 1);
        }


        private string convertBytesToString(byte[] bytes)
        {
            string stringResult = "";
            foreach (byte b in bytes)
            {
                stringResult = stringResult + b + ";";
            }
            stringResult = stringResult.Remove(stringResult.Length - 1, 1);

            return stringResult;
        }
    }
}
