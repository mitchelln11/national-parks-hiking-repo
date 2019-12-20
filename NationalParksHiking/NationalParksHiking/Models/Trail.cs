using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalParksHiking.Models
{
    public class HikingTrail
    {
        [Key]
        public int TrailId { get; set; }

        [Display(Name = "Trail Name")]
        public string TrailName { get; set; }

        [Display(Name = "Difficulty")]
        public string TrailDifficulty { get; set; }

        [ForeignKey("Park")]
        public int ParkId { get; set; }
        public Park Park { get; set; }
    }
}