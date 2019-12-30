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
    public class TrailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trails
        public async Task<ActionResult> Index()
        {
            return View(await db.HikingTrails.ToListAsync());
        }

        // GET: Trails/Details/5
        public async Task<ActionResult> Details(int? id, Trail trailName, HikingTrailJsonInfo hikingTrailJsonInfo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var trailInfo = await db.HikingTrails.FindAsync(id);
            await GetTrailName(trailName, hikingTrailJsonInfo);
            if (trailInfo == null)
            {
                return HttpNotFound();
            }
            return View(trailInfo);
        }

        // GET: Trails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TrailId,TrailName,TrailDifficulty,TrailLat,TrailLng")] HikingTrail trail)
        {
            if (ModelState.IsValid)
            {
                db.HikingTrails.Add(trail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(trail);
        }

        // GET: Trails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var trail = await db.HikingTrails.FindAsync(id);
            if (trail == null)
            {
                return HttpNotFound();
            }
            return View(trail);
        }

        // POST: Trails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TrailId,TrailName,TrailDifficulty,TrailLat,TrailLng")] Trail trail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(trail);
        }

        // GET: Trails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var trail = await db.HikingTrails.FindAsync(id);
            if (trail == null)
            {
                return HttpNotFound();
            }
            return View(trail);
        }

        // POST: Trails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var trail = await db.HikingTrails.FindAsync(id);
            db.HikingTrails.Remove(trail);
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


        public async Task GetTrailName(Trail trailName, HikingTrailJsonInfo hikingTrailJsonInfo)
        {
            // Do I need to check Foreign Key ID like we do with User ID?
            string fulltrailName = hikingTrailJsonInfo.trails[0].name;
            trailName.name = fulltrailName;
            await db.SaveChangesAsync();
        }

        // ------------------ Run single httpclient and response call -----------------------------
        public async Task GetTrailDifficulty(Park park, ApiKeys apiKeys)
        {
            // How do I pass the park ID number to a trail controller?
            // Get Lat Long from Parks database
            string trailKey = apiKeys.HikingProjectKey;
            string parkLat = park.ParkLat;
            string parkLng = park.ParkLng;
            string url = $"https://www.hikingproject.com/data/get-trails?lat={parkLat}&lon={parkLng}&key={trailKey}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonresult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                HikingTrailJsonInfo hikingTrailJsonInfo = JsonConvert.DeserializeObject<HikingTrailJsonInfo>(jsonresult);
                string TrailDifficulty = hikingTrailJsonInfo.trails[0].difficulty.ToString();
                //string userLoggedIn = User.Identity.GetUserId();
                Trail trail = new Trail();
                trail.difficulty = TrailDifficulty;
                await db.SaveChangesAsync();
            }
        }
    }
}
