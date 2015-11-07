using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// new...
using LocalSecurity.ServiceLayer;
using AutoMapper;

namespace LocalSecurity.Controllers
{
    // The purpose of this class is to fetch information about errors

    public class ExceptionsController : ApiController
    {
        // Reference to the worker object
        private Worker w;
        private Manager m;

        public ExceptionsController()
        {
            w = new Worker();
            m = new Manager(w);
        }

        // GET: api/Exceptions
        /// <summary>
        /// Information for all exceptions, sorted by timestamp in descending order
        /// </summary>
        /// <returns>Collection of ExceptionInfo objects</returns>
        public IHttpActionResult Get()
        {
            // Get all
            var fetchedObjects = w.Exceptions.GetAll();

            ExceptionInfosLinked result = new ExceptionInfosLinked
                (Mapper.Map<IEnumerable<ExceptionInfoWithLink>>(fetchedObjects));

            return Ok(result);
        }

        // GET: api/Exceptions/5
        /// <summary>
        /// Information for one exception
        /// </summary>
        /// <param name="id">Exception object identifier</param>
        /// <returns>ExceptionInfo object</returns>
        public IHttpActionResult Get(int? id)
        {
            // Fetch the object
            var fetchedObject = w.Exceptions.GetById(id.GetValueOrDefault());

            // Continue?
            if (fetchedObject == null) { return NotFound(); }

            ExceptionInfoLinked result = new ExceptionInfoLinked
                (Mapper.Map<ExceptionInfoWithLink>(fetchedObject));

            return Ok(result);
        }

        /*
        // POST: api/Exceptions
        public void Post([FromBody]string value) { }

        // PUT: api/Exceptions/5
        public void Put(int id, [FromBody]string value) { }

        // DELETE: api/Exceptions/5
        public void Delete(int id) { }
        */

    }

}
