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
    public class PromptListItemsController : Controller
    {
        private GardenationDbEntities1 db = new GardenationDbEntities1();

        // GET: PromptListItems
        public ActionResult Index()
        {
            var promptListItems = db.PromptListItems.Include(p => p.Garden).Include(p => p.PromptListType);
            return View(promptListItems.ToList());
        }

        // GET: PromptListItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromptListItem promptListItem = db.PromptListItems.Find(id);
            if (promptListItem == null)
            {
                return HttpNotFound();
            }
            return View(promptListItem);
        }

        // GET: PromptListItems/Create
        public ActionResult Create()
        {
            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name");
            ViewBag.PromptListTypeID = new SelectList(db.PromptListTypes, "PromptListTypeID", "Name");
            return View();
        }


        // POST: PromptListItems/Complete/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complete(int? id)
        {
            //get the current prompt list item
            var thisPrompt = db.PromptListItems.Find(id);
            //return 404 if not in database
            if (thisPrompt == null)
            {
                return HttpNotFound();
            }

            thisPrompt.Complete = true;
            Vegetable thisVeg = db.Vegetables.Find(Int32.Parse(thisPrompt.VegetableReference));
            thisVeg.WaterCountdown = 0;
            thisVeg.SowDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Details", "Gardens", new { id = thisPrompt.GardenID });
        }


        // POST: PromptListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PromptListItemID,TriggerDate,GardenID,PromptListTypeID,VegetableReference,Message")] PromptListItem promptListItem)
        {
            if (ModelState.IsValid)
            {
                db.PromptListItems.Add(promptListItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", promptListItem.GardenID);
            ViewBag.PromptListTypeID = new SelectList(db.PromptListTypes, "PromptListTypeID", "Name", promptListItem.PromptListTypeID);
            return View(promptListItem);
        }

        // GET: PromptListItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromptListItem promptListItem = db.PromptListItems.Find(id);
            if (promptListItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", promptListItem.GardenID);
            ViewBag.PromptListTypeID = new SelectList(db.PromptListTypes, "PromptListTypeID", "Name", promptListItem.PromptListTypeID);
            return View(promptListItem);
        }

        // POST: PromptListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PromptListItemID,TriggerDate,GardenID,PromptListTypeID,VegetableReference,Message")] PromptListItem promptListItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promptListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", promptListItem.GardenID);
            ViewBag.PromptListTypeID = new SelectList(db.PromptListTypes, "PromptListTypeID", "Name", promptListItem.PromptListTypeID);
            return View(promptListItem);
        }

        // GET: PromptListItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromptListItem promptListItem = db.PromptListItems.Find(id);
            if (promptListItem == null)
            {
                return HttpNotFound();
            }
            return View(promptListItem);
        }

        // POST: PromptListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PromptListItem promptListItem = db.PromptListItems.Find(id);
            db.PromptListItems.Remove(promptListItem);
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
