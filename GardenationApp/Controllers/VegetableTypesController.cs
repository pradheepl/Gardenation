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
    public class VegetableTypesController : Controller
    {
        private GardenationDbEntities1 db = new GardenationDbEntities1();

        // GET: VegetableTypes
        public ActionResult Index()
        {
            return View(db.VegetableTypes.ToList());
        }

        // GET: VegetableTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VegetableType vegetableType = db.VegetableTypes.Find(id);
            if (vegetableType == null)
            {
                return HttpNotFound();
            }
            return View(vegetableType);
        }

        // GET: VegetableTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VegetableTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VegetableTypeID,Name,WaterFrequency,ImagePath,EquipmentNeeded,HarvestDateMod")] VegetableType vegetableType)
        {
            if (ModelState.IsValid)
            {
                db.VegetableTypes.Add(vegetableType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vegetableType);
        }

        // GET: VegetableTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VegetableType vegetableType = db.VegetableTypes.Find(id);
            if (vegetableType == null)
            {
                return HttpNotFound();
            }
            return View(vegetableType);
        }

        // POST: VegetableTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VegetableTypeID,Name,WaterFrequency,ImagePath,EquipmentNeeded,HarvestDateMod")] VegetableType vegetableType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vegetableType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vegetableType);
        }

        // GET: VegetableTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VegetableType vegetableType = db.VegetableTypes.Find(id);
            if (vegetableType == null)
            {
                return HttpNotFound();
            }
            return View(vegetableType);
        }

        // POST: VegetableTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VegetableType vegetableType = db.VegetableTypes.Find(id);
            db.VegetableTypes.Remove(vegetableType);
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
