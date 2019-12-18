using System;
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

        [ForeignKey("Trail")]
        public int TrailId { get; set; }
        public Trail Trail { get; set; }

        [Display(Name = "Amount of Ratings")]
        public int RatingAmt { get; set; }
    }
}