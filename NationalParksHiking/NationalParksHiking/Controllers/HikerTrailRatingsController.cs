using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NationalParksHiking.Models;

namespace NationalParksHiking.Controllers
{
    public class HikerTrailRatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HikerTrailRatings
        public ActionResult Index()
        {
            var hikerTrailRatings = db.HikerTrailRatings.Include(h => h.Hiker).Include(h => h.HikingTrail);
            return View(hikerTrailRatings.ToList());
        }

        // GET: HikerTrailRatings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HikerTrailRating hikerTrailRating = db.HikerTrailRatings.Find(id);
            if (hikerTrailRating == null)
            {
                return HttpNotFound();
            }
            return View(hikerTrailRating);
        }

        // GET: HikerTrailRatings/Create
        public ActionResult Create()
        {
            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName");
            ViewBag.TrailId = new SelectList(db.HikingTrails, "TrailId", "TrailName");
            return View();
        }

        // POST: HikerTrailRatings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HikerTrailRatingId,HikerId,TrailId,RatingAmt")] HikerTrailRating hikerTrailRating)
        {
            if (ModelState.IsValid)
            {
                db.HikerTrailRatings.Add(hikerTrailRating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName", hikerTrailRating.HikerId);
            ViewBag.TrailId = new SelectList(db.HikingTrails, "TrailId", "TrailName", hikerTrailRating.TrailId);
            return View(hikerTrailRating);
        }

        // GET: HikerTrailRatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HikerTrailRating hikerTrailRating = db.HikerTrailRatings.Find(id);
            if (hikerTrailRating == null)
            {
                return HttpNotFound();
            }
            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName", hikerTrailRating.HikerId);
            ViewBag.TrailId = new SelectList(db.HikingTrails, "TrailId", "TrailName", hikerTrailRating.TrailId);
            return View(hikerTrailRating);
        }

        // POST: HikerTrailRatings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HikerTrailRatingId,HikerId,TrailId,RatingAmt")] HikerTrailRating hikerTrailRating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hikerTrailRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName", hikerTrailRating.HikerId);
            ViewBag.TrailId = new SelectList(db.HikingTrails, "TrailId", "TrailName", hikerTrailRating.TrailId);
            return View(hikerTrailRating);
        }

        // GET: HikerTrailRatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HikerTrailRating hikerTrailRating = db.HikerTrailRatings.Find(id);
            if (hikerTrailRating == null)
            {
                return HttpNotFound();
            }
            return View(hikerTrailRating);
        }

        // POST: HikerTrailRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HikerTrailRating hikerTrailRating = db.HikerTrailRatings.Find(id);
            db.HikerTrailRatings.Remove(hikerTrailRating);
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
    }
}
