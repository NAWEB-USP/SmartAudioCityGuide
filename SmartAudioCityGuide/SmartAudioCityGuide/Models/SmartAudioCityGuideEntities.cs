using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SmartAudioCityGuide.Models
{
    public class SmartAudioCityGuideEntities : DbContext
    {
        public DbSet<Users> users { get; set; }

        public DbSet<Locations> locations { get; set; }

        public DbSet<Comments> comments { get; set; }

        public DbSet<Codes> codes { get; set; }

        public DbSet<UserLocations> userLocation { get; set; }

        public DbSet<Contacts> contacts { get; set; }

        public DbSet<TypeOfComments> typeOfComments { get; set; }

    }
}