using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        [Display(Name = "Park Code")]
        public string ParkCode { get; set; }

        [NotMapped]
        public string ComboParkLatLng { get; set; }

        [NotMapped]
        public string Designation { get; set; }

        [NotMapped]
        public CurrentWeatherInfo CurrentWeatherInfo { get; set; } // Connect extra weather app info to Park model -- Not implemented into database as it's an object

        [NotMapped]
        public List<HikingTrail> Trails { get; set; }

        [NotMapped]
        public HikerTrailRating HikerTrailRating { get; set; } // To Use on Park info from Junction table 

        [NotMapped]
        public List<HikerTrailRating> hikerTrailRatings { get; set; } // Testing to see if a value comes through through a list.

        [NotMapped]
        public ParkMarkers ParkMarkers { get; set;} // From Below

        [NotMapped]
        public HikerParkWishlist HikerParkWishlist { get; set; } // To Use on Park info from Junction table 

        [NotMapped]
        public SelectList StarRatings { get; set; }

        [NotMapped]
        public Hiker Hiker { get; set; } // Used in the Parks Wishlist redirectAction
    }

    public class CurrentWeatherInfo
    {
        public double temperature { get; set; }
        public double wind { get; set; }
        public string condition { get; set; }
    }

    public class ParkMarkers
    {
        //public string ParkUniqueCode { get; set; }
        public string ParkLatitude { get; set; }
        public string ParkLongitude { get; set; }
    }
}   