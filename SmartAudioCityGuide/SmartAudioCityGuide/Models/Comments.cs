using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAudioCityGuide.Models
{

    public class Comments
    {
        [Key]
        [Required]
        public int id { get; set; }

        public int userId { get; set; }

        public int locationsId { get; set; }

        public int typeOfCommentsId { get; set; }

        [StringLength(300)]
        public string description { get; set; }

        [StringLength(300)]
        public string archiveDescription { get; set; }

        public bool isText { get; set; }

        public string sound { get; set; }
    }
}