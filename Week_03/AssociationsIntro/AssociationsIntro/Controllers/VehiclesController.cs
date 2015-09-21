using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AssociationsIntro.Controllers
{
    public class VehiclesController : ApiController
    {
        // Reference to the manager object
        private Manager m = new Manager();

        // GET: api/Vehicles
        public IHttpActionResult Get()
        {
            return Ok(m.GetAllVehicles());
        }

        // GET: api/Vehicles/WithManufacturer
        [Route("api/vehicles/withmanufacturer")]
        public IHttpActionResult GetWithManufacturer()
        {
            return Ok(m.GetAllVehWithManufacturer());
        }

        // GET: api/Vehicles/5
        public IHttpActionResult Get(int? id)
        {
            // Determine whether we can continue
            if (!id.HasValue) { return NotFound(); }

            // Fetch the object, so that we can inspect its value
            var fetchedObject = m.GetOneVehicleById(id.Value);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(fetchedObject);
            }
        }

        // GET: api/Vehicles/5
        [Route("api/vehicles/{id}/withmanufacturer")]
        public IHttpActionResult GetWithManufacturer(int? id)
        {
            // Determine whether we can continue
            if (!id.HasValue) { return NotFound(); }

            // Fetch the object, so that we can inspect its value
            var fetchedObject = m.GetOneVehWithManufacturer(id.Value);

            if (fetchedObject == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(fetchedObject);
            }
        }

        // POST: api/Vehicles
        public IHttpActionResult Post([FromBody]VehicleAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null)
            {
                return BadRequest("Invalid request URI");
            }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to add the new object
                var addedItem = m.AddVehicle(newItem);

                // Notice the ApiController convenience methods
                if (addedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot add the object");
                }
                else
                {
                    // HTTP 201 with the new object in the entity body
                    // Notice how to create the URI for the Location header

                    var uri = Url.Link("DefaultApi", new { id = addedItem.Id });
                    return Created<VehicleBase>(uri, addedItem);
                }
            }
            else
            {
                // HTTP 400
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Vehicles/5
        public IHttpActionResult Put(int id, [FromBody]VehicleEdit editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != editedItem.Id)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                var changedItem = m.EditVehicle(editedItem);

                // Notice the ApiController convenience methods
                if (changedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    // HTTP 200 with the changed item in the entity body
                    return Ok<VehicleBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Vehicles/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.DeleteVehicle(id);
        }

    }

}
