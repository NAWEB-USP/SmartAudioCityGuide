using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAudioCityGuide.Models
{
    public class Locations
    {
        [Key]
        [Required]
        public int id { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }

        [NotMapped]
        public Locations currentLocation
        {
            get
            {
                return (Locations)HttpContext.Current.Session["currentLocation"];
            }
            set
            {
                HttpContext.Current.Session["currentLocation"] = value;
            }
        }

    }
}