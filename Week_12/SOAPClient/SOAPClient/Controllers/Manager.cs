using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using SOAPClient.SenecaES;

namespace SOAPClient.Controllers
{
    // Attention - Manager class, as you have done in the past
    public class Manager
    {
        // Attention - Reference to the web service proxy class, which was created with "Add Service Reference"
        EmployeeServiceClient es = new EmployeeServiceClient();

        // All employees
        public IEnumerable<EmployeeBase> AllEmployees()
        {
            // Attention - Call the web service method
            var fetchedObjects = es.AllEmployees();

            if (fetchedObjects == null)
            {
                return new List<EmployeeBase>();
            }
            else
            {
                return fetchedObjects;
            }
        }

        // Employee by identifier
        public EmployeeBase EmployeeById(int id)
        {
            // Call the web service method
            var fetchedObject = es.EmployeeById(id);

            return (fetchedObject == null) ? null : fetchedObject;
        }

        public EmployeeBase AddEmployee(EmployeeAdd newItem)
        {
            // Call the web service method
            var addedItem = es.AddEmployee(newItem);

            return (addedItem == null) ? null : addedItem;
        }

    }

}
