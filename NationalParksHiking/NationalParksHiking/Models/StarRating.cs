using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NationalParksHiking.Models
{
    public class StarRating
    {
        [Key]
        public int StarRatingId { get; set; }

        public int IndividualStarRating { get; set; }
    }
}