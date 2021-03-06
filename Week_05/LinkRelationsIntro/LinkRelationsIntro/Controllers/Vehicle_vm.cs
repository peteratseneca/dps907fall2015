﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new
using System.ComponentModel.DataAnnotations;

namespace LinkRelationsIntro.Controllers
{
    public class VehicleAdd
    {
        public VehicleAdd()
        {
            this.ModelYear = DateTime.Now.Year;
            this.MSRP = 20000;
        }

        [Required, StringLength(50)]
        public string Model { get; set; }

        [Required, StringLength(50)]
        public string Trim { get; set; }

        [Range(1880,3000)]
        public int ModelYear { get; set; }

        [Range(1,UInt32.MaxValue)]
        public int MSRP { get; set; }

        [Required]
        public string Manufacturer { get; set; }
    }

    public class VehicleBase : VehicleAdd
    {
        public int Id { get; set; }
    }

    // ############################################################
    // Help info for creating a new item

    public class VehicleAddTemplate
    {
        public string Model { get { return "Vehicle model name, required, string, up to 50 characters"; } }
        public string Trim { get { return "Trim name or code, required, string, up to 50 characters"; } }
        public string ModelYear { get { return "Model year, required, number, ranges from 1880 to 3000"; } }
        public string MSRP { get { return "MSRP, required, number, ranges from 1 (dollar) on up"; } }
        public string Manufacturer { get { return "Manufacturer name, required, string, up to 200 characters"; } }
    }

    // ############################################################
    // Classes that have link relations

    public class VehicleWithLink : VehicleBase
    {
        public Link Link { get; set; }
    }

    public class VehicleLinked : LinkedItem<VehicleWithLink>
    {
        // Constructor - call the base class constructor
        public VehicleLinked(VehicleWithLink item) : base(item) { }
    }

    public class VehiclesLinked : LinkedCollection<VehicleWithLink>
    {
        // Constructor - call the base class constructor
        public VehiclesLinked(IEnumerable<VehicleWithLink> collection) : base(collection) { }
    }

}
