using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace LinkRelationsIntro.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            // There will be logic elsewhere to set the allowable range for the following
            this.ModelYear = DateTime.Now.Year;
            this.MSRP = 20000;
        }
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Model { get; set; }

        [Required, StringLength(50)]
        public string Trim { get; set; }

        public int ModelYear { get; set; }

        public int MSRP { get; set; }

        [Required]
        public string Manufacturer { get; set; }
    }

}
