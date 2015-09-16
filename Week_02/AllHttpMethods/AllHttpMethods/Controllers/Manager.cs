using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AllHttpMethods.Models;
using AutoMapper;

// This source code file is used as the app's single central location 
// to hold code for data service operations,
// and application and business logic

// Notice (above) that it has a reference to the 'design model' classes
// Notice (below) that its public methods accept and return instances 
// of 'resource model' objects or collections

namespace AllHttpMethods.Controllers
{
    public class Manager
    {
        // Reference to the facade services class
        private ApplicationDbContext ds = new ApplicationDbContext();

        // All objects
        public IEnumerable<HumanBase> GetAllHumans()
        {
            // Get all the objects from the data store
            var fetchedObjects = ds.Humans
                .OrderBy(fn => fn.FamilyName)
                .ThenBy(gn => gn.GivenNames);

            // Return them as a 'resource model' collection
            return Mapper.Map<IEnumerable<HumanBase>>(fetchedObjects);
        }

        // All objects, for a user interface list
        public IEnumerable<HumanList> GetAllForList()
        {
            // Get all the objects from the data store
            var fetchedObjects = ds.Humans
                .OrderBy(fn => fn.FamilyName)
                .ThenBy(gn => gn.GivenNames);

            // Return them as a 'resource model' collection
            return Mapper.Map<IEnumerable<HumanList>>(fetchedObjects);
        }

        // Single object, by identifier
        public HumanBase GetHumanById(int id)
        {
            // Get the matching object
            var fetchedObject = ds.Humans.Find(id);

            // Return it (or null if not found)
            return (fetchedObject == null) ? null : Mapper.Map<HumanBase>(fetchedObject);
        }

        // Add object
        public HumanBase AddHuman(HumanAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Add the new object, by mapping from the 'resource model' object
                Human addedItem = Mapper.Map<Human>(newItem);

                ds.Humans.Add(addedItem);
                ds.SaveChanges();

                // Return the now-fully-configured 'resource model' version of the object
                return Mapper.Map<HumanBase>(addedItem);
            }
        }

        // Edit object
        public HumanBase EditHuman(HumanEdit editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            var storedItem = ds.Humans.Find(editedItem.Id);

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

                return Mapper.Map<HumanBase>(storedItem);
            }
        }

        // Delete item
        public void DeleteHuman(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Humans.Find(id);

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
                    ds.Humans.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }






        // The following method will seed the data store
        // This method is only for our own private use (not intended for public users)

        public void InitializeDataStore()
        {
            // Remove existing items
            foreach (var item in ds.Humans)
            {
                ds.Humans.Remove(item);
            }

            // Add new objects
            Human peter = new Human() { FamilyName = "McIntyre", GivenNames = "Peter" };
            ds.Humans.Add(peter);

            Human ian = new Human() { FamilyName = "Tipson", GivenNames = "Ian", BirthDate = new DateTime(1985, 04, 23) };
            ds.Humans.Add(ian);

            ds.SaveChanges();

        }
    }

}
