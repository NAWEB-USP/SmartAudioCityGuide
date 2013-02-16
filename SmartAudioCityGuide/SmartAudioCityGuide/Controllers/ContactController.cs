using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartAudioCityGuide.Models;
using SmartAudioCityGuide.Services;

namespace SmartAudioCityGuide.Controllers
{
    public class ContactController : Controller
    {
        private IContactServices contactServices;

        public ContactController()
        {
            contactServices = new ContactServices(new SmartAudioCityGuideEntities());
        }

        public ContactController(IContactServices _contactServices)
        {
            contactServices = _contactServices;
        }


        //
        // GET: /Contact/

        public ActionResult index()
        {
            ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            return View();
        }


        public ActionResult add(Contacts contact)
        {

            bool result = true;
            try
            {
                if (contact.name == null)
                {
                    ModelState.AddModelError("name", "*This field can't be null.");
                    TempData["ModelState"] = ModelState;
                    result = false;
                }

                if (contact.eMail == null)
                {
                    ModelState.AddModelError("eMail", "*This field can't be null.");
                    TempData["ModelState"] = ModelState;
                    result = false;
                }

                if (contact.country == null)
                {
                    ModelState.AddModelError("country", "*This field can't be null.");
                    TempData["ModelState"] = ModelState;
                    result = false;
                }

                if (contact.city == null)
                {
                    ModelState.AddModelError("city", "*This field can't be null.");
                    TempData["ModelState"] = ModelState;
                    result = false;
                }

                if (contact.phone == null)
                {
                    ModelState.AddModelError("phone", "*This field can't be null.");
                    TempData["ModelState"] = ModelState;
                    result = false;
                }

                if (result)
                {
                    ModelState.AddModelError("contact", "Thanks to be in contact with us.");
                    TempData["ModelState"] = ModelState;
                    contactServices.addContact(contact);
                }
            }
            catch (Exception)
            {
            }
            return View("index");
        }
    }
}
