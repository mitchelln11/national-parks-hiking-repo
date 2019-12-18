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
    public class HikerParkWishlistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HikerParkWishlists
        public ActionResult Index()
        {
            var hikerParkWishlists = db.HikerParkWishlists.Include(h => h.Hiker).Include(h => h.Park);
            return View(hikerParkWishlists.ToList());
        }

        // GET: HikerParkWishlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HikerParkWishlist hikerParkWishlist = db.HikerParkWishlists.Find(id);
            if (hikerParkWishlist == null)
            {
                return HttpNotFound();
            }
            return View(hikerParkWishlist);
        }

        // GET: HikerParkWishlists/Create
        public ActionResult Create()
        {
            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName");
            ViewBag.ParkId = new SelectList(db.Parks, "ParkId", "ParkName");
            return View();
        }

        // POST: HikerParkWishlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HikerParkWishlistId,HikerId,ParkId")] HikerParkWishlist hikerParkWishlist)
        {
            if (ModelState.IsValid)
            {
                db.HikerParkWishlists.Add(hikerParkWishlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName", hikerParkWishlist.HikerId);
            ViewBag.ParkId = new SelectList(db.Parks, "ParkId", "ParkName", hikerParkWishlist.ParkId);
            return View(hikerParkWishlist);
        }

        // GET: HikerParkWishlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HikerParkWishlist hikerParkWishlist = db.HikerParkWishlists.Find(id);
            if (hikerParkWishlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName", hikerParkWishlist.HikerId);
            ViewBag.ParkId = new SelectList(db.Parks, "ParkId", "ParkName", hikerParkWishlist.ParkId);
            return View(hikerParkWishlist);
        }

        // POST: HikerParkWishlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HikerParkWishlistId,HikerId,ParkId")] HikerParkWishlist hikerParkWishlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hikerParkWishlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HikerId = new SelectList(db.Hikers, "HikerId", "FirstName", hikerParkWishlist.HikerId);
            ViewBag.ParkId = new SelectList(db.Parks, "ParkId", "ParkName", hikerParkWishlist.ParkId);
            return View(hikerParkWishlist);
        }

        // GET: HikerParkWishlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HikerParkWishlist hikerParkWishlist = db.HikerParkWishlists.Find(id);
            if (hikerParkWishlist == null)
            {
                return HttpNotFound();
            }
            return View(hikerParkWishlist);
        }

        // POST: HikerParkWishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HikerParkWishlist hikerParkWishlist = db.HikerParkWishlists.Find(id);
            db.HikerParkWishlists.Remove(hikerParkWishlist);
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
