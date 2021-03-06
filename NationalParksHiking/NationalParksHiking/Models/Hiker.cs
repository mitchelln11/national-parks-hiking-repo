﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalParksHiking.Models
{
    public class Hiker
    {
        [Key]
        public int HikerId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        [ForeignKey("ApplicationUser")]
        [HiddenInput(DisplayValue = false)]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public Park Park { get; set; } // Read Park info on hiker views
        public List<HikerParkWishlist> hikerParkWishlists { get; set; }
    }
}