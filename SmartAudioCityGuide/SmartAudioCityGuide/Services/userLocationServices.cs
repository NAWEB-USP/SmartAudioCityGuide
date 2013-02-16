using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public class UserLocationServices : IDisposable, IUserLocationServices
    {
        private SmartAudioCityGuideEntities db = new SmartAudioCityGuideEntities();

        public UserLocationServices()
            : this(new SmartAudioCityGuideEntities())
        {
        }

        public UserLocationServices(SmartAudioCityGuideEntities context)
        {
            db = context;
        }

        public void addUserLocation(UserLocations userLocation)
        {
            db.userLocation.Add(userLocation);
            db.SaveChanges();
        }

        public void addUserLocationOrUpdatetime(UserLocations userLocation)
        {
            bool foundUserByWindowsPhone = false;
            foundUserByWindowsPhone = findUserLocationByWindowsPhoneId(userLocation.windowsPhoneId);
            if (foundUserByWindowsPhone == true)
            {
                userLocation = getUserLocationByWindowsPhoneId(userLocation.windowsPhoneId);
                userLocation.requestTime = DateTime.Now;
                db.SaveChanges();
            }
            else
            {
                addUserLocation(userLocation);
            }
        }

        public bool findUserLocationByWindowsPhoneId(string windowsPhoneId)
        {
            List<UserLocations> userLocation = new List<UserLocations>();
            userLocation =  (from user in db.userLocation
                            where user.windowsPhoneId  == windowsPhoneId
                            select user).ToList();

            if (userLocation.Count > 0 )
                return true;
            return false;
        }

        public UserLocations getUserLocationByWindowsPhoneId(string windowsPhoneId)
        {
            UserLocations userLocation = (from user in db.userLocation
                                         where user.windowsPhoneId == windowsPhoneId
                                         select user).FirstOrDefault();

            return userLocation;
        }

        public List<UserLocations> findUserByUserLocationAndDistance(UserLocations userLocation, double distance)
        {
            List<UserLocations> usersLocation = (from t in db.userLocation
                                                 where (t.latitude <= userLocation.latitude + distance && t.latitude >= userLocation.latitude - distance &&
                                                 t.longitude <= userLocation.longitude + distance && t.longitude >= userLocation.longitude - distance)
                                                 select t).ToList();

            return usersLocation;
        }

        public UserLocations findUserLocationByPhoneId(string phoneId)
        {
            UserLocations userLocation = (from t in db.userLocation
                                          where t.windowsPhoneId == phoneId
                                          select t).FirstOrDefault();

            return userLocation;

        }

        public UserLocations findUserLocationByHash(string hash)
        {
            UserLocations userLocation = (from t in db.userLocation
                                          where t.hash == hash
                                          select t).FirstOrDefault();

            return userLocation;

        }

        public void updateLatitudeAndLongitudeByPhoneId(string phoneId, double latitude, double longitude)
        {
            UserLocations userLocation = findUserLocationByPhoneId(phoneId);

            if (userLocation != null)
            {
                userLocation.latitude = latitude;
                userLocation.longitude = longitude;
                try
                {
                    db.SaveChanges();
                }
                catch
                {

                }
            }

        }


        public void updateHasById(int idUserLocation, string hash)
        {
            UserLocations userLocation = (from t in db.userLocation
                                          where t.id == idUserLocation
                                          select t).FirstOrDefault();
            try
            {
                userLocation.hash = hash;
                db.SaveChanges();
            }
            catch
            {

            }
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