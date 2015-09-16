using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AllHttpMethods.Controllers
{
    public class HumansController : ApiController
    {
        // Reference to the data service operations manager class
        private Manager m = new Manager();

        // GET: api/Humans
        public IHttpActionResult Get()
        {
            return Ok(m.GetAllHumans());
        }

        // New method, added by your professor, not the scaffolder
        // GET: api/Humans?list
        public IHttpActionResult Get(string list)
        {
            // If the request handling pipeline receives...
            // - a GET request
            // - for the collection URI
            // - and it has a "list" query string key
            // This method will get called

            return Ok(m.GetAllForList());
        }

        // GET: api/Humans/5
        public IHttpActionResult Get(int id)
        {
            // Attempt to get the matching object
            var fetchedObject = m.GetHumanById(id);

            // Notice the ApiController convenience methods
            if (fetchedObject == null)
            {
                // HTTP 404
                return NotFound();
            }
            else
            {
                // HTTP 200 with the object in the HTTP message entity body
                return Ok(fetchedObject);
            }
        }

        // POST: api/Humans
        public IHttpActionResult Post([FromBody]HumanAdd newItem)
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
                var addedItem = m.AddHuman(newItem);

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
                    return Created<HumanBase>(uri, addedItem);
                }
            }
            else
            {
                // HTTP 400
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Humans/5
        public IHttpActionResult Put(int id, [FromBody]HumanEdit editedItem)
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
                var changedItem = m.EditHuman(editedItem);

                // Notice the ApiController convenience methods
                if (changedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    // HTTP 200 with the changed item in the entity body
                    return Ok<HumanBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Humans/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.DeleteHuman(id);
        }
    
    }

}
