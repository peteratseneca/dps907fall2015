using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// new...
using Lab6.ServiceLayer;
using AutoMapper;

namespace Lab6.Controllers
{
    // Attention - Controller protected with the Authorize attribute
    [Authorize]
    public class InstrumentsController : ApiController
    {
        // Reference to the worker object
        private Worker w;
        private Manager m;

        public InstrumentsController()
        {
            w = new Worker();
            m = new Manager(w);
        }

        // GET: api/Instruments
        /// <summary>
        /// Information for all Instruments
        /// </summary>
        /// <returns>Collection of Instrument objects</returns>
        public IHttpActionResult Get()
        {
            // Get all
            var fetchedObjects = w.Instruments.GetAll();

            InstrumentsLinked result = new InstrumentsLinked
                (Mapper.Map<IEnumerable<InstrumentWithLink>>(fetchedObjects));

            return Ok(result);
        }

        // Attention - BSD requirement - new method, "get all" for all users
        // This is not a difficult task, but it requires some thought
        // It also requires a custom route

        // GET: api/Instruments/ForAllUsers
        /// <summary>
        /// Information for all Instruments
        /// </summary>
        /// <returns>Collection of Instrument objects</returns>
        [Route("api/instruments/forallusers")]
        public IHttpActionResult GetForAllUsers()
        {
            // Get all
            var fetchedObjects = w.Instruments.GetForAllUsers();

            InstrumentsLinked result = new InstrumentsLinked
                (Mapper.Map<IEnumerable<InstrumentWithLink>>(fetchedObjects));

            return Ok(result);
        }

        // GET: api/Instruments/5
        /// <summary>
        /// Information for one Instrument
        /// </summary>
        /// <param name="id">Instrument identifier (int)</param>
        /// <returns>Instrument object</returns>
        public IHttpActionResult Get(int? id)
        {
            // This method DOES use content negotiation (aka conneg) 

            // Fetch the object
            var fetchedObject = w.Instruments.GetById(id.GetValueOrDefault());

            // Continue?
            if (fetchedObject == null) { return NotFound(); }

            // Here is the content negotiation code

            // Look for an Accept header that starts with 'image' or 'audio'

            var imageHeader = Request.Headers.Accept
                .SingleOrDefault(a => a.MediaType.ToLower().StartsWith("image/"));

            var audioHeader = Request.Headers.Accept
                .SingleOrDefault(a => a.MediaType.ToLower().StartsWith("audio/"));

            // BSD requirement

            if (imageHeader == null & audioHeader == null)
            {
                // Normal processing for a JSON result

                InstrumentLinked result = new InstrumentLinked(Mapper.Map<InstrumentWithLink>(fetchedObject));

                // Get the request URI path
                string self = Request.RequestUri.AbsolutePath;

                // Add link relations to indicate a command that can be performed
                result.Links.Add(new Link() { Rel = "edit", Href = self + "/setphoto", ContentType = "image/*", Method = "PUT" });
                // BSD requirement
                result.Links.Add(new Link() { Rel = "edit", Href = self + "/setsoundclip", ContentType = "audio/*", Method = "PUT" });

                // If a media item exists, add some custom links...

                if (result.Item.PhotoMediaLength > 0)
                {
                    // Add a link relation to indicate that an alternate representation is available
                    result.Links.Add(new Link() { Rel = "self", Href = self, ContentType = "image/*", Title = "Instrument photo", Method = "GET" });
                }

                // BSD requirement
                if (result.Item.SoundClipMediaLength > 0)
                {
                    // Add a link relation to indicate that an alternate representation is available
                    result.Links.Add(new Link() { Rel = "self", Href = self, ContentType = "audio/*", Title = "Instrument sound clip", Method = "GET" });
                }

                // Return the result, using the built-in media formatters (JSON, XML)
                return Ok(result);
            }
            else
            {
                // Special processing for a media result

                // BSD requirement has additional logic to check for two headers

                if (imageHeader != null)
                {
                    // Confirm that a media item exists
                    if (fetchedObject.PhotoMediaLength > 0)
                    {
                        // Return the result, using the custom media formatter
                        return Ok(fetchedObject.PhotoMedia);
                    }
                }

                if (audioHeader != null)
                {
                    // Confirm that a media item exists
                    if (fetchedObject.SoundClipMediaLength > 0)
                    {
                        // Return the result, using the custom media formatter
                        return Ok(fetchedObject.SoundClipMedia);
                    }
                }

                // Otherwise, return "not found"
                // Yes, this is correct. Read the RFC: https://tools.ietf.org/html/rfc7231#section-6.5.4
                return NotFound();
            }

        }

        // POST: api/Instruments
        /// <summary>
        /// Add a new Instrument
        /// </summary>
        /// <param name="newItem">New Instrument object (the template has the object schema)</param>
        /// <returns>New Instrument object</returns>
        public IHttpActionResult Post([FromBody]InstrumentAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = w.Instruments.Add(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id });

            // Use the factory constructor for the "add new" use case
            InstrumentLinked result = new InstrumentLinked
                (Mapper.Map<InstrumentWithLink>(addedItem), addedItem.Id);

            return Created(uri, result);
        }

        // PUT: api/Instruments/5/SetPhoto
        /// <summary>
        /// Add an instrument photo to the Instrument object
        /// </summary>
        /// <param name="id">Instrument identifier (int)</param>
        /// <param name="media">Photo (jpg, png, etc.)</param>
        /// <returns>HTTP 204</returns>
        [Route("api/instruments/{id}/setphoto")]
        public IHttpActionResult PutPhoto(int id, [FromBody]byte[] media)
        {
            // Get the Content-Type header from the request
            var contentType = Request.Content.Headers.ContentType.MediaType;

            // Attempt to save
            if (w.Instruments.SetPhoto(id, contentType, media))
            {
                // By convention, we have decided to return HTTP 204
                // It's a 'success' code, but there's no content for a 'command' task
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                // Uh oh, some error happened, so tell the user
                return BadRequest("Unable to set the photo");
            }
        }

        // PUT: api/Instruments/5/SetSoundClip
        /// <summary>
        /// Add an instrument sound clip to the Instrument object
        /// </summary>
        /// <param name="id">Instrument identifier (int)</param>
        /// <param name="media">Sound clip (mp3, etc.)</param>
        /// <returns>HTTP 204</returns>
        [Route("api/instruments/{id}/setsoundclip")]
        public IHttpActionResult PutSoundClip(int id, [FromBody]byte[] media)
        {
            // Get the Content-Type header from the request
            var contentType = Request.Content.Headers.ContentType.MediaType;

            // Attempt to save
            if (w.Instruments.SetSoundClip(id, contentType, media))
            {
                // By convention, we have decided to return HTTP 204
                // It's a 'success' code, but there's no content for a 'command' task
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                // Uh oh, some error happened, so tell the user
                return BadRequest("Unable to set the sound clip");
            }
        }

        /*
        // PUT: api/Books/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Books/5
        public void Delete(int id)
        {
        }
        */

    }

}
