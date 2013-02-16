using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartAudioCityGuide.Models;

namespace SmartAudioCityGuide.Services
{
    public interface IUserLocationServices :IDisposable
    {
        void addUserLocation(UserLocations userLocation);
        void addUserLocationOrUpdatetime(UserLocations userLocation);
        bool findUserLocationByWindowsPhoneId(string windowsPhoneId);
        UserLocations getUserLocationByWindowsPhoneId(string windowsPhoneId);
        List<UserLocations> findUserByUserLocationAndDistance(UserLocations userLocation, double distance);
        UserLocations findUserLocationByPhoneId(string phoneId);
        void updateLatitudeAndLongitudeByPhoneId(string phoneId, double latitude, double longitude);
        void updateHasById(int idUserLocation, string hash);
        UserLocations findUserLocationByHash(string hash);
    }
}