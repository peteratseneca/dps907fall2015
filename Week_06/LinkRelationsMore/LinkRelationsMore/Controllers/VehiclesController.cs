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
    public class VehiclesController : ApiController
    {
        // Reference to the worker object
        private Worker w;
        private Manager m;

        public VehiclesController()
        {
            w = new Worker();
            m = new Manager(w);
        }

        // GET: api/Vehicles
        public IHttpActionResult Get()
        {
            // Get all
            var fetchedObjects = w.Vehicles.GetAll();

            // Create an object to be returned
            VehiclesLinked result = new VehiclesLinked
                (Mapper.Map<IEnumerable<VehicleWithLink>>(fetchedObjects));

            return Ok(result);
        }

        // GET: api/Vehicles/5
        public IHttpActionResult Get(int? id)
        {
            // Fetch the object
            var fetchedObject = w.Vehicles.GetById(id.GetValueOrDefault());

            // Continue?
            if (fetchedObject == null) { return NotFound(); }

            VehicleLinked result = new VehicleLinked
                (Mapper.Map<VehicleWithLink>(fetchedObject));

            return Ok(result);
        }

        // POST: api/Vehicles
        public IHttpActionResult Post([FromBody]VehicleAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = w.Vehicles.Add(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id });

            // Use the factory constructor for the "add new" use case
            VehicleLinked result = new VehicleLinked
            (Mapper.Map<VehicleWithLink>(addedItem), addedItem.Id);

            return Created(uri, result);
        }

        /*
        // PUT: api/Vehicles/5
        public void Put(int id, [FromBody]string value)
        {
        }
        */

        // DELETE: api/Vehicles/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            w.Vehicles.DeleteExisting(id);
        }

    }

}
