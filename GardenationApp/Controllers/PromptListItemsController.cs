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
        private GardenationDbEntities db = new GardenationDbEntities();

        // GET: PromptListItems
        public ActionResult Index()
        {
            var promptListItems = db.PromptListItems.Include(p => p.Garden);
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
            return View();
        }

        // POST: PromptListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PromptListItemID,Title,Type,ImagePath,TriggerDate,Note,GardenID")] PromptListItem promptListItem)
        {
            if (ModelState.IsValid)
            {
                db.PromptListItems.Add(promptListItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", promptListItem.GardenID);
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
            return View(promptListItem);
        }

        // POST: PromptListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PromptListItemID,Title,Type,ImagePath,TriggerDate,Note,GardenID")] PromptListItem promptListItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promptListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GardenID = new SelectList(db.Gardens, "GardenID", "Name", promptListItem.GardenID);
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
