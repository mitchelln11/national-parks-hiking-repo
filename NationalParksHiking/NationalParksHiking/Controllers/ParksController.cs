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

        // Attempt to separate multiple state parks
        //public async ActionResult AddParkToWishList()
        //{
        //    ParkFilterViewModel parkFilter = new ParkFilterViewModel();
        //    List<TempStateList> tempStateList = new List<TempStateList>();
        //    parkFilter.Parks = await db.Parks.ToListAsync();
        //    var test = parkFilter.Parks.Select(p => p.ParkState).ToList();
        //    return View(test);
        //}


        // GET: Parks
        public async Task<ActionResult> Index()
        {
            ParkFilterViewModel parkFilter = new ParkFilterViewModel();
            await GetApiKey(); // Needed to use API key, to populate the US map
            await RunBasicParkJson();
            parkFilter.Parks = await db.Parks.ToListAsync();

            var fullList = parkFilter.Parks.ToList(); // All parks to reference against
            
            // Find Parks in multiple states based on comma
            List<string> tempStateList = new List<string>(); // Temporary holding list for multiple state parks
            foreach (var singleState in fullList)
            {
                if (singleState.ParkState.Contains(","))
                {
                    tempStateList.Add(singleState.ParkState);
                }
            }

            // Use list of multi-state parks from above ^^, and add each index
            List<string> multiStateCollection = new List<string>();
            for (var i = 0; i < tempStateList.Count; i++)
            {
                string[] stringArray = tempStateList[i].Split(','); // must use ',' (single quote) instead of "," because Split paremeters are always char
                foreach(var stateArray in stringArray)
                {
                    multiStateCollection.Add(stateArray);
                }
            }

            // Final List
            List<string> finalStateList = new List<string>();
            List<string> currentStates = db.Parks.Select(s => s.ParkState).ToList();
            // Add multi-state parks to final list
            foreach (var extraState in multiStateCollection)
            {
                finalStateList.Add(extraState);
            }
            // Add existing parks to final list if they don't contain a comma
            foreach(var state in currentStates)
            {
                if(!state.Contains(','))
                {
                    finalStateList.Add(state);
                }
            }

            parkFilter.States = new SelectList(finalStateList.Select(p => p).Distinct().OrderBy(s => s).ToList());
            return View(parkFilter);
        }

        // Created in order to send the information to a new area, redirecting back to the index, but with new information
        [HttpPost]
        public async Task<ActionResult> Index(ParkFilterViewModel parkFilter) // Passing in parkFilter Objects from above method.
        {
            await GetApiKey(); // Needed to use API key, to populate the US map
            await RunBasicParkJson();
            parkFilter.Parks = await db.Parks.Where(p => p.ParkState.Contains(parkFilter.SelectedState)).ToListAsync(); // Matching Park database park name with filter's Selection
            parkFilter.States = new SelectList(parkFilter.Parks.Select(p => p.ParkState).ToList()); // Get IEnum error if the this isn't here. Adds list back to page
            return View(parkFilter);
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
            await RunBasicParkJson();
            await RunWeatherJson(apiKeys, park);
            await RunHikingJson(apiKeys, park);
            await GetApiKey();
            await GetParkMarker(id);
            //await GetLoopListForTrails(id);
            //SelectStateForFilter();
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
        public async Task<ActionResult> Edit(int? id)
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

        public ActionResult AddParkToWishList(int? id) // why does it have to be id rather than parkId?
        {
            var user = User.Identity.GetUserId(); // Get Application user to match against all Hiker records
            HikerParkWishlist hikerParkWishlist = new HikerParkWishlist(); // Instantiate new wish list item
            Hiker hiker = db.Hikers.Where(h => h.ApplicationId == user).FirstOrDefault(); // Find correct, logged in user
            hikerParkWishlist.HikerId = hiker.HikerId; // Add HikerId to database
            // Add ParkId to database
            int convertedParkId = Convert.ToInt32(id);  // Convert passed park Id to acceptable int format
            hikerParkWishlist.ParkId = convertedParkId; // Add to database
            // Add park name to wishlist
            var parkWishName = db.Parks.Where(p => p.ParkId == hikerParkWishlist.ParkId).FirstOrDefault();
            hikerParkWishlist.ParkName = parkWishName.ParkName;
            db.HikerParkWishlists.Add(hikerParkWishlist);
            db.SaveChanges();
            return RedirectToAction("Details", "Hikers", new { id = hiker.HikerId } );
        }

        // ------------------ Get Lat Long -----------------------------
        public async Task GetLatLong(Park park)
        {
            var comboLatLong = park.ComboParkLatLng;

            // NOT AN INSTANCE ERROR BECAUSE SOME LATLNGS ARE EMPTY OR NULL. FIGURE OUT WHISH ONES ARE ERRORING OUT
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

        public async Task RunBasicParkJson()
        {
            string parkKey = ApiKeys.NpsKey;
            int apiLimit = 150;
            string url = $"https://developer.nps.gov/api/v1/parks?q=National%20Park&limit={apiLimit}&api_key={parkKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            Park park = new Park(); // Instantiate a new park
            if (response.IsSuccessStatusCode)
            {
                NpsJsonInfo npsJsonInfo = JsonConvert.DeserializeObject<NpsJsonInfo>(jsonresult); // Run through entire JSON file
                var parkList = npsJsonInfo.data.Select(m => m).ToList(); // returns entire list of Parks, up to 150, this only has 90 based of the National Parks query

                foreach (var singlePark in parkList)
                {
                    park.Designation = singlePark.designation; // Temporary National Parks holding
                    if (park.Designation.Contains("National and State Parks") || park.Designation.Contains("National Park")) // First statement to add Redwood
                    {
                        // Missing the following
                        //  National Park of American Samoa // No designation
                        //  Sequoia Combined with King's Canyon
                        park.ParkName = singlePark.fullName;
                        park.ParkState = singlePark.states;
                        park.ParkDescription = singlePark.description;
                        park.ComboParkLatLng = singlePark.latLong; // Temporary latlong holding
                        park.ParkCode = singlePark.parkCode;

                        var uniqueParkCode = db.Parks.Where(c => c.ParkCode == singlePark.parkCode).FirstOrDefault();
                        if (uniqueParkCode == null)
                        {
                            db.Parks.Add(park);
                            if (park.ComboParkLatLng != String.Empty && park.ComboParkLatLng != null)
                            {
                                await GetLatLong(park);
                            }
                        }
                        await db.SaveChangesAsync();
                    }
                }
                await db.SaveChangesAsync();
            }
        }

        //// --------------------- Run Google Place API call -----------------------------------------------------------
        //public async Task RunPlacesJson(Park park)
        //{
        //    string placeKey = ApiKeys.GooglePlacesKey;
        //    string url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=-33.8670522,151.1957362&radius=50000&type=restaurant&key={placeKey}";
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.GetAsync(url);
        //    string jsonresult = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        NpsJsonInfo npsJsonInfo = JsonConvert.DeserializeObject<NpsJsonInfo>(jsonresult); // Run through entire JSON file
        //        Datum parkInfo = npsJsonInfo.data.Where(p => p.parkCode == park.ParkCode).Single(); // Look into individual park when passing id in URL
        //        // From here on out, refer to parkInfo instead of npsJsonInfo
        //        await GetParkDescription(park, parkInfo);
        //        await GetFullParkName(park, parkInfo);
        //        await GetParkState(park, parkInfo);
        //        await GetLatLong(park, parkInfo);
        //        await db.SaveChangesAsync();
        //    }
        //}


        //  -------///////------START TRAIL RELATED METHODS-----------\\\\\\\\\\\\\\\\\\\---------------
        // ------------------ Get Trail Ratings --------------------

        public async Task GetAverageTrailRating()
        {
            //var user = User.Identity.GetUserId();
            List<HikerTrailRating> AllRatings = db.HikerTrailRatings.ToList(); // Get Full list of trails
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
            await db.SaveChangesAsync();
        }

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
                    await db.SaveChangesAsync();
                }
                else
                {
                    hikingTrail.TrailSummary = trailSummary;
                    await db.SaveChangesAsync();
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
                    await db.SaveChangesAsync();
                }
                else
                {
                    hikingTrail.TrailCondition = "No condition status available at this time";
                    await db.SaveChangesAsync();
                }

                //await GetAverageTrailRating();

                // Check to see if it already exists
                var trailCode = db.HikingTrails.Where(c => c.HikingApiCode == hikingTrail.HikingApiCode).FirstOrDefault();
                if (trailCode == null)
                {
                    db.HikingTrails.Add(hikingTrail);
                    await db.SaveChangesAsync();
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
                //await GetAverageTrailRating();
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
