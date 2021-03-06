﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NationalParksHiking.Models
{
    public class HikerTrailRating
    {
        [Key]
        public int HikerTrailRatingId { get; set; }

        [ForeignKey("Hiker")]
        public int HikerId { get; set; }
        public Hiker Hiker { get; set; }

        [ForeignKey("HikingTrail")]
        public int TrailId { get; set; }
        public HikingTrail HikingTrail { get; set; }

        [Display(Name = "Individual Review")]
        public int IndividualRating { get; set; }
    }
}