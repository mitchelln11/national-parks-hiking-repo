﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NationalParksHiking.Models;
using System.Net.Http;
using Newtonsoft.Json;
using NationalParksHiking;
using Microsoft.AspNet.Identity;

namespace NationalParksHiking.Controllers
{
    public class ParksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //https://nimblegecko.com/using-simple-drop-down-lists-in-ASP-NET-MVC/
        //public ActionResult GetStateFromDropdown()
        //{
        //    // Get Sign Up information from the session
        //    var states = GetAllStates();
        //    var model = new Park();
        //    model.States = GetSelectListItems(states);
        //    // Display Done.html page that shows Name and selected state.
        //    return View(model);
        //}

        
        //[HttpPost]
        //public ActionResult GetStateFromDropdown(Park park)
        //{
        //    // Get all states for DropDownList
        //    var states = GetAllStates();
        //    var model = new Park();
        //    // Create a list of SelectListItems so these can be rendered on the page
        //    model.States = GetSelectListItems(states);
        //    if (ModelState.IsValid)
        //    {
        //        Session["Park"] = park;
        //        return RedirectToAction("Index", "Park");
        //    }
        //    return View("Index", "Park");
        //}

        //public ActionResult FoundState()
        //{
        //    var model = Session["Park"] as Park;
        //    return View("Index", "Park", model);
        //}


        // GET: Parks
        public async Task<ActionResult> Index(ApiKeys apiKeys)
        {
            await GetApiKey(); // Needed to use API key

            //var states = GetAllStates();
            //var model = new Park();
            //model.States = GetSelectListItems(states);
            //Park park = new Park();
            //await RunParkJson(apiKeys, park);
            var parks = await db.Parks.ToListAsync();
            return View(parks);
        }


        // GET: Parks/Details/5
        public async Task<ActionResult> Details(int? id, ApiKeys apiKeys)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Park park = await db.Parks.FindAsync(id);
            park.CurrentWeatherInfo = new CurrentWeatherInfo(); // Instantiate blank spot for data to bind to
            park.CurrentWeatherInfo.temperature = 876; // Doesn't matter what's here, will overwrite anyway
            park.Trails = db.HikingTrails.Where(i => i.ParkId == id).ToList();
            await RunParkJson(apiKeys, park);
            await RunWeatherJson(apiKeys, park);
            await RunHikingJson(apiKeys, park);
            await GetApiKey();
            await GetParkMarker(id);
            if (park == null)
            {
                return HttpNotFound();
            }
            return View(park);
        }

        // GET: Parks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ParkId,ParkName,StreetAddress,ParkCity,ParkState,ParkLat,ParkLng")] Park park)
        {
            if (ModelState.IsValid)
            {
                db.Parks.Add(park);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Parks");
            }

            return View(park);
        }

        // GET: Parks/Edit/5
        public async Task<ActionResult> Edit(int? id, ApiKeys apiKeys)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Park park = await db.Parks.FindAsync(id);
            //await RunJsonClient(park, apiKeys);
            if (park == null)
            {
                return HttpNotFound();
            }
            return View(park);
        }

        // POST: Parks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ParkId,ParkName,StreetAddress,ParkCity,ParkState,ParkLat,ParkLng")] Park park)
        {
            if (ModelState.IsValid)
            {
                db.Entry(park).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(park);
        }

        // GET: Parks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Park park = await db.Parks.FindAsync(id);
            if (park == null)
            {
                return HttpNotFound();
            }
            return View(park);
        }

        // POST: Parks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Park park = await db.Parks.FindAsync(id);
            db.Parks.Remove(park);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //  -------///////------START PARK RELATED METHODS-----------\\\\\\\\\\\\\\\\\\\---------------

        // ------------------ Get Lat Long -----------------------------
        public async Task GetLatLong(Park park, Datum parkInfo)
        {
            var comboLatLong = parkInfo.latLong;
            var latLongArray = comboLatLong.Split().ToArray(); // Splits based on comma, set to an array
            string isolatedLatitude = latLongArray[0].TrimEnd(','); // New lat variable for the 0 index, trim trailing comma
            string isolatedLongtitude = latLongArray[1].TrimEnd(','); // New lng variable for the 1 index trim trailing comma
            string latitude = isolatedLatitude.Substring(4, isolatedLatitude.Length - 4); // Remove beginning lat: text
            string longitude = isolatedLongtitude.Substring(5, isolatedLongtitude.Length - 5); // Remove beginning lng: text
            park.ParkLat = latitude;
            park.ParkLng = longitude;
            await db.SaveChangesAsync();
        }

        // ------------------ Get Park Description --------------------
        public async Task GetParkDescription(Park park, Datum parkInfo)
        {
            string parkDescription = parkInfo.description;
            park.ParkDescription = parkDescription;
            await db.SaveChangesAsync();
        }

        // ------------------ Get Park Name -----------------------------
        public async Task GetFullParkName(Park park, Datum parkInfo)
        {
            string fullParkName = parkInfo.fullName;
            park.ParkName = fullParkName;
            await db.SaveChangesAsync();
        }

        // ------------------ Get State ----------------------------------
        public async Task GetParkState(Park park, Datum parkInfo)
        {
            string ParkState = parkInfo.states;
            park.ParkState = ParkState;
            await db.SaveChangesAsync();
        }

        // ------------------ Run single httpclient and response call for Parks -----------------------------
        public async Task RunParkJson(ApiKeys apiKeys, Park park)
        {
            string parkKey = apiKeys.NpsKey;
            int apiLimit = 150;
            string url = $"https://developer.nps.gov/api/v1/parks?q=National%20Park&limit={apiLimit}&api_key={parkKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                NpsJsonInfo npsJsonInfo = JsonConvert.DeserializeObject<NpsJsonInfo>(jsonresult); // Run through entire JSON file
                Datum parkInfo = npsJsonInfo.data.Where(p => p.parkCode == park.ParkCode).Single(); // Look into individual park when passing id in URL
                // From here on out, refer to parkInfo instead of npsJsonInfo
                await GetParkDescription(park, parkInfo);
                await GetFullParkName(park, parkInfo);
                await GetParkState(park, parkInfo);
                await GetLatLong(park, parkInfo);
                await db.SaveChangesAsync();
            }
        }



        //  -------///////------START TRAIL RELATED METHODS-----------\\\\\\\\\\\\\\\\\\\---------------
        // ------------------ Get Trail Ratings --------------------

        public async Task GetAverageTrailRating()
        {
            var user = User.Identity.GetUserId();
            //HikingTrail trailInPark = new HikingTrail();
            //List<HikingTrail> hikingTrails = db.HikingTrails.Any(p => p.TrailId == trailInfo[0].);
            //var parkId = db.HikingTrails.Where(t => t.ParkId == Park.ParkId);
            //var trailList = db.HikerTrailRatings.ToList(); // returns all the ratings

            // Where TrailId Matches
            // Get Count of reviews
            // Get Sum of reviews
            List<HikerTrailRating> AllRatings = db.HikerTrailRatings.ToList();
            //var ReviewAverage = AllRatings[0].AverageUserRating;
            var ReviewCount = AllRatings.Count;

            List<int> RatingInts = new List<int>();
            foreach(HikerTrailRating rating in AllRatings)
            {
                int individualRating = rating.IndividualRating;
                RatingInts.Add(individualRating);
            }
            var RatingsSum = RatingInts.Sum();
            var TotalAverageRating = RatingsSum/ReviewCount;
            for (var j=0;j<AllRatings.Count;j++)
            {
                AllRatings[j].AverageUserRating = TotalAverageRating;
            }
            
            //foreach (var Rating in AllRatings)
            //{
            //    //List<HikerTrailRating> hikerTrailRating;
            //    //TrailCollection trailCollections = new TrailCollection(); 
            //    var IndivRating = Rating.IndividualRating;
            //    //Rating.IndividualRating = hikerTrailRating.Add();
            //}
            //var SingleRating = AllRatings.
            //List<HikingTrail> trailList = db.HikingTrails.ToList();
            //for (var i=0;i<trailList.Count;i++)
            //{
            //    //HikerTrailRating hikerTrailRating = new HikerTrailRating(); // Instantiating new Rating
            //    var test = db.HikingTrails.Where(d => d.TrailId == trailList[i].TrailId);
            //    var Rating = hikerTrailRating.IndividualRating;
            //    var TrailIndex = trailList[i].TrailId;
            //    //var Rating = trailList[i].
            //    db.HikerTrailRatings.Add(hikerTrailRating);
            //}

            //var 

            //var trailList = db.HikerTrailRatings.Where(t => t.TrailId == Convert.ToInt32(trailInfo.);
            //List<HikerTrailRating> hikingTrailRatings = db.HikerTrailRatings.Where(h => h.TrailId == trailInPark.TrailId).ToList();
            //var AmountOfReviews = trailInfo.Count();
            //var Rating = db.HikerTrailRatings.Where()
            //HikingTrail hikingTrail = new HikingTrail();
            //HikerTrailRating hikerTrailRating = new HikerTrailRating();
            //HikerTrailRating hikerTrailRating = new HikerTrailRating();
            // Pull Individual Ratings
            // Pull Trail being Rated
            //HikingTrail hikingTrail = new HikingTrail()
            //HikerTrailRating hikerTrailRating = db.HikerTrailRatings.Where(t => t.TrailId == hikingTrail.TrailId);
            //var rating = db.HikingTrails.Where(h => h.TrailId == hikingTrail.TrailId);
            //HikingTrail hikingTrail = db.HikingTrails.Where(t => t.TrailId == hikerTrailRating.TrailId);

            //var test = hikerTrailRating.Average();
            //var IndividualRating = hikerTrailRating.IndividualRating
            //var AverageTrailRating = IndividualRating.
            //var TotalNumberOfReviews = hikingTrail.TotalUserReviews;
            //var IndividualRating = ;
            //var AverageTrailReview = 20/TotalNumberOfReviews; 
            await db.SaveChangesAsync();
        }

        //public async Task AddRating()
        //{
        //    var user = User.Identity.GetUserId();
        //    List<HikingTrail> trailList = db.HikingTrails.ToList();
        //    for (var i = 0; i < trailList.Count; i++)
        //    {
        //        HikerTrailRating hikerTrailRating = new HikerTrailRating(); // Instantiating new Rating
        //        hikerTrailRating.HikerId = Convert.ToInt32(user);
        //        hikerTrailRating.IndividualRating = db.HikerTrailRatings.Where(r => r.TrailId == trailList[i].TrailId).;
        //        //var test = db.HikingTrails.Where(d => d.TrailId == trailList[i].TrailId);
        //        //var Rating = hikerTrailRating.IndividualRating;
        //        //var TrailIndex = trailList[i].TrailId;
        //        //var Rating = trailList[i].
        //        db.HikerTrailRatings.Add(hikerTrailRating);
        //    }
        //    await db.SaveChangesAsync();
        //}

        // ------------------ Get Trail Details --------------------
        public async Task GetTrailDetails(Park Park, List<Trail> trailInfo)
        {
            var foreignParkId = Park.ParkId;
            HikingTrail hiking = db.HikingTrails.Where(t => t.ParkId == foreignParkId).FirstOrDefault(); // Matching park id with foreign id
            for (var i=0;i<trailInfo.Count;i++)
            {
                HikingTrail hikingTrail = new HikingTrail(); // I want to add a new record
                hikingTrail.HikingApiCode = trailInfo[i].id.ToString();
                hikingTrail.ParkId = foreignParkId;
                hikingTrail.TrailName = trailInfo[i].name.ToString();
                hikingTrail.TrailDifficulty = trailInfo[i].difficulty.ToString();

                string trailSummary = trailInfo[i].summary;
                if (trailSummary == null )
                {
                    hikingTrail.TrailSummary = "No information available at this time.";
                }
                else
                {
                    hikingTrail.TrailSummary = trailSummary;
                }

                // Convert long decimal to a 2 decimal number
                double TrailLength = trailInfo[i].length;
                double SimpletrailLength = Math.Round(TrailLength, 2);
                hikingTrail.TrailLength = SimpletrailLength;

                // Trail Conditions
                string trailCondition = trailInfo[i].conditionDetails;
                if (trailCondition != null)
                {
                    hikingTrail.TrailCondition = trailCondition;
                }
                else
                {
                    hikingTrail.TrailCondition = "No condition status available at this time";
                }

                //await GetAverageTrailRating();

                // Check to see if it already exists
                var trailCode = db.HikingTrails.Where(c => c.HikingApiCode == hikingTrail.HikingApiCode).FirstOrDefault();
                if (trailCode == null)
                {
                    db.HikingTrails.Add(hikingTrail);
                }
            }
            await db.SaveChangesAsync();
        }

        // ------------------ Get Hiking Project JSON -----------------------------
        public async Task RunHikingJson(ApiKeys apiKeys, Park park)
        {
            string hikingKey = apiKeys.HikingProjectKey;
            string parkLatitude = park.ParkLat;
            string parkLongitude = park.ParkLng;
            int distanceFromCenter = 40;
            string url = $"https://www.hikingproject.com/data/get-trails?lat={parkLatitude}&lon={parkLongitude}&maxDistance={distanceFromCenter}&key={hikingKey}"; // Lat, long, and API Key neede for API call
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                HikingTrailJsonInfo hikingTrailJsonInfo = JsonConvert.DeserializeObject<HikingTrailJsonInfo>(jsonresult);
                //HikingTrail hikingTrail = new HikingTrail(); // Empty value needs to be here to pass parameters to methods.
                List<Trail> trailInfo = hikingTrailJsonInfo.trails.ToList();
                //await AddRating();
                await GetTrailDetails(park, trailInfo);
                await GetAverageTrailRating();
                await db.SaveChangesAsync();
            }
        }



        //  -------///////------START FULL US MAP WITH MARKER RELATED METHODS-----------\\\\\\\\\\\\\\\\\\\---------------

        //For Details page
        public async Task GetParkMarker(int? id)
        {
            Park park = await db.Parks.FindAsync(id);
            park = db.Parks.Where(p => p.ParkId == id).Single();
            ViewBag.ParkVarName = park.ParkCode;
            ViewBag.ParkLat = park.ParkLat;
            ViewBag.ParkLng = park.ParkLng;
            await db.SaveChangesAsync();
        }

        //For Index page
        public async Task GetApiKey()
        {
           ViewBag.APIKey = "https://maps.googleapis.com/maps/api/js?key=" + ApiKeys.GoogleMapsJsKey + "&callback=initMap";
            await db.SaveChangesAsync();
        }


        //  -------///////------START WEATHER RELATED METHODS-----------\\\\\\\\\\\\\\\\\\\---------------
        // ------------------ Get Temperature ----------------------------------
        public async Task GetCurrentTemperature(Park park, WeatherJsonInfo weatherJsonInfo)
        {
            float tempInKelvin = weatherJsonInfo.main.temp;
            double convertKelvinToFahrenheit = ((tempInKelvin - 273.15) * 9 / 5) + 32;
            int simpleDegree = Convert.ToInt32(convertKelvinToFahrenheit);
            park.CurrentWeatherInfo.temperature = simpleDegree; // Object reference not set to an instance of an object.
            // Line 260: park.CurrentWeatherInfo.temperature = simpleDegree;
            await db.SaveChangesAsync();
        }

        // ------------------ Get Wind Condition ----------------------------------
        public async Task GetWindCondition(Park park, WeatherJsonInfo weatherJsonInfo)
        {
            float windCondition = weatherJsonInfo.wind.speed;
            double simpleWindMeasure = Math.Round(windCondition, 2);
            park.CurrentWeatherInfo.wind = simpleWindMeasure;
            await db.SaveChangesAsync();
        }

        // ------------------ Get Weather Description ----------------------------------
        public async Task GetWeatherCondition(Park park, WeatherJsonInfo weatherJsonInfo)
        {
            string weatherCondition = weatherJsonInfo.weather[0].main;
            park.CurrentWeatherInfo.condition = weatherCondition;
            await db.SaveChangesAsync();
        }

        // ------------------ Get Weather JSON -----------------------------
        public async Task RunWeatherJson(ApiKeys apiKeys, Park park)
        {
            string weatherKey = apiKeys.OpenWeatherKey;
            string parkLatitude = park.ParkLat;
            string parkLongitude = park.ParkLng;
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={parkLatitude}&lon={parkLongitude}&APPID={weatherKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                WeatherJsonInfo weatherJsonInfo = JsonConvert.DeserializeObject<WeatherJsonInfo>(jsonresult);
                await GetCurrentTemperature(park, weatherJsonInfo);
                await GetWindCondition(park, weatherJsonInfo);
                await GetWeatherCondition(park, weatherJsonInfo);
                await db.SaveChangesAsync();
            }
        }
    }
}
