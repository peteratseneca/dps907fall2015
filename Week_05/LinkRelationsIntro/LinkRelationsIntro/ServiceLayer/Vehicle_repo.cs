using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using LinkRelationsIntro.Models;
using LinkRelationsIntro.Controllers;
using AutoMapper;

namespace LinkRelationsIntro.ServiceLayer
{
    public class Vehicle_repo : Repository<Vehicle>
    {
        // Constructor
        public Vehicle_repo(ApplicationDbContext ds) : base(ds) { }

        public IEnumerable<VehicleBase> GetAll()
        {
            // Call the base class method
            var fetchedObjects = RGetAll(null);

            // Notice that we can do the sort here if we want to
            return Mapper.Map<IEnumerable<VehicleBase>>(fetchedObjects.OrderBy(v => v.Model));
        }

        public VehicleBase GetById(int id)
        {
            // Call the base class method
            var fetchedObject = RGetById(i => i.Id == id, null);

            return (fetchedObject == null) ? null : Mapper.Map<VehicleBase>(fetchedObject);
        }

        public VehicleBase Add(VehicleAdd newItem)
        {
            // Add the new object
            var addedItem = RAdd(Mapper.Map<Vehicle>(newItem));

            // Return the object
            return Mapper.Map<VehicleBase>(addedItem);
        }

    }

}
