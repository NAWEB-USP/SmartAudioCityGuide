using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartAudioCityGuide.Models
{

    public class TypeOfComments
    {
        [Key]
        [Required]
        public int id { get; set; }

        [StringLength(300)]
        public string name { get; set; }

        [StringLength(300)]
        public string description { get; set; }
    }
}