using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public class UserServices :IDisposable,IUserServices
    {
        private SmartAudioCityGuideEntities db;

        public UserServices()
            : this(new SmartAudioCityGuideEntities())
        { 
        }

        public UserServices(SmartAudioCityGuideEntities context)
        {
            db = context;
        }

        public void addUser(Users user)
        {
            db.users.Add(user);
            db.SaveChanges();
        }

        public void athenticateUserWithHash(string hash)
        {
            Users user = db.users.FirstOrDefault(m => m.hash == hash);
            user.authenticate = 1;
            db.SaveChanges();
        }

        public bool hasAUserWithThisEmail(string email)
        {
            var user = db.users.FirstOrDefault(m => m.userName == email);

            if (user == null)
                return false;

            return true;
        }

        public Users findUserByUserName(string userName)
        {
            Users user = db.users.FirstOrDefault(m => m.userName == userName);
            return user;
        }

        public Users findUserByPhoneId(string phoneId)
        {
            Users user = db.users.FirstOrDefault(m => m.phoneId == phoneId);
            return user;
        }

        public void updateUser(int idUser, Users newUser)
        {
            Users user = db.users.FirstOrDefault(m => m.id == idUser);

            user = newUser;
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