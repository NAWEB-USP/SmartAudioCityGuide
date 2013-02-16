using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Services;
using SmartAudioCityGuide.Models;
using System.Web.Script.Serialization;

namespace SmartAudioCityGuide.Controllers
{
    public class CommentsWebServicesController : Controller
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        private ICommentServices commentServices;
        private ILocationServices locationServices;
        private ICodeServices codeServices;

        public CommentsWebServicesController()
        {
            commentServices = new CommentServices(new SmartAudioCityGuideEntities());
            locationServices = new LocationServices(new SmartAudioCityGuideEntities());
            codeServices = new CodeServices(new SmartAudioCityGuideEntities());
        }

        public CommentsWebServicesController(ICodeServices _codeServices, ILocationServices _locationServices, ICommentServices _commentServices)
        {
            codeServices = _codeServices;
            locationServices = _locationServices;
            commentServices = _commentServices;
        }


        public string addCommentoToLocation(string location, string comment, string code)
        {
            string firstCode = codeServices.findFirstCode();
            Locations locationPassed = new Locations();
            Locations locationSearch = new Locations();
            Comments commentPassed = new Comments();
            
            if (code == firstCode)
            {
                locationPassed = serializer.Deserialize<Locations>(location);
                commentPassed = serializer.Deserialize<Comments>(comment);
                locationSearch = locationServices.findLocationByLatAndLng(locationPassed.latitude.ToString(), locationPassed.longitude.ToString());
                if (locationPassed == null || locationPassed.id == 0)
                {
                    locationServices.addLocations(locationPassed);
                    commentPassed.locationsId = locationPassed.id;
                    commentPassed.isText = true;
                }
                else
                {
                    commentPassed.locationsId = locationSearch.id;
                    commentPassed.isText = true;
                }
                commentServices.addComment(commentPassed);
                return "1";

            }
            return "0";

        }
        public string addCommentAndSoundToLocation(string location, string comment, string code)
        {
            string firstCode = codeServices.findFirstCode();
            Locations locationPassed = new Locations();
            Locations locationSearch = new Locations();
            Comments commentPassed = new Comments();

            if (code == firstCode)
            {
                locationPassed = serializer.Deserialize<Locations>(location);
                commentPassed = serializer.Deserialize<Comments>(comment);
                locationSearch = locationServices.findLocationByLatAndLng(locationPassed.latitude.ToString(), locationPassed.longitude.ToString());
                if (locationPassed == null || locationPassed.id == 0)
                {
                    locationServices.addLocations(locationPassed);
                    commentPassed.locationsId = locationPassed.id;
                    commentPassed.isText = false;
                }
                else
                {
                    commentPassed.locationsId = locationSearch.id;
                    commentPassed.isText = false;
                }
                commentServices.addComment(commentPassed);
                return "1";

            }
            return "0";

        }

        public string findAllCommentsArround(string location, string radius, string code)
        {
            string firstCode = codeServices.findFirstCode();
            Locations locationPassed = new Locations();
            Locations location1 = new Locations();
            List<Comments> commentsResult = new List<Comments>();
            string commentsResultJson;
            if (code == firstCode)
            {
                locationPassed = serializer.Deserialize<Locations>(location);

                location1 = locationServices.findLocationByLatAndLng(locationPassed.latitude.ToString(), locationPassed.longitude.ToString());
                commentsResult = commentServices.findCommentByIdLocation(location1.id);
                
                commentsResultJson = serializer.Serialize(commentsResult);
                return commentsResultJson;
            }
            return null;
        }

    }
}