//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenationApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vegetable
    {
        public int VegetableID { get; set; }
        public Nullable<int> WaterCountdown { get; set; }
        public Nullable<System.DateTime> SowDate { get; set; }
        public int GardenID { get; set; }
        public int VegetableTypeID { get; set; }
        public Nullable<System.DateTime> HarvestedDate { get; set; }
        public Nullable<System.DateTime> HarvestSuggestionDate { get; set; }
        public Nullable<bool> WaterReminderActive { get; set; }
    
        public virtual Garden Garden { get; set; }
        public virtual VegetableType VegetableType { get; set; }
    }
}
