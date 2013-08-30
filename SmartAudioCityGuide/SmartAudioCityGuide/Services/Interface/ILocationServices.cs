using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public interface ILocationServices :IDisposable
    {
        void addLocations(Locations location);
        List<Locations> findLocationsAround(string lat, string lng, double kilometers);
        List<Locations> findAllLocations();
        Locations findLocationByLatAndLng(string lat, string lng);
    }
}