using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAudioCityGuide.Models
{
    public class UserLocations
    {
        [Key]
        [Required]
        public int id { get; set; }

        public int userId { get; set; }

        [StringLength(300)]
        public string windowsPhoneId { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public DateTime requestTime { get; set; }

        [StringLength(300)]
        public string hash { get; set; }
    }
}