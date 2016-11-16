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
    public class GardensController : Controller
    {
        private GardenationDbEntities1 db = new GardenationDbEntities1();

        // GET: Gardens
        public ActionResult Index()
        {
            var gardens = db.Gardens.Include(g => g.City);
            return View(gardens.ToList());
        }

        // GET: Gardens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            Garden garden = db.Gardens.Find(id);
            ViewBag.Vegetable1 = "";
            foreach(var item in garden.Vegetables)
            {
                
            }

            if (garden == null)
            {
                return HttpNotFound();
            }
            return View(garden);
        }

        // GET: Gardens/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");
            //get select list with name of all vegetable types
            ViewBag.VegetableTypes = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            return View();
        }

        // POST: Gardens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GardenID,LastVisitedDate,Name,SqFeet,CityID")] Garden garden)
        {
            if (ModelState.IsValid)
            {
                db.Gardens.Add(garden);
                garden.CreatedDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Create", "Vegetables");
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", garden.CityID);
            return View(garden);
        }

        // GET: Gardens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Garden garden = db.Gardens.Find(id);
            if (garden == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", garden.CityID);
            return View(garden);
        }

        // POST: Gardens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GardenID,Name,CreatedDate,LastVisitedDate,SqFeet,CityID")] Garden garden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(garden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", garden.CityID);
            return View(garden);
        }

        // GET: Gardens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Garden garden = db.Gardens.Find(id);
            if (garden == null)
            {
                return HttpNotFound();
            }
            return View(garden);
        }

        // POST: Gardens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Garden garden = db.Gardens.Find(id);
            db.Gardens.Remove(garden);
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
