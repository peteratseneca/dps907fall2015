using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// new...
using LinkRelationsMore.ServiceLayer;
using AutoMapper;

namespace LinkRelationsMore.Controllers
{
    public class ManufacturersController : ApiController
    {
        // Reference to the worker object
        private Worker w;
        private Manager m;

        public ManufacturersController()
        {
            w = new Worker();
            m = new Manager(w);
        }

        // GET: api/Manufacturers
        public IHttpActionResult Get()
        {
            // Get all
            var fetchedObjects = w.Manufacturers.GetAll();

            // Create an object to be returned
            ManufacturersLinked result = new ManufacturersLinked
                (Mapper.Map<IEnumerable<ManufacturerWithLink>>(fetchedObjects));

            return Ok(result);
        }

        // GET: api/Manufacturers/5
        public IHttpActionResult Get(int? id)
        {
            // Fetch the object
            var fetchedObject = w.Manufacturers.GetById(id.GetValueOrDefault());

            // Continue?
            if (fetchedObject == null) { return NotFound(); }

            ManufacturerLinked result = new ManufacturerLinked
                (Mapper.Map<ManufacturerWithLink>(fetchedObject));

            return Ok(result);
        }

        // POST: api/Manufacturers
        public IHttpActionResult Post([FromBody]ManufacturerAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = w.Manufacturers.Add(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id });

            // Use the factory constructor for the "add new" use case
            ManufacturerLinked result = new ManufacturerLinked
                (Mapper.Map<ManufacturerWithLink>(addedItem), addedItem.Id);

            return Created(uri, result);
        }

        /*
        // PUT: api/Manufacturers/5
        public void Put(int id, [FromBody]string value)
        {
        }
        */

        // DELETE: api/Manufacturers/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            w.Manufacturers.DeleteExisting(id);
        }

    }

}
