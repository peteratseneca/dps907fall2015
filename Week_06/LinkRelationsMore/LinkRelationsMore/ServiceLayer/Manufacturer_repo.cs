using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using LinkRelationsMore.Models;
using LinkRelationsMore.Controllers;
using AutoMapper;

namespace LinkRelationsMore.ServiceLayer
{
    public class Manufacturer_repo : Repository<Manufacturer>
    {
        // Constructor
        public Manufacturer_repo(ApplicationDbContext ds) : base(ds) { }

        public IEnumerable<ManufacturerBase> GetAll()
        {
            // Call the base class method
            var fetchedObjects = RGetAll(null);

            // Notice that we can do the sort here if we want to
            return Mapper.Map<IEnumerable<ManufacturerBase>>(fetchedObjects.OrderBy(m => m.Name));
        }

        public ManufacturerFull GetById(int id)
        {
            // Call the base class method
            var fetchedObject = RGetById(i => i.Id == id, new[] { "Vehicles" });

            return (fetchedObject == null) ? null : Mapper.Map<ManufacturerFull>(fetchedObject);
        }

        public ManufacturerBase Add(ManufacturerAdd newItem)
        {
            // Add the new object
            var addedItem = RAdd(Mapper.Map<Manufacturer>(newItem));

            // Return the object
            return Mapper.Map<ManufacturerBase>(addedItem);
        }

    }

}
