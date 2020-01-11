using System;
using System.Collections;
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

        [Display(Name = "Summary")]
        public string TrailSummary { get; set; }

        [Display(Name = "Length")]
        public double TrailLength { get; set; }

        [Display(Name = "Trail Condition")]
        public string TrailCondition { get; set; }

        public string HikingApiCode { get; set; }

        [ForeignKey("Park")]
        public int ParkId { get; set; }
        public Park Park { get; set; }

        [NotMapped]
        public HikerTrailRating HikerTrailRating { get; set; } // To use on models

        //public IEnumerator GetEnumerator()
        //{
        //    for(var i=1;i<7;i++)
        //    {
        //        switch (i)
        //        {
        //            case  1:
        //                yield return TrailName;
        //                break;
        //            case 2:
        //                yield return TrailDifficulty;
        //                break;
        //            case 3:
        //                yield return TrailSummary;
        //                break;
        //            case 4:
        //                yield return TrailLength;
        //                break;
        //            case 5:
        //                yield return TrailCondition;
        //                break;
        //            default:
        //                Console.WriteLine("Could not get value");
        //                break;
        //        }
        //    }
        //}
    }
}