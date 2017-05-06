using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GardenationApp.Models;
using GardenationApp.ViewModels;
using GardenationApp.Helpers;

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

            if (garden == null)
            {
                return HttpNotFound();
            }

            //Create plant, water, and harvest prompts for each vegetable that needs it
            GardenHelpers.DailyCheck(garden);

            db.SaveChanges();

            return View(garden);
        }

        // GET: Gardens/Create
        public ActionResult Create()
        {
            //TODO: Refactor create garden experience with Angular and delete this nonsense
            CreateGardenVM createGardenVM = new CreateGardenVM();
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");
            ViewBag.VegetableTypeID1 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID2 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID3 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID4 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID5 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID6 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");

            //get select list for garden size choices
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "4", Value = "4", Selected = true });
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            ViewBag.Sqft = items;

            return View(createGardenVM);
        }

        // POST: Gardens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GardenID,Name,Sqft,CityID,VegetableTypeID1,VegetableTypeID2,VegetableTypeID3,VegetableTypeID4,VegetableTypeID5,VegetableTypeID6")] CreateGardenVM createGardenVM)
        {
            if (ModelState.IsValid)
            {
                Garden newGarden = new Garden();

                //converts the viewmodel to a new garden with vegetables and prompts
                GardenHelpers.GardenInit(newGarden, createGardenVM);

                db.Gardens.Add(newGarden);
                db.SaveChanges();

                //show the details of created garden
                return RedirectToAction("Details", "Gardens", new { id = newGarden.GardenID });
            }
            return View();
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

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "4", Value = "4", Selected = true });
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            ViewBag.SqFeet = items;

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", garden.CityID);
            return View(garden);
        }

        // POST: Gardens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GardenID,Name,SqFeet,CityID")] Garden garden)
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
            //remove all the vegetables first since their gardenId is not nullable
            foreach (var veg in garden.Vegetables.ToList()) //set to list to handle foreach modify error
            {
                db.Vegetables.Remove(veg);
            }
            //remove all prompt list items
            foreach (var prompt in garden.PromptListItems.ToList())
            {
                db.PromptListItems.Remove(prompt);
            }
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
