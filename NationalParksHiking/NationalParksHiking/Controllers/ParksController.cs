using System;
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

namespace NationalParksHiking.Controllers
{
    public class ParksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Parks
        public async Task<ActionResult> Index()
        {
            return View(await db.Parks.ToListAsync());
        }

        // GET: Parks/Details/5
        public async Task<ActionResult> Details(int? id, ApiKeys apiKeys, HikingTrail hikingTrail)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Park park = await db.Parks.FindAsync(id);
            
            // !!!!!!!!!!!!! -- ONCE THE DETAILS VIEW IS HIT, IT OVERWRITES THE CONTENT WITH THE VERY FIRST RECORD INFO, DOES NOT CHANGE ALL, JUST ONCE THE VIEW IS HIT

            park.CurrentWeatherInfo = new CurrentWeatherInfo(); // Instantiate blank spot for data to bind to
            park.CurrentWeatherInfo.temperature = 876; // Doesn't matter what's here, will overwrite anyway
            park = db.Parks.Where(p => p.ParkId == id).Single();
            park.Trails = db.HikingTrails.Where(i => i.ParkId == id).ToList();
            await RunJsonClient(park, apiKeys);
            await RunWeatherJson(apiKeys, park);
            await RunHikingJson(apiKeys, park, hikingTrail);
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
        public async Task<ActionResult> Create([Bind(Include = "ParkId,ParkName,StreetAddress,ParkCity,ParkState,ParkLat,ParkLng")] Park park, ApiKeys apiKeys)
        {
            if (ModelState.IsValid)
            {
                db.Parks.Add(park);
                //await RunJsonClient(park, apiKeys);
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
        // ------------------ Get Park Description --------------------
        public async Task GetParkDescription(Park park, NpsJsonInfo npsJsonInfo)
        {
            string parkDescription = npsJsonInfo.data[0].description;
            park.ParkDescription = parkDescription;
            await db.SaveChangesAsync();
        }


        // ------------------ Get Lat Long -----------------------------
        public async Task GetLatLong(Park park, NpsJsonInfo npsJsonInfo)
        {
            string comboLatLong = npsJsonInfo.data[0].latLong; // Grabs entire lat long string.
            var latLongArray = comboLatLong.Split().ToArray(); // Splits based on comma, set to an array
            string isolatedLatitude = latLongArray[0].TrimEnd(','); // New lat variable for the 0 index, trim trailing comma
            string isolatedLongtitude = latLongArray[1].TrimEnd(','); // New lng variable for the 1 index trim trailing comma
            string latitude = isolatedLatitude.Substring(4, isolatedLatitude.Length - 4); // Remove beginning lat: text
            string longitude = isolatedLongtitude.Substring(5, isolatedLongtitude.Length - 5); // Remove beginning lng: text
            park.ParkLat = latitude;
            park.ParkLng = longitude;
            await db.SaveChangesAsync();
        }


        // ------------------ Get Park Name -----------------------------
        public async Task GetFullParkName(Park park, NpsJsonInfo npsJsonInfo)
        {
            string fullParkName = npsJsonInfo.data[0].fullName;
            park.ParkName = fullParkName;
            await db.SaveChangesAsync();
        }

        // ------------------ Get State ----------------------------------
        public async Task GetParkState(Park park, NpsJsonInfo npsJsonInfo)
        {

            string ParkState = npsJsonInfo.data[0].states;
            park.ParkState = ParkState;
            

            await db.SaveChangesAsync(); // Issues with saving the database???????
        }

        // ------------------ Run single httpclient and response call for Parks -----------------------------
        public async Task RunJsonClient(Park park, ApiKeys apiKeys)
        {
            string parkKey = apiKeys.NpsKey;
            string url = $"https://developer.nps.gov/api/v1/parks?api_key={parkKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                NpsJsonInfo npsJsonInfo = JsonConvert.DeserializeObject<NpsJsonInfo>(jsonresult);
                await GetParkDescription(park, npsJsonInfo);
                await GetFullParkName(park, npsJsonInfo);
                await GetParkState(park, npsJsonInfo);
                await GetLatLong(park, npsJsonInfo);
                await db.SaveChangesAsync();
            }
        }

        //  -------///////------START TRAIL RELATED METHODS-----------\\\\\\\\\\\\\\\\\\\---------------
        // ------------------ Get Trail Name --------------------
        public async Task GetTrailName(HikingTrail hikingTrail, HikingTrailJsonInfo hikingTrailJsonInfo)
        {
            string trailName = hikingTrailJsonInfo.trails[0].name;
            hikingTrail.TrailName = trailName;
            await db.SaveChangesAsync();
        }

        public async Task GetTrailDifficulty(HikingTrail hikingTrail, HikingTrailJsonInfo hikingTrailJsonInfo)
        {
            string trailDifficulty = hikingTrailJsonInfo.trails[0].difficulty;
            hikingTrail.TrailDifficulty = trailDifficulty;
            await db.SaveChangesAsync();
        }

        public async Task GetTrailLength(HikingTrail hikingTrail, HikingTrailJsonInfo hikingTrailJsonInfo)
        {
            double trailLength = hikingTrailJsonInfo.trails[0].length;
            hikingTrail.TrailLength = trailLength;
            await db.SaveChangesAsync();
        }

        public async Task GetTrailSummary(HikingTrail hikingTrail, HikingTrailJsonInfo hikingTrailJsonInfo)
        {
            string trailSummary = hikingTrailJsonInfo.trails[0].summary;
            hikingTrail.TrailSummary = trailSummary;
            await db.SaveChangesAsync();
        }

        // ------------------ Get Hiking Project JSON -----------------------------
        public async Task RunHikingJson(ApiKeys apiKeys, Park park, HikingTrail hikingTrail)
        {
            string hikingKey = apiKeys.HikingProjectKey;
            string parkLatitude = park.ParkLat;
            string parkLongitude = park.ParkLng;
            string url = $"https://www.hikingproject.com/data/get-trails?lat={parkLatitude}&lon={parkLongitude}&key={hikingKey}"; // Lat, long, and API Key neede for API call
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                HikingTrailJsonInfo hikingTrailJsonInfo = JsonConvert.DeserializeObject<HikingTrailJsonInfo>(jsonresult);
                await GetTrailName(hikingTrail, hikingTrailJsonInfo);
                await GetTrailDifficulty(hikingTrail, hikingTrailJsonInfo);
                await GetTrailLength(hikingTrail, hikingTrailJsonInfo);
                await GetTrailSummary(hikingTrail, hikingTrailJsonInfo);
                await db.SaveChangesAsync();
            }
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
