using NationalParksHiking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace NationalParksHiking.HelperClass
{
    public class parkHelper
    {
        static List<Park> ParkStates = new List<Park>() {
            new Park() { ParkState = "AL"},
            new Park() { ParkState = "AK"},
            new Park() { ParkState = "AZ"},
            new Park() { ParkState = "AR"},
            new Park() { ParkState = "CA"},
            new Park() { ParkState = "CO"},
            new Park() { ParkState = "CT"},
            new Park() { ParkState = "DC"},
            new Park() { ParkState = "DE"},
            new Park() { ParkState = "FL"},
            new Park() { ParkState = "GA"},
            new Park() { ParkState = "HI"},
            new Park() { ParkState = "ID"},
            new Park() { ParkState = "IL"},
            new Park() { ParkState = "IN"},
            new Park() { ParkState = "IA"},
            new Park() { ParkState = "KS"},
            new Park() { ParkState = "KY"},
            new Park() { ParkState = "LA"},
            new Park() { ParkState = "ME"},
            new Park() { ParkState = "MD"},
            new Park() { ParkState = "MA"},
            new Park() { ParkState = "MI"},
            new Park() { ParkState = "MN"},
            new Park() { ParkState = "MS"},
            new Park() { ParkState = "MO"},
            new Park() { ParkState = "MT"},
            new Park() { ParkState = "NE"},
            new Park() { ParkState = "NV"},
            new Park() { ParkState = "NH"},
            new Park() { ParkState = "NJ"},
            new Park() { ParkState = "NM"},
            new Park() { ParkState = "NY"},
            new Park() { ParkState = "NC"},
            new Park() { ParkState = "ND"},
            new Park() { ParkState = "OH"},
            new Park() { ParkState = "OK"},
            new Park() { ParkState = "OR"},
            new Park() { ParkState = "PA"},
            new Park() { ParkState = "RI"},
            new Park() { ParkState = "SC"},
            new Park() { ParkState = "SD"},
            new Park() { ParkState = "TN"},
            new Park() { ParkState = "TX"},
            new Park() { ParkState = "UT"},
            new Park() { ParkState = "VT"},
            new Park() { ParkState = "VA"},
            new Park() { ParkState = "WA"},
            new Park() { ParkState = "WV"},
            new Park() { ParkState = "WI"},
            new Park() { ParkState="WY"}
        };

        static public List<Park> GetParkStates()
        {
            return ParkStates;
        }
    }
}