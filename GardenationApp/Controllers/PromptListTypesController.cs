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
    public class PromptListTypesController : Controller
    {
        private GardenationDbEntities1 db = new GardenationDbEntities1();

        // GET: PromptListTypes
        public ActionResult Index()
        {
            return View(db.PromptListTypes.ToList());
        }

        // GET: PromptListTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromptListType promptListType = db.PromptListTypes.Find(id);
            if (promptListType == null)
            {
                return HttpNotFound();
            }
            return View(promptListType);
        }

        // GET: PromptListTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PromptListTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PromptListTypeID,Name,ColorClass,IconClass")] PromptListType promptListType)
        {
            if (ModelState.IsValid)
            {
                db.PromptListTypes.Add(promptListType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(promptListType);
        }

        // GET: PromptListTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromptListType promptListType = db.PromptListTypes.Find(id);
            if (promptListType == null)
            {
                return HttpNotFound();
            }
            return View(promptListType);
        }

        // POST: PromptListTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PromptListTypeID,Name,ColorClass,IconClass")] PromptListType promptListType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promptListType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(promptListType);
        }

        // GET: PromptListTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromptListType promptListType = db.PromptListTypes.Find(id);
            if (promptListType == null)
            {
                return HttpNotFound();
            }
            return View(promptListType);
        }

        // POST: PromptListTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PromptListType promptListType = db.PromptListTypes.Find(id);
            db.PromptListTypes.Remove(promptListType);
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
