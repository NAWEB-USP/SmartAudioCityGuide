using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public interface IContactServices :IDisposable
    {
        void addContact(Contacts contact);
    }
}