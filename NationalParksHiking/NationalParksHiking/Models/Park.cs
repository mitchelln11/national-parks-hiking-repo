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
        public CurrentWeatherInfo CurrentWeatherInfo { get; set; } // Connect extra weather app info to Park model -- Not implemented into database as it's an object

        [NotMapped]
        public List<HikingTrail> Trails { get; set; }

        //[NotMapped]
        //public HikingTrail trail { get; set; }

        [NotMapped]
        public HikerTrailRating HikerTrailRating { get; set; } // To Use on Park info from Junction table 

        [NotMapped]
        public List<HikerTrailRating> hikerTrailRatings { get; set; } // Testing to see if a value comes through through a list.

        [NotMapped]
        public ParkMarkers ParkMarkers { get; set;} // From Below

        [NotMapped]
        public HikerParkWishlist HikerParkWishlist { get; set; } // To Use on Park info from Junction table 

        //[NotMapped]
        //[Display(Name = "Filter by State")]
        //public string State { get; set; }

        //[NotMapped]
        //public IEnumerable<SelectListItem> States { get; set; }

        [NotMapped]
        public Hiker Hiker { get; set; } // Used in the Parks Wishlist redirectAction


        public IEnumerable<SelectListItem> GetStateAsSelectedItem()
        {
            return new SelectList(ParkStates, "State");
        }

        static List<Park> ParkStates = new List<Park>() {
            new Park() { ParkState = "AL"},
            new Park() { ParkState = "AK"},
            new Park() { ParkState = "AZ"},
            new Park() { ParkState = "AR"},
            new Park() { ParkState = "CA"},
            new Park() { ParkState = "CO"},
            new Park() { ParkState = "CT"},
            new Park() { ParkState = "DC"},
            new Park() { ParkState = "DE"},
            new Park() { ParkState = "FL"},
            new Park() { ParkState = "GA"},
            new Park() { ParkState = "HI"},
            new Park() { ParkState = "ID"},
            new Park() { ParkState = "IL"},
            new Park() { ParkState = "IN"},
            new Park() { ParkState = "IA"},
            new Park() { ParkState = "KS"},
            new Park() { ParkState = "KY"},
            new Park() { ParkState = "LA"},
            new Park() { ParkState = "ME"},
            new Park() { ParkState = "MD"},
            new Park() { ParkState = "MA"},
            new Park() { ParkState = "MI"},
            new Park() { ParkState = "MN"},
            new Park() { ParkState = "MS"},
            new Park() { ParkState = "MO"},
            new Park() { ParkState = "MT"},
            new Park() { ParkState = "NE"},
            new Park() { ParkState = "NV"},
            new Park() { ParkState = "NH"},
            new Park() { ParkState = "NJ"},
            new Park() { ParkState = "NM"},
            new Park() { ParkState = "NY"},
            new Park() { ParkState = "NC"},
            new Park() { ParkState = "ND"},
            new Park() { ParkState = "OH"},
            new Park() { ParkState = "OK"},
            new Park() { ParkState = "OR"},
            new Park() { ParkState = "PA"},
            new Park() { ParkState = "RI"},
            new Park() { ParkState = "SC"},
            new Park() { ParkState = "SD"},
            new Park() { ParkState = "TN"},
            new Park() { ParkState = "TX"},
            new Park() { ParkState = "UT"},
            new Park() { ParkState = "VT"},
            new Park() { ParkState = "VA"},
            new Park() { ParkState = "WA"},
            new Park() { ParkState = "WV"},
            new Park() { ParkState = "WI"},
            new Park() { ParkState="WY"}
        };

        static public List<Park> GetParkStates()
        {
            return ParkStates;
        }
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