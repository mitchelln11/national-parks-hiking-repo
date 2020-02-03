using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalParksHiking.Models
{
    public class ParkTrailRatingViewModel
    {
        public List<StarRating> StarRatings { get; set; }
        public int SelectedRating { get; set; }
        public SelectList StarRating { get; set; }

        //====  Connecting for View Purposes ===
        public Park Park { get; set; }
    }
}