using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public class LocationServices : ILocationServices, IDisposable
    {
        private SmartAudioCityGuideEntities db;

        public LocationServices()
            : this(new SmartAudioCityGuideEntities())
        {
        }

        public LocationServices(SmartAudioCityGuideEntities context)
        {
            db = context;
        }

        public void addLocations(Locations location)
        {
            db.locations.Add(location);
            db.SaveChanges();
        }

        public List<Locations> findLocationsAround(string lat, string lng, double distance)
        {
            double doubleLat, doubleLon;
            doubleLat = Convert.ToDouble(lat);
            doubleLon = Convert.ToDouble(lng);
            List<Locations> locations = (from t in db.locations
                                         where (t.latitude <= doubleLat + distance && t.latitude >= doubleLat - distance &&
                                            t.longitude <= doubleLon + distance && t.longitude >= doubleLon - distance)
                                         select t).ToList();

            return locations;
        }

        public List<Locations> findAllLocations()
        {
            return db.locations.ToList();
        }

        public Locations findLocationByLatAndLng(string lat, string lng)
        {
            double doubleLat, doubleLon;
            double epsilon = 0.0001;
            doubleLat = Convert.ToDouble(lat);
            doubleLon = Convert.ToDouble(lng);

            List<Locations> locations = (from loc in db.locations
                                         select loc).ToList();

            locations = (from loc in locations
                         where loc.latitude <= doubleLat + epsilon && loc.latitude >= doubleLat - epsilon
                         select loc).ToList();

            Locations location = (from loc in locations
                                  where loc.longitude <= doubleLon + epsilon && loc.longitude >= doubleLon - epsilon
                                  select loc).FirstOrDefault();


            return location;
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