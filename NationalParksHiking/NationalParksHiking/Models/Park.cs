using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NationalParksHiking.Models
{
    public class Park
    {
        [Key]
        public int ParkId { get; set; }

        [Display(Name = "Park Name")]
        public string ParkName { get; set; }

        [Display(Name = "State")]
        public string ParkState { get; set; }

        [Display(Name = "Latitude")]
        public string ParkLat { get; set; }

        [Display(Name = "Longitude")]
        public string ParkLng { get; set; }

        [Display(Name = "Description")]
        public string ParkDescription { get; set; }

        [NotMapped]
        public CurrentWeatherInfo CurrentWeatherInfo { get; set; } // Connect extra weather app info to Park model -- Not implemented into database as it's an object

       [NotMapped]
       public List<HikingTrail> Trails { get; set; }
    }

    public class CurrentWeatherInfo
    {
        public double temperature { get; set; }
        public double wind { get; set; }
        public string condition { get; set; }
    }
}