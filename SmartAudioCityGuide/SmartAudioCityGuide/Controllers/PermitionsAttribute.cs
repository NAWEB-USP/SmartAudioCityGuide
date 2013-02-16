using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Controllers
{
    public class PermissionsAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Users user = (Users)filterContext.HttpContext.Session["currentUser"];
            if (user == null || user.authenticate == 0)
            {
                //send them off to the login page
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content("~/");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }

    }
}
