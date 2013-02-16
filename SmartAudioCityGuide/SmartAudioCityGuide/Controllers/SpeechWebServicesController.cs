using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SmartAudioCityGuide.Services;
using SmartAudioCityGuide.Models;
using TranslatorService.Speech;
using System.Web.Script.Serialization;
using System.Text;

namespace SmartAudioCityGuide.Controllers
{
    public class SpeechWebServicesController : Controller
    {
        private ICommentServices commentServices;
        private JavaScriptSerializer javaSerializer = new JavaScriptSerializer();

        public SpeechWebServicesController()
        {
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
        }

        public SpeechWebServicesController(ICommentServices _commentServices)
        {
            commentServices = _commentServices;
        }

        public string sendLastCommentFromIdLocation(string idLocation, string code)
        {
            List<Comments> comments = new List<Comments>();
            Comments lastComment = new Comments();
            Stream resultStream;
            byte[] result;
            string resultJson;
            List<string> listChar = new List<string>();
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            comments = commentServices.findCommentByIdLocation(Convert.ToInt32(idLocation));
            lastComment = comments.Last();
            resultStream = speech.GetSpeakStream(lastComment.description,"pt");
            result = convertStreamToByteBuffer(resultStream);
            for (int i = 0; i < result.Length; i++)
            {
                listChar.Add(result[i].ToString());
            }
            resultJson = javaSerializer.Serialize(listChar);
            return resultJson;

        }

        public string sendLastSoundCommentFromIdLocation(int idLocation, string code)
        {
            List<Comments> comments = new List<Comments>();
            Comments lastComment = new Comments();
            comments = commentServices.findCommentByIdLocation(idLocation);
            lastComment = comments.LastOrDefault();
            if (lastComment != null)
                return lastComment.sound;
            else
                return null;
        }

        public string sendLastSoundCommentFromIdLocationAndTypeOfComment(int idLocation,int idTypeOfComment, string code)
        {
            List<Comments> comments = new List<Comments>();
            Comments lastComment = new Comments();
            comments = commentServices.findCommentByIdLocation(idLocation);
            comments = (from comen in comments
                        where comen.typeOfCommentsId == idTypeOfComment
                        select comen).ToList();
            lastComment = comments.LastOrDefault();
            if (lastComment != null)
                return lastComment.sound;
            else
                return null;
        }

        public string talkThis(string talk, string code)
        {
            Stream resultStream;
            byte[] result;
            string resultJson;
            List<string> listChar = new List<string>();
            SpeechSynthesizer speech = new SpeechSynthesizer("SmartAudioCityGuide", "Lz+vYpOFm6NTP83A9y0tPoX6ByJa06Q6yxHvoBsD0xo=");
            resultStream = speech.GetSpeakStream(talk);
            result = convertStreamToByteBuffer(resultStream);
            for (int i = 0; i < result.Length; i++)
            {
                listChar.Add(result[i].ToString());
            }
            resultJson = javaSerializer.Serialize(listChar);

            return resultJson;

        }

        public byte[] convertStreamToByteBuffer(System.IO.Stream theStream)
        {
            int b1;
            System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
            while ((b1 = theStream.ReadByte()) != -1)
            {
                tempStream.WriteByte(((byte)b1));
            }
            return tempStream.ToArray();
        }
    }
}
