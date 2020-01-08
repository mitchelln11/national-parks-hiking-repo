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
        public List<Park> ParkStates { get; set; }

        public IEnumerable<SelectListItem> GetStateAsSelectedItem()
        {
            return new SelectList(ParkStates, "State");
        }
    }
}