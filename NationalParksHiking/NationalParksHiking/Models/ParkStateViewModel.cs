using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalParksHiking.Models
{
    public class ParkStateViewModel
    {
        public Park Park { get; set; } // From Main Park Model
        public HikingTrail HikingTrail { get; set; } // From Main Hiking Model


        // From Tutorial on Dropdown Lists with states
        public List<Park> ParkStates { get; set; }

        public IEnumerable<SelectListItem> GetStateAsSelectedItem()
        {
            return new SelectList(ParkStates, "State");
        }
    }
}