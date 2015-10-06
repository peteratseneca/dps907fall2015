using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using System.ComponentModel.DataAnnotations;

namespace LinkRelationsMore.Controllers
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

    public class ManufacturerAddTemplate
    {
        public string Name { get { return "Manufacturer name, required, string, up to 50 characters"; } }
        public string Country { get { return "Country, required, string, up to 50 characters"; } }
        public string YearStarted { get { return "Year started, required, number, ranges from 1850 to 2200"; } }
    }

    public class ManufacturerBase : ManufacturerAdd
    {
        public int Id { get; set; }
    }

    public class ManufacturerFull : ManufacturerBase
    {
        public IEnumerable<VehicleBase> Vehicles { get; set; }
    }

    public class ManufacturerEdit
    {
        // Must include the object's identifier as a property
        [Range(1, UInt32.MaxValue)]
        public int Id { get; set; }

        // We can decide what other properties can be edited
        // This is based on the use case, and the intent of the property's data
        // For this use case, we have decided that only the "Name" property can be changed
        [Required, StringLength(50)]
        public string Name { get; set; }
    }

    // ############################################################
    // Classes that have link relations

    // Attention - The "with link" class now inherits from "full", not "base"

    public class ManufacturerWithLink : ManufacturerFull
    {
        public Link Link { get; set; }
    }

    public class ManufacturerLinked : LinkedItem<ManufacturerWithLink>
    {
        // Constructor - call the base class constructor

        // Attention - Use the appropriate constructor for your use case

        // All use cases except "add new"
        public ManufacturerLinked(ManufacturerWithLink item) : base(item) { }

        // "Add new" use case
        public ManufacturerLinked(ManufacturerWithLink item, int id) : base(item, id) { }
    }

    public class ManufacturersLinked : LinkedCollection<ManufacturerWithLink>
    {
        // Constructor - call the base class constructor
        public ManufacturersLinked(IEnumerable<ManufacturerWithLink> collection) : base(collection)
        {
            Template = new ManufacturerAddTemplate();
        }

        public ManufacturerAddTemplate Template { get; set; }
    }

}
