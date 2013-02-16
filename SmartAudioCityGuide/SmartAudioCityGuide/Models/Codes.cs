using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAudioCityGuide.Models
{
    public class Codes
    {
        [Key]
        [Required]
        public int id { get; set; }

        [StringLength(300)]
        public string code { get; set; }
    }
}