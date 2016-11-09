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
    
    public partial class City
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {
            this.Gardens = new HashSet<Garden>();
        }
    
        public int CityID { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string WeatherType { get; set; }
        public string SpecialMessage { get; set; }
        public Nullable<System.DateTime> SpringFrostDate { get; set; }
        public Nullable<System.DateTime> FallFrostDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Garden> Gardens { get; set; }
    }
}
