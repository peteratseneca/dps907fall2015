using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// new...
using MediaUpload.ServiceLayer;

namespace MediaUpload.Controllers
{
    // The purpose of this controller is to enable the user to create an error condition
    // Each will cause the error to be logged, and some info will be returned to the requestor

    public class CreateErrorController : ApiController
    {
        // Reference to the worker object
        private Worker w;
        private Manager m;

        public CreateErrorController()
        {
            w = new Worker();
            m = new Manager(w);
        }

        // GET: api/CreateError
        /// <summary>
        /// Information on how to create a request that generates an exception
        /// </summary>
        /// <returns>Collection of strings</returns>
        public IEnumerable<string> Get()
        {
            return new string[]
            {
                "These URIs will throw an exception:",
                "api/createerror/dividebyzero",
                "api/createerror/nullreference",
                "api/createerror/other"
            };
        }

        /// <summary>
        /// Generates an exception: Divide by zero
        /// </summary>
        /// <returns>Brief information about the exception</returns>
        [Route("api/CreateError/DivideByZero")]
        public string GetDivideByZero()
        {
            // Divide by zero
            int i = 0;
            double j = 123 / i;

            return j.ToString();
        }

        /// <summary>
        /// Generates an exception: Null reference
        /// </summary>
        /// <returns>Brief information about the exception</returns>
        [Route("api/CreateError/NullReference")]
        public string GetNullReference()
        {
            // Null reference
            // Create a reference to an object, but do not initialize it
            ExceptionInfoAdd ex = null;

            return ex.Message;
        }

        /// <summary>
        /// Generates an exception: Other
        /// </summary>
        /// <returns>Brief information about the exception</returns>
        [Route("api/CreateError/Other")]
        public string GetOther()
        {
            // Create a general error
            throw new Exception();
        }

    }

}
