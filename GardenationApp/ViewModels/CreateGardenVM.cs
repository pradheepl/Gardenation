﻿using GardenationApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace GardenationApp.ViewModels
{
    public class CreateGardenVM
    {
        public int GardenID { get; set; }
        [Required]
        public string Name { get; set; } 
        [Required]
        public string Sqft { get; set; }
        public int CityID { get; set; }

        public int VegetableTypeID1 { get; set; }
        public int VegetableTypeID2 { get; set; }
        public int VegetableTypeID3 { get; set; }
        public int VegetableTypeID4 { get; set; }
        public int VegetableTypeID5 { get; set; }
        public int VegetableTypeID6 { get; set; }
        public int VegetableTypeID7 { get; set; }
        public int VegetableTypeID8 { get; set; }
        public int VegetableTypeID9 { get; set; }

    }

}