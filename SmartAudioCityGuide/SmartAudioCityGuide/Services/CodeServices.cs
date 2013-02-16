using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public class CodeServices :IDisposable, ICodeServices
    {
        private SmartAudioCityGuideEntities db;

        public CodeServices()
            : this(new SmartAudioCityGuideEntities())
        { 
        }

        public CodeServices(SmartAudioCityGuideEntities context)
        {
            db = context;
        }
        
        public string findFirstCode()
        {
            string result;

            result = db.codes.First().code;

            return result;
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