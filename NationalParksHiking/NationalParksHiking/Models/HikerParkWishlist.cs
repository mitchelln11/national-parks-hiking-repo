﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NationalParksHiking.Models
{
    public class HikerParkWishlist  // Junction table
    {
        [Key]
        public int HikerParkWishlistId { get; set; }

        [ForeignKey("Hiker")]
        public int HikerId { get; set; }
        public Hiker Hiker { get; set; }

        [ForeignKey("Park")]
        public int ParkId { get; set; }
        public string ParkName { get; set; }
        public Park Park { get; set; }
    }
}