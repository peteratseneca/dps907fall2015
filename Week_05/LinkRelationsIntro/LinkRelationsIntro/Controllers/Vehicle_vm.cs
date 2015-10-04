using System;
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
        // Constructor
        public VehicleLinked(VehicleWithLink item)
        {
            Item = item;

            // Get the current request URL
            var absolutePath = HttpContext.Current.Request.Url.AbsolutePath;

            // Link relation for 'self' in the item
            this.Item.Link = new Link() { Rel = "self", Href = absolutePath };

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath });

            // Link relation for 'collection'
            string[] u = absolutePath.Split(new char[] { '/' });
            this.Links.Add(new Link() { Rel = "collection", Href = string.Format("/{0}/{1}", u[1], u[2]) });
        }
    }

    public class VehiclesLinked : LinkedCollection<VehicleWithLink>
    {
        // Constructor
        public VehiclesLinked(IEnumerable<VehicleWithLink> collection)
        {
            Collection = collection;

            // Get the current request URL
            var absolutePath = HttpContext.Current.Request.Url.AbsolutePath;

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath });

            // Add 'item' links for each item in the collection
            foreach (var item in this.Collection)
            {
                item.Link = new Link() { Rel = "item", Href = string.Format("{0}/{1}", absolutePath, item.Id) };
            }
        }
    }

}
