using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AssociationsOther.Controllers;

namespace AssociationsOther
{
    // Attention - This class implements the IEmployeeService interface
    public class EmployeeService : IEmployeeService
    {
        // Manager reference
        Manager m = new Manager();

        public string HelloWorld()
        {
            return "Hello, world!";
        }

        // Get all
        public IEnumerable<EmployeeBase> AllEmployees()
        {
            return m.GetAllEmployees();
        }

        // Get one by its identifier
        public EmployeeBase EmployeeById(int? id)
        {
            // Determine whether we can continue
            if (!id.HasValue) { return null; }

            // Fetch the object, so that we can inspect its value
            var fetchedObject = m.GetEmployeeById(id.Value);

            return (fetchedObject == null) ? null : fetchedObject;
        }

        // Add new
        public EmployeeBase AddEmployee(EmployeeAdd newItem)
        {
            // Attention - Call the model state validator in the manager class
            if (m.ModelStateIsValid(newItem))
            {
                var addedItem = m.AddEmployee(newItem);
                return (addedItem == null) ? null : addedItem;
            }
            else
            {
                return null;
            }
        }

    }

}
