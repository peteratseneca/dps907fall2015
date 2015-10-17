using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// new...
using MediaUpload.ServiceLayer;
using AutoMapper;

namespace MediaUpload.Controllers
{
    // Attention - Books controller

    public class BooksController : ApiController
    {
        // Reference to the worker object
        private Worker w;
        private Manager m;

        public BooksController()
        {
            w = new Worker();
            m = new Manager(w);
        }

        // GET: api/Books
        /// <summary>
        /// Information for all books
        /// </summary>
        /// <returns>Collection of book objects</returns>
        public IHttpActionResult Get()
        {
            // Get all
            var fetchedObjects = w.Books.GetAll();

            BooksLinked result = new BooksLinked
                (Mapper.Map<IEnumerable<BookWithLink>>(fetchedObjects));

            return Ok(result);
        }

        // GET: api/Books/5
        /// <summary>
        /// Information for one book
        /// </summary>
        /// <param name="id">Book identifier (int)</param>
        /// <returns>Book object</returns>
        public IHttpActionResult Get(int? id)
        {
            // Fetch the object
            var fetchedObject = w.Books.GetById(id.GetValueOrDefault());

            // Continue?
            if (fetchedObject == null) { return NotFound(); }

            BookLinked result = new BookLinked
                (Mapper.Map<BookWithLink>(fetchedObject));

            // Get the request URI path
            string self = Request.RequestUri.AbsolutePath;

            // Add a link relation to indicate a command that can be performed
            result.Links.Add(new Link() { Rel = "edit", Href = self + "/setphoto", ContentType = "image/*", Method = "PUT" });

            return Ok(result);
        }

        // POST: api/Books
        /// <summary>
        /// Add a new book
        /// </summary>
        /// <param name="newItem">New book object (the template has the object schema)</param>
        /// <returns>New book object</returns>
        public IHttpActionResult Post([FromBody]BookAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = w.Books.Add(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id });

            // Use the factory constructor for the "add new" use case
            BookLinked result = new BookLinked
                (Mapper.Map<BookWithLink>(addedItem), addedItem.Id);

            return Created(uri, result);
        }

        // PUT: api/Books/5/setphoto
        /// <summary>
        /// Add a book cover photo to the book object
        /// </summary>
        /// <param name="id">Book identifier (int)</param>
        /// <param name="photo">Photo (jpg, png, etc.)</param>
        /// <returns>HTTP 204</returns>
        [Route("api/books/{id}/setphoto")]
        public IHttpActionResult PutPhoto(int id, [FromBody]byte[] photo)
        {
            // Get the Content-Type header from the request
            var contentType = Request.Content.Headers.ContentType.MediaType;

            // Attempt to save
            if (w.Books.SetPhoto(id, contentType, photo))
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
