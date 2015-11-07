using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using LocalSecurity.Models;
using LocalSecurity.Controllers;
using AutoMapper;

namespace LocalSecurity.ServiceLayer
{
    public class ExceptionInfo_repo : Repository<ExceptionInfo>
    {
        // Constructor
        public ExceptionInfo_repo(ApplicationDbContext ds) : base(ds) { }

        public IEnumerable<ExceptionInfoBase> GetAll()
        {
            // Call the base class method
            var fetchedObjects = RGetAll(null);

            // Notice that we can do the sort here if we want to
            return Mapper.Map<IEnumerable<ExceptionInfoBase>>(fetchedObjects.OrderByDescending(e => e.DateAndTime));
        }

        public ExceptionInfoBase GetById(int id)
        {
            // Call the base class method
            var fetchedObject = RGetById(i => i.Id == id, null);

            return (fetchedObject == null) ? null : Mapper.Map<ExceptionInfoBase>(fetchedObject);
        }

        public ExceptionInfoBase Add(ExceptionInfoAdd newItem)
        {
            // Add the new object
            var addedItem = RAdd(Mapper.Map<ExceptionInfo>(newItem));

            // Return the object
            return Mapper.Map<ExceptionInfoBase>(addedItem);
        }

    }

}
