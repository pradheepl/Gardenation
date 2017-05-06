using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GardenationApp.Models;
using GardenationApp.ViewModels;
using GardenationApp.Helpers;

namespace GardenationApp.Helpers
{
    public class GardenHelpers
    {

        /// <summary>
        /// Converts the viewmodel to a garden with vegetables and prompts that can 
        /// be saved to the database
        /// </summary>
        /// <param name="garden"></param>
        /// <param name="gardenvm"></param>
        public static void GardenInit(Garden garden, CreateGardenVM gardenvm)
        {
            MergeGardenModels(garden, gardenvm);
            //LoggerInit(garden); see below
            VegetablesInit(garden, gardenvm);
            //TODO: MAJOR - GardenSageSort() put vegetables in the ideal spot for the garden type and location and sun
            DailyCheck(garden);
        }


        
        protected static void MergeGardenModels(Garden garden, CreateGardenVM gardenvm)
        {
            garden.GardenID = gardenvm.GardenID;
            garden.Name = gardenvm.Name;
            garden.SqFeet = Int32.Parse(gardenvm.Sqft); //VM Sqft is string because it receives from selectlist viewbag
            garden.CreatedDate = DateTime.Now;
            garden.CityID = gardenvm.CityID;
        }

        //TODO: Add logs to database and use this helper to log all user activities
        // so they can see all the tasks they've done later on.

        //public static void LoggerInit(Garden garden)
        //{
        //    GardenLog newLog = new GardenLog();
        //    newLog.Message = $"Created {garden.Name}";
        //    newLog.Date = DateTime.Now;
        //    newLog.UserID = 1; //admin user
        //    newLog.GardenID = garden.GardenID;
        //    garden.GardenLogs.Add(newLog);
        //}

        protected static void VegetablesInit(Garden garden, CreateGardenVM gardenvm)
        {
            //TODO:Refactor this to do it automatically depending on garden type and size
            List<int> vegIds = new List<int>();
            if (garden.SqFeet == 4)
            {
                vegIds.Add(gardenvm.VegetableTypeID1);
                vegIds.Add(gardenvm.VegetableTypeID2);
                vegIds.Add(gardenvm.VegetableTypeID3);
                vegIds.Add(gardenvm.VegetableTypeID4);
            }

            if (garden.SqFeet == 6)
            {
                vegIds.Add(gardenvm.VegetableTypeID1);
                vegIds.Add(gardenvm.VegetableTypeID2);
                vegIds.Add(gardenvm.VegetableTypeID3);
                vegIds.Add(gardenvm.VegetableTypeID4);
                vegIds.Add(gardenvm.VegetableTypeID5);
                vegIds.Add(gardenvm.VegetableTypeID6);
            }
            for (int j = 0; j < vegIds.Count; j++)
            {
                //Create the vegetables and add to the garden
                var newVeg = new Vegetable();
                newVeg.VegetableTypeID = vegIds[j];
                newVeg.GardenID = gardenvm.GardenID;
                garden.Vegetables.Add(newVeg);
            }
        }

        public static void DailyCheck(Garden garden)
        {
            //TODO: check if garden is valid and check if it has already been visited today
            if (garden.LastVisitedDate != DateTime.Now)
                SeedCheck(garden.PromptListItems, garden.Vegetables);

            WaterCheck(garden.PromptListItems, garden.Vegetables);

            if (garden.LastVisitedDate != null)
            {
                HarvestCheck(garden.PromptListItems, garden.Vegetables);
            }

        }

        protected static void SeedCheck(ICollection<PromptListItem> prompts, ICollection<Vegetable> vegetables)
        {
            foreach (Vegetable veg in vegetables)
            {
                if (veg.SowDate == null) //TODO: Revise db to be able to check if something has been planted or already reminded to plant then if not:
                {
                    //set sowreminderactive true
                    PromptListItem newSeedPrompt = new PromptListItem();
                    newSeedPrompt.Complete = false;
                    newSeedPrompt.GardenID = veg.GardenID;
                    newSeedPrompt.Message = "Plant " + veg.VegetableType.Name;
                    newSeedPrompt.TriggerDate = DateTime.Now;
                    prompts.Add(newSeedPrompt);
                }
            }
        }

        protected static void WaterCheck(ICollection<PromptListItem> prompts, ICollection<Vegetable> vegetables)
        {
            foreach (Vegetable veg in vegetables)
            {
                if (veg.WaterCountdown <= 0 && veg.WaterReminderActive == false)
                {
                    veg.WaterReminderActive = true;
                    PromptListItem newWaterPrompt = new PromptListItem();
                    newWaterPrompt.Complete = false;
                    newWaterPrompt.VegetableReference = veg.VegetableID.ToString(); //TODO: Refactor to just make this a nullable foreign key
                    newWaterPrompt.TriggerDate = DateTime.Now;
                    newWaterPrompt.Message = "Water " + veg.VegetableType.Name;
                    prompts.Add(newWaterPrompt);
                }
            }
        }

        protected static void HarvestCheck(ICollection<PromptListItem> prompts, ICollection<Vegetable> vegetables)
        {
            foreach (Vegetable veg in vegetables)
            {
                if (veg.HarvestSuggestionDate <= DateTime.Now) //TODO: Refact db to include reminder prompt active
                {
                    //reminder type active
                    PromptListItem newHarvestPrompt = new PromptListItem();
                    newHarvestPrompt.Complete = false;
                    newHarvestPrompt.GardenID = veg.GardenID;
                    newHarvestPrompt.VegetableReference = veg.VegetableID.ToString();
                    newHarvestPrompt.Message = "Harvest " + veg.VegetableType.Name;
                    newHarvestPrompt.TriggerDate = DateTime.Now;
                    prompts.Add(newHarvestPrompt);
                }
            }
        }
    }
}