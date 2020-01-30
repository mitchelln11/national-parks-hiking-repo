﻿using System;
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

        public SelectList States { get; set; }


        public List<StatesList> StateList { get; set; }
    }
}