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
            return View(garden);
        }

        //// GET: Gardens/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");
        //    ViewBag.VegetableTypeID1 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
        //    ViewBag.VegetableTypeID2 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
        //    ViewBag.VegetableTypeID3 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
        //    ViewBag.VegetableTypeID4 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");

        //    //get select list with name of all vegetable types
        //    //List<SelectListItem> items = new List<SelectListItem>();
        //    //items.Add(new SelectListItem { Text = "Tomatoe", Value = "0", Selected = true });
        //    //items.Add(new SelectListItem { Text = "Carrot", Value = "1" });
        //    //items.Add(new SelectListItem { Text = "Romaine Lettuce", Value = "2" });
        //    //items.Add(new SelectListItem { Text = "Onion", Value = "3" });
        //    //ViewBag.VegetableType1 = items;
        //    //ViewBag.VegetableType2 = items;
        //    //ViewBag.VegetableType3 = items;
        //    //ViewBag.VegetableType4 = items;
        //    //ViewBag.VegetableType5 = items;
        //    //ViewBag.VegetableType6 = items;

        //    return View();
        //}

        //// POST: Gardens/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "GardenID,Name,SqFeet,CityID")] Garden garden)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Gardens.Add(garden);
        //        garden.CreatedDate = DateTime.Now;
        //        db.SaveChanges();
        //        return RedirectToAction("Create", "Vegetables");
        //    }

        //    ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", garden.CityID);
        //    return View(garden);
        //}

        // GET: Gardens/Create
        public ActionResult Create()
        {
            CreateGardenVM createGardenVM = new CreateGardenVM();
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");
            ViewBag.VegetableTypeID1 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID2 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID3 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");
            ViewBag.VegetableTypeID4 = new SelectList(db.VegetableTypes, "VegetableTypeID", "Name");

            //get select list for garden size choices
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "4", Value = "4", Selected = true });
            ViewBag.SizeChoices = items;


            return View(createGardenVM);
        }

        // POST: Gardens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GardenID,Name,Size,CityID,VegetableTypeID1,VegetableTypeID2,VegetableTypeID3,VegetableTypeID4")] CreateGardenVM createGardenVM)
        {
            if (ModelState.IsValid)
            {

                //create a garden with no vegetables
                Garden garden = new Garden();
                garden.GardenID = createGardenVM.GardenID;
                garden.Name = createGardenVM.Name;
                garden.SqFeet = 4;   //createGardenVM.Size; TODO: Resolve problem with value not passing to controller
                garden.CreatedDate = DateTime.Now;
                garden.CityID = createGardenVM.CityID;
                db.Gardens.Add(garden);

                //TODO: Create a vegetable Type that represents the choice of no vegetable
                //create a list of the viewmodels vegetables that were passed
                List<int> ViewModelVegetableIDs = new List<int>();
                if(garden.SqFeet == 4) {
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID1);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID2);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID3);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID4);
                }

                //Add vegetables to the list according to the size of the garden
                List<Vegetable> NewVegetableList = new List<Vegetable>();
                for(int i = 0; i < ViewModelVegetableIDs.Count; i++)
                {
                   var newVeg = new Vegetable();
                    NewVegetableList.Add(newVeg);
                }

                //for each vegetable in the list of new vegetables, set their properies at add them to the database
                for(var i = 0; i < NewVegetableList.Count; i++)
                {
                    NewVegetableList[i].VegetableTypeID = ViewModelVegetableIDs[i];
                    NewVegetableList[i].GardenID = createGardenVM.GardenID;
                    db.Vegetables.Add(NewVegetableList[i]);
                }

                db.SaveChanges();
                return RedirectToAction("Details", "Gardens", new { id = garden.GardenID });
                
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", createGardenVM.CityID);
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
