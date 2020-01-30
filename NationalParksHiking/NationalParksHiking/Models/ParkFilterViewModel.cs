using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalParksHiking.Models
{
    public class ParkFilterViewModel
    {
        public List<Park> Parks { get; set; }

        [Display(Name="Select State")]
        public string SelectedState { get; set; }

        public SelectList States { get; set; } // SelectList is unique to make a dropdown, not the name of the object


        public SelectList StateList { get; set; }
    }
}