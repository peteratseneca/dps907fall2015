using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using Lab6.Models;
using Lab6.Controllers;
using AutoMapper;
using System.Security.Claims;

namespace Lab6.ServiceLayer
{
    public class Instrument_repo : Repository<Instrument>
    {
        public Instrument_repo(ApplicationDbContext _ds) : base(_ds) { }

        // Attention - Security context for the currently-executing request
        protected ClaimsPrincipal User = HttpContext.Current.User as ClaimsPrincipal;

        // Get all, get one, and two command handlers

        /// <summary>
        /// Get all instruments
        /// </summary>
        /// <returns>Collection of InstrumentBase objects</returns>
        public IEnumerable<InstrumentWithMediaInfo> GetAll()
        {
            // Attention - Custom "get all" method, cannot use RGetAll()
            // Fetch the objects with a matching user name
            var fetchedObjects = _dbset.Where(i => i.UserName == User.Identity.Name);

            return Mapper.Map<IEnumerable<InstrumentWithMediaInfo>>(fetchedObjects);
        }

        // Attention - Matching method for the controller's GetForAllUsers method
        /// <summary>
        /// Get all instruments
        /// </summary>
        /// <returns>Collection of InstrumentBase objects</returns>
        public IEnumerable<InstrumentWithMediaInfo> GetForAllUsers()
        {
            // Attention - use the original RGetAll()
            var fetchedObjects = RGetAll(null);

            return Mapper.Map<IEnumerable<InstrumentWithMediaInfo>>(fetchedObjects);
        }

        /// <summary>
        /// Get instrument by its identifier
        /// </summary>
        /// <param name="id">Instrument identifier, int</param>
        /// <returns>InstrumentWithMedia object</returns>
        public InstrumentWithMedia GetById(int id)
        {
            // Attention - BSD requirement - "get one" must work for any authenticated user
            // Again, this is not hard, but requires some thought
            // First, recognize that the controller is protected with the [Authorize] attribute
            // That means that a user can request a resource if they're authenticated
            // Therefore here - in the repo - we do NOT need any logic
            // Second, the user name matching code (below) simply needs to be deleted
            // I left it in here, but the BSD student work would not have it there

            // Attention - Custom "get one" method - a few ways to do this...
            // Can do it in one method call, by including two conditions in the predicate
            var fetchedObject = RGetById(i => i.Id == id & i.UserName == User.Identity.Name, null);

            return (fetchedObject == null) ? null : Mapper.Map<InstrumentWithMedia>(fetchedObject);

            // Attention - Alternatively, can do it in a sequence

            // Call the base class method
            //var fetchedObject = RGetById(i => i.Id == id, null);

            // Match and return the result, ternary statement
            /*
            return (fetchedObject == null | fetchedObject.UserName != User.Identity.Name)
                ? null
                : Mapper.Map<InstrumentWithMedia>(fetchedObject);
            */

            // Match and return the result, if statement
            /*
            if (fetchedObject == null | fetchedObject.UserName != User.Identity.Name)
            {
                return null;
            }
            else
            {
                return Mapper.Map<InstrumentWithMedia>(fetchedObject);
            }
            */
        }

        /// <summary>
        /// Add instrument
        /// </summary>
        /// <param name="newItem">InstrumentAdd object</param>
        /// <returns>InstrumentBase object</returns>
        public InstrumentWithMediaInfo Add(InstrumentAdd newItem)
        {
            // Attention - Several ways to "add new"
            // The easiest (in my view) is to add the user name here
            // Alternatively, I suppose that the InstrumentAdd class could be changed
            // But that puts more burden on the caller of this method
            // So, we'll just implement our own custom method

            // Add the new object...

            // Create a new Instrument
            var addedItem = Mapper.Map<Instrument>(newItem);
            // Add the user name
            addedItem.UserName = User.Identity.Name;

            // Save
            _dbset.Add(addedItem);
            _ds.SaveChanges();

            // Return the object
            return Mapper.Map<InstrumentWithMediaInfo>(addedItem);
        }

        /// <summary>
        /// Set an instrument's photo
        /// </summary>
        /// <param name="id">Instrument identifier (int)</param>
        /// <param name="contentType">Content-Type value</param>
        /// <param name="media">Photo bytes</param>
        /// <returns>Success: True or False</returns>
        public bool SetPhoto(int id, string contentType, byte[] media)
        {
            // Ensure that we can continue
            if (string.IsNullOrEmpty(contentType) | media == null) { return false; }

            // Attempt to find the matching object
            var storedItem = _dbset.Find(id);

            // Ensure that we can continue
            // Attention - The following is probably the easiest way to do this
            // Add another condition to match the user name
            if (storedItem == null | storedItem.UserName != User.Identity.Name) { return false; }

            // Save the photo
            storedItem.PhotoContentType = contentType;
            storedItem.PhotoMedia = media;

            // Attempt to save changes
            return (SaveChanges() > 0) ? true : false;
        }

        // BSD requirement

        /// <summary>
        /// Set an instrument's sound clip
        /// </summary>
        /// <param name="id">Instrument identifier (int)</param>
        /// <param name="contentType">Content-Type value</param>
        /// <param name="media">Sound clip bytes</param>
        /// <returns>Success: True or False</returns>
        public bool SetSoundClip(int id, string contentType, byte[] media)
        {
            // Ensure that we can continue
            if (string.IsNullOrEmpty(contentType) | media == null) { return false; }

            // Attempt to find the matching object
            var storedItem = _dbset.Find(id);

            // Attention - The following is probably the easiest way to do this
            // Add another condition to match the user name
            if (storedItem == null | storedItem.UserName != User.Identity.Name) { return false; }

            // Save the photo
            storedItem.SoundClipContentType = contentType;
            storedItem.SoundClipMedia = media;

            // Attempt to save changes
            return (SaveChanges() > 0) ? true : false;
        }

    }

}
