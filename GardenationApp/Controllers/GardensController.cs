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
            
            //Create water prompts for each vegetable that is not
            foreach(var veg in garden.Vegetables)
            {
                if (veg.WaterCountdown <= 0 && veg.WaterReminderActive == false)
                {
                    veg.WaterReminderActive = true;

                    PromptListItem newWaterPrompt = new PromptListItem();
                    newWaterPrompt.Complete = false;
                    newWaterPrompt.GardenID = garden.GardenID;
                    newWaterPrompt.Message = "Time to water your " + veg.VegetableType.Name;
                    newWaterPrompt.PromptListTypeID = 1; //TODO: refactor this hard code to search for the "Water" type
                    newWaterPrompt.TriggerDate = DateTime.Now;
                    newWaterPrompt.VegetableReference = "" + veg.VegetableID;

                    db.PromptListItems.Add(newWaterPrompt);
                }
            }

            db.SaveChanges();

            //Declare ViewBags outside of if statement scope
            ViewBag.BootstrapColumnClass = "";
            ViewBag.GardenClass = "";
            ViewBag.SqftClass = "";

            if (garden.SqFeet == 4)
            {
                ViewBag.BootstrapColumnClass = "col-xs-6";
                ViewBag.GardenClass = "garden4";
                ViewBag.SqftClass = "sqft4"; //TODO: don't need this - just use size of garden
            }

            if (garden.SqFeet == 6)
            {
                ViewBag.BootstrapColumnClass = "col-xs-4";
                ViewBag.GardenClass = "garden6";
                ViewBag.SqftClass = "sqft6";
            }


            return View(garden);
        }

        // GET: Gardens/Create
        public ActionResult Create()
        {
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

                //create a garden with no vegetables
                Garden garden = new Garden();
                garden.GardenID = createGardenVM.GardenID;
                garden.Name = createGardenVM.Name;
                garden.SqFeet = Int32.Parse(createGardenVM.Sqft); //VM Sqft is string because it receives from selectlist viewbag
                garden.CreatedDate = DateTime.Now;
                garden.CityID = createGardenVM.CityID;
                db.Gardens.Add(garden);

                //create a list of the viewmodels vegetables that were passed
                List<int> ViewModelVegetableIDs = new List<int>();

                if (garden.SqFeet == 4) {
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID1);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID2);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID3);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID4);
                }
                
                if(garden.SqFeet == 6)
                {
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID1);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID2);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID3);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID4);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID5);
                    ViewModelVegetableIDs.Add(createGardenVM.VegetableTypeID6);
                }

                //Add vegetables to the list according to the size of the garden
                List<Vegetable> NewVegetableList = new List<Vegetable>();
                for(int j = 0; j < ViewModelVegetableIDs.Count; j++)
                {
                   var newVeg = new Vegetable();
                    NewVegetableList.Add(newVeg);
                }

                //for each vegetable in the list of new vegetables, set their properies at add them to the database
                int i = 0;
                foreach (var veg in NewVegetableList)
                {
                    
                    veg.VegetableTypeID = ViewModelVegetableIDs[i];
                    veg.GardenID = createGardenVM.GardenID;
                    veg.WaterReminderActive = false;
                    veg.WaterCountdown = 365; //it will never go off until it is planted/given a SeedDate
                    //Add the vegetable to the list
                    db.Vegetables.Add(veg);
                    i++;
                }

                db.SaveChanges();

                foreach(var veg2 in garden.Vegetables)
                {
                    //add a seeding prompt for each vegetable in the database
                    PromptListItem newSeedPrompt = new PromptListItem();
                    newSeedPrompt.TriggerDate = DateTime.Now;
                    //remember the garden
                    newSeedPrompt.GardenID = garden.GardenID;
                    //complete false
                    newSeedPrompt.Complete = false;
                    newSeedPrompt.PromptListTypeID = 4; //'Plant' type hardcoded based on Database  //TODO: refactor to find the ID
                    //find the name of the vegetable type based on ID
                    VegetableType vegType = new VegetableType();
                    foreach (var type in db.VegetableTypes)
                    {
                        if (type.VegetableTypeID == veg2.VegetableTypeID)
                        {
                            vegType = type;
                        }
                    }
                    newSeedPrompt.Message = " Time to plant your " + vegType.Name;
                    newSeedPrompt.VegetableReference = "" + veg2.VegetableID;

                    //add the prompt to the garden
                    db.PromptListItems.Add(newSeedPrompt);

                }

                db.SaveChanges();

                //show the details of created garden
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
            foreach(var prompt in garden.PromptListItems.ToList())
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
