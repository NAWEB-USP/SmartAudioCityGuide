using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAudioCityGuide.Models
{
    public class Contacts
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Display(Name = "Name")]
        [StringLength(300)]
        public string name { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(300)]
        public string eMail {get;set;}

        [Display(Name = "County")]
        [StringLength(300)]
        public string country { get; set; }

        [Display(Name = "City")]
        [StringLength(300)]
        public string city { get; set; }

        [Display(Name = "Phone")]
        [StringLength(300)]
        public string phone { get; set; }
    }
}