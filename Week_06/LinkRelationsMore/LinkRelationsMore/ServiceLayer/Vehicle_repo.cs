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
    public class Vehicle_repo : Repository<Vehicle>
    {
        // Constructor
        public Vehicle_repo(ApplicationDbContext ds) : base(ds) { }

        public IEnumerable<VehicleBase> GetAll()
        {
            // Call the base class method
            var fetchedObjects = RGetAll(new[] { "Manufacturer" });

            // Notice that we can do the sort here if we want to
            return Mapper.Map<IEnumerable<VehicleBase>>(fetchedObjects.OrderBy(v => v.Model));
        }

        public VehicleFull GetById(int id)
        {
            // Call the base class method
            var fetchedObject = RGetById(i => i.Id == id, new[] { "Manufacturer" });

            return (fetchedObject == null) ? null : Mapper.Map<VehicleFull>(fetchedObject);
        }

        public VehicleBase Add(VehicleAdd newItem)
        {
            // Custom processing, because we have an associated item
            // Will not call the abstract base class "add" method

            // Ensure that we can continue
            if (newItem == null) { return null; }

            // Must validate the manufacturer
            var associatedItem = _ds.Manufacturers.Find(newItem.ManufacturerId);
            if (associatedItem == null) { return null; }

            // Attempt to add the new object
            Vehicle addedItem = Mapper.Map<Vehicle>(newItem);
            addedItem.Manufacturer = associatedItem;

            _ds.Vehicles.Add(addedItem);
            _ds.SaveChanges();

            // Return the object
            return Mapper.Map<VehicleBase>(addedItem);
        }

    }

}
