using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace AllHttpMethods.Models
{
    // After defining a class, go to the IdentityModels.cs source code file
    // In the ApplicationDbContext class, add a DbSet<TEntity> property 
    // to define and hold the collection

    public class Human
    {
        public Human()
        {
            // Default initial value, which can be overwritten
            this.BirthDate = DateTime.Now.AddYears(-20);
        }

        public int Id { get; set; }

        // Notice this style, one annotation per line of code
        [Required]
        [StringLength(50)]
        public string FamilyName { get; set; }

        // Notice this style, a list of annotations in one line of code
        [Required, StringLength(50)]
        public string GivenNames { get; set; }

        public DateTime BirthDate { get; set; }

        [StringLength(50)]
        public string FavouriteColour { get; set; }
    }

}
