using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using System.ComponentModel.DataAnnotations;

namespace AssociationsIntro.Controllers
{
    public class ManufacturerAdd
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Country { get; set; }

        [Range(1850, 2200)]
        public int YearStarted { get; set; }
    }

    public class ManufacturerBase : ManufacturerAdd
    {
        public int Id { get; set; }
    }

    public class ManufacturerWithVehicles : ManufacturerBase
    {
        public IEnumerable<VehicleBase> Vehicles { get; set; }
    }

    public class ManufacturerEdit
    {
        // Must include the object's identifier as a property
        public int Id { get; set; }

        // We can decide what other properties can be edited
        // This is based on the use case, and the intent of the property's data
        // For this use case, we have decided that only the "Name" property can be changed
        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
