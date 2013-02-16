using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SmartAudioCityGuide.Models
{
    public class Route
    {
        public Route()
        {
            hint = new List<string>();
        }

        public string description;

        public double lat;

        public double lon;

        public double dist;

        public double time;

        public List<string> hint;
    }
}