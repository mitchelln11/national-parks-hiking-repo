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

namespace NationalParksHiking.Controllers
{
    public class TrailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trails
        public async Task<ActionResult> Index()
        {
            return View(await db.Trails.ToListAsync());
        }

        // GET: Trails/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trail trail = await db.Trails.FindAsync(id);
            if (trail == null)
            {
                return HttpNotFound();
            }
            return View(trail);
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
        public async Task<ActionResult> Create([Bind(Include = "TrailId,TrailName,TrailDifficulty,TrailLat,TrailLng")] Trail trail)
        {
            if (ModelState.IsValid)
            {
                db.Trails.Add(trail);
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
            Trail trail = await db.Trails.FindAsync(id);
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
            Trail trail = await db.Trails.FindAsync(id);
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
            Trail trail = await db.Trails.FindAsync(id);
            db.Trails.Remove(trail);
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
    }
}
