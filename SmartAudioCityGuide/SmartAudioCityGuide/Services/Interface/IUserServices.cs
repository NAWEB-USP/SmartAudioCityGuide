using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public interface IUserServices : IDisposable
    {
        void addUser(Users user);
        void athenticateUserWithHash(string hash);
        bool hasAUserWithThisEmail(string email);
        Users findUserByUserName(string userName);
        Users findUserByPhoneId(string phoneId);
        void updateUser(int idUser, Users newUser);
    }
}