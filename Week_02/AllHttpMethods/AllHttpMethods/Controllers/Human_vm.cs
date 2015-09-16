using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace AllHttpMethods.Controllers
{
    // For a user interface select item control
    public class HumanList
    {
        public int Id { get; set; }
        public string FamilyName { get; set; }
        public string GivenNames { get; set; }
        public string FullName { get { return string.Format("{0}, {1}", this.FamilyName, this.GivenNames); } }
    }

    // Used to add a new object
    public class HumanAdd
    {
        [Required, StringLength(50)]
        public string FamilyName { get; set; }

        [Required, StringLength(50)]
        public string GivenNames { get; set; }
        
        public DateTime BirthDate { get; set; }

        [StringLength(50)]
        public string FavouriteColour { get; set; }
    }

    // Typical base class with all or most of the object's important properties
    // Notice the inheritance pattern
    public class HumanBase : HumanAdd
    {
        public int Id { get; set; }
    }

    // Customized object for editing
    // In this use case, we allow ONLY the favorite colour to be edited
    // We cannot edit the name or birth date, because those properties are absent
    public class HumanEdit
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string FavouriteColour { get; set; }
    }

}