using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NationalParksHiking.Models;
using Newtonsoft.Json;

namespace NationalParksHiking.Controllers
{
    public class HikersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hikers
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            Hiker hiker = db.Hikers.Where(h => h.ApplicationId == userId).FirstOrDefault();
            return View("Home", "Home" );
            //return View(db.Hikers.ToList());
        }

        // GET: Hikers/Details/5
        public ActionResult Details(int? id)
        {
            string userLoggedIn = User.Identity.GetUserId();
            Hiker personLoggedIn = db.Hikers.Where(u => u.ApplicationId == userLoggedIn).FirstOrDefault();
            id = personLoggedIn.HikerId; // assigns logged in user id -- works

            // Add items to wishlist
            var hikerParkWishlist = db.HikerParkWishlists.Where(w => w.HikerId == id).ToList(); // Compares logged in user with id on Wishlist junction table -- works - shows correct amount on wishlist
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hiker hiker = db.Hikers.Find(id);
            if (hiker == null)
            {
                return HttpNotFound();
            }
            @RedirectToRoute("Details", new { id = hiker.HikerId});
            return View(hiker); 
        }

        // GET: Hikers/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Hikers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HikerId,FirstName,LastName,StreetAddress,City,State,Latitude,Longitude,ApplicationId")] Hiker hiker, ApiKeys apiKeys)
        {
            if (ModelState.IsValid)
            {
                string newuserid = User.Identity.GetUserId();
                hiker.ApplicationId = newuserid;
                await GetHikerLatLong(apiKeys, hiker);
                db.Hikers.Add(hiker);
                db.SaveChanges();
                return RedirectToAction("Details", "Hikers", new { id = hiker.HikerId });
            }
            return View(hiker);
        }

        // GET: Hikers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hiker hiker = db.Hikers.Find(id);
            if (hiker == null)
            {
                return HttpNotFound();
            }
            return View(hiker);
        }

        // POST: Hikers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HikerId,FirstName,LastName,StreetAddress,City,State,Latitude,Longitude,ApplicationId")] Hiker hiker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hiker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Hikers", new { id = hiker.HikerId });
            }
            return View(hiker);
        }

        // GET: Hikers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hiker hiker = db.Hikers.Find(id);
            if (hiker == null)
            {
                return HttpNotFound();
            }
            return View(hiker);
        }

        // POST: Hikers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hiker hiker = db.Hikers.Find(id);
            db.Hikers.Remove(hiker);
            db.SaveChanges();
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


        public async Task GetHikerLatLong(ApiKeys apiKeys, Hiker hiker)
        {
            string HikerKey = apiKeys.GeoKey;
            string HikerAddress = hiker.StreetAddress;
            string HikerCity = hiker.City;
            string HikerState = hiker.State;
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={HikerAddress},+{HikerCity},+{HikerState}&key={HikerKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                HikerJsonInfo hikerJsonInfo = JsonConvert.DeserializeObject<HikerJsonInfo>(jsonresult);
                string HikerLat = hikerJsonInfo.results[0].geometry.location.lat.ToString();
                string HikerLng = hikerJsonInfo.results[0].geometry.location.lng.ToString();
                hiker.Latitude = HikerLat.ToString();
                hiker.Longitude = HikerLng.ToString();
                await db.SaveChangesAsync();
            }
        }

        public ActionResult GetParkName()
        {
            string userid = User.Identity.GetUserId(); // Get user Id
            var hiker = db.Hikers.Where(h => h.ApplicationId == userid).FirstOrDefault(); // Find current user
            List<Park> park = db.Parks.ToList();  // Displays full, existing list of parks
            List<string> Wishlist = new List<string>(); // Instantiate a blank list

            foreach (var parklist in park)
            {
                string parkName = parklist.ParkName;
                if (parklist.ParkName != null)
                {
                    Wishlist.Add(parkName); // Add Every park name to the db.Parks database
                }
            }
            db.SaveChanges();
            return View(Wishlist);
        }
    }
}
