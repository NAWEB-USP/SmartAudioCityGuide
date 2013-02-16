using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;
using System.Threading;
using TranslatorService.Speech;

namespace SmartAudioCityGuide.Controllers
{
    public class UserController : Controller
    {        
        private Users userSession = new Users();
        private CryptographyController cryptographyController = new CryptographyController();
        private EmailController emailController = new EmailController();
        private ICommentServices commentServices;
        private IUserServices userServices;

        public UserController()
        {
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
            userServices = new UserServices(new SmartAudioCityGuideEntities());
        }

        public UserController(ICommentServices _commentServices, IUserServices _userServices)
        {
            commentServices = _commentServices;
            userServices = _userServices;
        }

        // 
        // GET: /User/
        [Permissions]
        public ActionResult Index()
        {
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            List<Comments> commentsFromUser = new List<Comments>();
            string speak = "";
            try
            {
                commentsFromUser = commentServices.findCommentsByIdUser(userSession.currentUser.id);
            }
            catch
            {

            }

            ViewData["commentsFromUser"] = commentsFromUser;

            try
            {
                speak = "Welcome " + userSession.currentUser.name.ToString();
            }
            catch
            {

            }
            Thread thread = new Thread(new ThreadStart((Action)(() =>
            {
                try
                {
                    speech.Speak(speak, "en");
                }
                catch (Exception)
                {
                }

            }
             )));

            thread.Start();
            return View();
        }

        [Permissions]
        public ActionResult logOut()
        {
            try
            {
                userSession.currentLocation = null;
            }
            catch
            {

            }
            try
            {
                userSession.currentUser = null;
            }
            catch
            {

            }

            return RedirectToAction("index", "Home");
        }

        public ActionResult connect(Users user)
        {
            Users userToEnter = new Users();
            userToEnter = userServices.findUserByUserName(user.userName);

            if (userToEnter == null)
            {
                ModelState.AddModelError("user", "User not found.");
                TempData["ModelState"] = ModelState;
                return RedirectToAction("indexWithoutVoice", "Home");
            }

            if (userToEnter.password != cryptographyController.getMD5Hash(user.password))
            {
                ModelState.AddModelError("password", "Wrong password.");
                TempData["ModelState"] = ModelState;
                return RedirectToAction("indexWithoutVoice", "Home");
            }

            if(userToEnter.authenticate == 0)
            {
                //ModelState.AddModelError("authentication", "Authentication has been sent again to your e-mail.");
                // emailController.sendEmailToAuthenticateAUser(userToEnter);
                //TempData["ModelState"] = ModelState;               
                return RedirectToAction("indexWithoutVoice", "Home");
            }

            try
            {
                userSession.currentUser = userToEnter;
            }
            catch
            {

            }
            return RedirectToAction("index","User");
        }

        [HttpGet]
        public ActionResult register()
        {
            ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            return View();
        }

        [HttpPost]
        public ActionResult register(Users user, string confirmPassword)
        {
            if (!ModelState.IsValid)
                return View("register");

            if (confirmPassword != user.password)
            {
                ModelState.AddModelError("password", "Password are not the same.");
                ModelState.AddModelError("confirmPassword", " ");
                TempData["ModelState"] = ModelState;
                return View("register");
            }

            bool alreadyHasThisUser = userServices.hasAUserWithThisEmail(user.userName);

            if (alreadyHasThisUser == true)
            {
                ModelState.AddModelError("userName", "Already has this user.");
                TempData["ModelState"] = ModelState;
                return View("register");
            }

            user.password = cryptographyController.getMD5Hash(user.password);
            user.hash = cryptographyController.getMD5Hash(user.userName + user.password + DateTime.Now.ToString());
            user.authenticate = 1;
            userServices.addUser(user);

            //emailController.sendEmailToAuthenticateAUser(user);

            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public ActionResult authenticateUser(string hash)
        {
            userServices.athenticateUserWithHash(hash);
            return RedirectToAction("indexAfterRegister", "Home");
        }

        public ActionResult logOff()
        {
            try
            {
                userSession.currentUser = null;
            }
            catch
            {

            }
            return RedirectToAction("Index", "Home");
        }
    }
}
