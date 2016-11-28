using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GardenationApp.Models;

namespace GardenationApp.Controllers
{
    public class VegetablesController : Controller
    {
        private GardenationDbEntities1 db = new GardenationDbEntities1();

        // GET: Vegetables
        public ActionResult Index()
        {
            var vegetables = db.Vegetables.Include(v => v.Garden).Include(v => v.VegetableType);
            return View(vegetables.ToList());
        }

        // GET: Vegetables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vegetable vegetable = db.Vegetables.Find(id);
            if (vegetable == null)
            {
                return HttpNotFound();
            }
            return View(vegetable);
        }

        // GET: Vegetables/Create
        public ActionResult Create()
        {
            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name");
            ViewBag.VegetableTypeID = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            return View();
        }

        // POST: Vegetables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VegetableID,WaterCountdown,SowDate,GardenID,VegetableTypeID,HarvestedDate,HarvestSuggestionDate,WaterReminderActive")] Vegetable vegetable)
        {
            if (ModelState.IsValid)
            {
                db.Vegetables.Add(vegetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", vegetable.GardenID);
            ViewBag.VegetableTypeID = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name", vegetable.VegetableTypeID);
            return View(vegetable);
        }

        // GET: Vegetables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vegetable vegetable = db.Vegetables.Find(id);
            if (vegetable == null)
            {
                return HttpNotFound();
            }
            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", vegetable.GardenID);
            ViewBag.VegetableTypeID = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name", vegetable.VegetableTypeID);
            return View(vegetable);
        }

        // POST: Vegetables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VegetableID,WaterCountdown,SowDate,GardenID,VegetableTypeID,HarvestedDate,HarvestSuggestionDate,WaterReminderActive")] Vegetable vegetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vegetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", vegetable.GardenID);
            ViewBag.VegetableTypeID = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name", vegetable.VegetableTypeID);
            return View(vegetable);
        }

        // GET: Vegetables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vegetable vegetable = db.Vegetables.Find(id);
            if (vegetable == null)
            {
                return HttpNotFound();
            }
            return View(vegetable);
        }

        // POST: Vegetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vegetable vegetable = db.Vegetables.Find(id);
            db.Vegetables.Remove(vegetable);
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
