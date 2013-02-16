using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;
using TranslatorService.Speech;
using System.IO;

namespace SmartAudioCityGuide.Controllers
{
    public class CommentController : Controller
    {
        private Locations location = new Locations();
        private Users user = new Users();
        private ICommentServices commentsServices;
        private ILocationServices locationServices;

        public CommentController()
        {
            commentsServices = new CommentServices(new SmartAudioCityGuideEntities());
            locationServices = new LocationServices(new SmartAudioCityGuideEntities());
        }

        public CommentController(ICommentServices _commentServices, ILocationServices _locationServices)
        {
            commentsServices = _commentServices;
            locationServices = _locationServices;
        }
        
        [Permissions]
        public ActionResult addTextComment(Comments comment,string selectList)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            Stream stream;
            byte[] bytes;
            comment.isText = false;

            if (selectList == "--Select One--")
            {
                return RedirectToAction("index", "Maps");
            }

            else if(selectList == "Normal Mode")
            {
                comment.typeOfCommentsId = 1;
            }

            else if (selectList == "Exploration Mode")
            {
                comment.typeOfCommentsId = 2;
            }


            stream = speech.GetSpeakStream(comment.description);
            bytes = ReadFully(stream);
            comment.sound = convertBytesToString(bytes);

            
            try
            {
                locationServices.addLocations(location.currentLocation);
                comment.locationsId = location.currentLocation.id;
                comment.userId = user.currentUser.id;
            }
            catch
            {

            }

            commentsServices.addComment(comment);
            

            return RedirectToAction("index", "Maps");
        }

        [Permissions]
        public ActionResult addTextCommentWithLocation(Comments comment ,int locationId)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            Stream stream;
            byte[] bytes;
            comment.isText = false;
            comment.locationsId = locationId;

            stream = speech.GetSpeakStream(comment.description);
            bytes = ReadFully(stream);
            comment.sound = convertBytesToString(bytes);

            try
            {
                comment.userId = user.currentUser.id;
            }
            catch
            { 

            }

            commentsServices.addComment(comment);


            return RedirectToAction("index", "Maps");
        }

        [Permissions]
        public ActionResult editComment(int idComment)
        {
            Comments comment = new Comments();
            List<string> list = new List<string>();
            list.Add("Normal Mode");
            list.Add("Exploration Mode");
            comment = commentsServices.findCommentByIdComment(idComment);
            ViewData["commentId"] = idComment;
            ViewData["selectList"] = new SelectList(list);

            return View(comment);
        }

        [Permissions]
        public ActionResult editCommentWithId(int commentId , Comments comment, string selectList)
        {
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            Comments newComments = commentsServices.findCommentByIdComment(commentId);
            Stream stream;
            byte[] bytes;
            newComments.isText = false;

            if (selectList == "--Select One--")
            {
                return RedirectToAction("index", "Maps");
            }

            else if (selectList == "Normal Mode")
            {
                comment.typeOfCommentsId = 1;
            }

            else if (selectList == "Exploration Mode")
            {
                comment.typeOfCommentsId = 2;
            }


            stream = speech.GetSpeakStream(comment.description);
            bytes = ReadFully(stream);
            newComments.sound = convertBytesToString(bytes);
            newComments.typeOfCommentsId = comment.typeOfCommentsId;

            commentsServices.updateCommentForId(commentId, comment);
            
            return RedirectToAction("index", "Maps");
        }

        [Permissions]
        public ActionResult deleteComment(int commentId)
        {
            commentsServices.deleteComment(commentId);
            return RedirectToAction("index", "Home");
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }

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
