using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using AssociationsIntro.Models;
using AutoMapper;

// This source code file is used as the app's single central location 
// to hold code for data service operations

// Notice (above) that it has a reference to the 'design model' classes
// Notice (below) that its public methods accept and return instances of 'resource model' objects or collections

namespace AssociationsIntro.Controllers
{
    public class Manager
    {
        // Reference to the facade services class
        private ApplicationDbContext ds = new ApplicationDbContext();

        // Add methods for the data service operations

        // All manufacturers with their vehicles
        public IEnumerable<ManufacturerWithVehicles> GetAllMfrWithVehicles()
        {
            // Note that we must .Include("Vehicles") to fetch the associated objects
            var fetchedObjects = ds.Manufacturers.Include("Vehicles").OrderBy(man => man.Name);

            return Mapper.Map<IEnumerable<ManufacturerWithVehicles>>(fetchedObjects);
        }

        // One manufacturer with its vehicles
        public ManufacturerWithVehicles GetOneMfrWithVehicles(int id)
        {
            // Note that we must .Include("Vehicles") to fetch the associated objects
            var fetchedObject = ds.Manufacturers.Include("Vehicles").SingleOrDefault(i => i.Id == id);

            return (fetchedObject == null) ? null : Mapper.Map<ManufacturerWithVehicles>(fetchedObject);
        }

        // All vehicles with manufacturer info
        public IEnumerable<VehicleWithManufacturer> GetAllVehWithManufacturer()
        {
            // Note that we must .Include("Manufacturer") to fetch the associated object
            var fetchedObjects = ds.Vehicles.Include("Manufacturer").OrderBy(veh => veh.Model);

            return Mapper.Map<IEnumerable<VehicleWithManufacturer>>(fetchedObjects);
        }

        // One vehicle with its manufacturer info
        public VehicleWithManufacturer GetOneVehWithManufacturer(int id)
        {
            // Note that we must .Include("Manufacturer") to fetch the associated object
            var fetchedObject = ds.Vehicles.Include("Manufacturer").SingleOrDefault(i => i.Id == id);

            return (fetchedObject == null) ? null : Mapper.Map<VehicleWithManufacturer>(fetchedObject);
        }

        // Add new manufacturer
        public ManufacturerBase AddManufacturer(ManufacturerAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Add the new object
                Manufacturer addedItem = Mapper.Map<Manufacturer>(newItem);

                ds.Manufacturers.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return Mapper.Map<ManufacturerBase>(addedItem);
            }
        }

        // Add new vehicle
        public VehicleBase AddVehicle(VehicleAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Must validate the Manufacturer
                var associatedItem = ds.Manufacturers.Find(newItem.ManufacturerId);
                if (associatedItem == null)
                {
                    return null;
                }

                // Add the new object
                Vehicle addedItem = Mapper.Map<Vehicle>(newItem);
                addedItem.Manufacturer = associatedItem;

                ds.Vehicles.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return Mapper.Map<VehicleBase>(addedItem);
            }
        }

        public ManufacturerBase EditManufacturer(ManufacturerEdit editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            var storedItem = ds.Manufacturers.Find(editedItem.Id);

            if (storedItem == null)
            {
                return null;
            }
            else
            {
                // Fetch the object from the data store - ds.Entry(storedItem)
                // Get its current values collection - .CurrentValues
                // Set those to the edited values - .SetValues(editedItem)
                ds.Entry(storedItem).CurrentValues.SetValues(editedItem);
                // The SetValues() method ignores missing properties and navigation properties
                ds.SaveChanges();

                return Mapper.Map<ManufacturerBase>(storedItem);
            }
        }

        public VehicleBase EditVehicle(VehicleEdit editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            // The Vehicle class has a Required attribute on the associated Manufacturer
            // Therefore we MUST use Include to fetch the associated object

            // If we miss this step, the "save" task will fail validation,
            // because the Required property will be null

            // So, comment out the typical statement...
            //var storedItem = ds.Vehicles.Find(editedItem.Id);

            // And replace it with one that includes the Required associated object
            var storedItem = ds.Vehicles.Include("Manufacturer")
                .SingleOrDefault(v => v.Id == editedItem.Id);

            if (storedItem == null)
            {
                return null;
            }
            else
            {
                // Fetch the object from the data store - ds.Entry(storedItem)
                // Get its current values collection - .CurrentValues
                // Set those to the edited values - .SetValues(editedItem)
                ds.Entry(storedItem).CurrentValues.SetValues(editedItem);
                // The SetValues() method ignores missing properties and navigation properties
                ds.SaveChanges();

                return Mapper.Map<VehicleBase>(storedItem);
            }
        }
        public void DeleteManufacturer(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Manufacturers.Find(id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // If this fails, throw an exception (as above)
                    // This implementation just prevents an error from bubbling up

                    // This WILL fail if you attempt to remove a manufacturer
                    // that has an association with one or more vehicles

                    ds.Manufacturers.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        public void DeleteVehicle(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Vehicles.Find(id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // If this fails, throw an exception (as above)
                    // This implementation just prevents an error from bubbling up
                    ds.Vehicles.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }

    }

}
