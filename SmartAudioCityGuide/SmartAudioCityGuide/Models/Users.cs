using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAudioCityGuide.Models
{
    public class Users
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required(ErrorMessage="Name is needed")]
        [Display(Name = "Name")]
        [StringLength(300)]
        public string name { get; set; }

        [Required(ErrorMessage="E-mail is needed")]
        [Display(Name = "E-mail")]
        [StringLength(300)]
        public string userName { get; set; }

        [Required(ErrorMessage="Password is needed")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [Display(Name = "Authentication")]
        public int authenticate { get; set; }

        [Display(Name = "Hash")]
        [StringLength(300)]
        public string hash { get; set; }

        public int typeOfBlindness { get; set; }
        
        [StringLength(300)]
        public string idFacebook { get; set; }

        [StringLength(300)]
        public string acessTokenFacebook { get; set; }

        [StringLength(300)]
        public string phoneId { get; set; }

        [NotMapped]
        public Users currentUser
        {
            get
            {
                return (Users)HttpContext.Current.Session["currentUser"];
            }
            set
            {
                HttpContext.Current.Session["currentUser"] = value;
            }
        }
        
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