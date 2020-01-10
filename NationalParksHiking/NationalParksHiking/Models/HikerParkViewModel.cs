using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NationalParksHiking.Models
{
    public class HikerParkViewModel
    {
        public Hiker Hiker { get; set; } // From main Hiker model
        public Park Park { get; set; } // From main Park model
        public HikerParkWishlist HikerParkWishlist { get; set; } // From Hiker Park Junction Model
    }
}