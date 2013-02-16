using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public class ContactServices : IContactServices,IDisposable
    {
        private SmartAudioCityGuideEntities db;

        public ContactServices()
            : this(new SmartAudioCityGuideEntities())
        {

        }

        public ContactServices(SmartAudioCityGuideEntities context)
        {
            db = context;
        }

        public void addContact(Contacts contact)
        {
            db.contacts.Add(contact);
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}