using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AssociationsOther.Models;
using AutoMapper;
// Attention - Needed for model state validation
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Controllers
{
    public class Manager
    {
        // Facade reference
        ApplicationDbContext ds = new ApplicationDbContext();

        // ############################################################
        // Employees

        public IEnumerable<EmployeeBase> GetAllEmployees()
        {
            var fetchedObjects = ds.Employees
                .OrderBy(ln => ln.LastName)
                .ThenBy(fn => fn.FirstName);

            return Mapper.Map<IEnumerable<EmployeeBase>>(fetchedObjects);
        }

        // No associated data
        public EmployeeBase GetEmployeeById(int id)
        {
            var fetchedObject = ds.Employees.Find(id);

            return (fetchedObject == null) ? null : Mapper.Map<EmployeeBase>(fetchedObject);
        }

        // With self associated data (other Employee objects)
        public EmployeeBase2 GetEmployeeById2(int id)
        {
            var fetchedObject = ds.Employees
                .Include("ReportsToEmployee")
                .Include("EmployeesSupervised")
                .SingleOrDefault(e => e.Id == id);

            return (fetchedObject == null) ? null : Mapper.Map<EmployeeBase2>(fetchedObject);
        }

        // With all associated data
        public EmployeeBase3 GetEmployeeById3(int id)
        {
            var fetchedObject = ds.Employees
                .Include("ReportsToEmployee")
                .Include("EmployeesSupervised")
                .Include("HomeAddress")
                .Include("WorkAddress")
                .Include("JobDuties")
                .SingleOrDefault(e => e.Id == id);

            return (fetchedObject == null) ? null : Mapper.Map<EmployeeBase3>(fetchedObject);
        }

        public EmployeeBase AddEmployee(EmployeeAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Add the new object
                Employee addedItem = Mapper.Map<Employee>(newItem);

                ds.Employees.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return Mapper.Map<EmployeeBase>(addedItem);
            }
        }

        public EmployeeBase EditEmployeeNames(EmployeeEditNames editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            var storedItem = ds.Employees.Find(editedItem.Id);

            if (storedItem == null)
            {
                return null;
            }
            else
            {
                // Fetch the object from the data store - ds.Entry(storedItem)
                // Get its current values collection - .CurrentValues
                // Set those to the edited values - .SetValues(editedItem)
                ds.Entry(storedItem).CurrentValues.SetValues(editedItem);
                // The SetValues() method ignores missing properties and navigation properties
                ds.SaveChanges();

                return Mapper.Map<EmployeeBase>(storedItem);
            }
        }

        public void DeleteEmployee(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Employees.Find(id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // If this fails, throw an exception (as above)
                    // This implementation just prevents an error from bubbling up

                    ds.Employees.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        // ############################################################
        // These are COMMANDS
        // They affect an employee object
        // Notice that they are idempotent 

        public void SetEmployeeSupervisor(EmployeeSupervisor item)
        {
            // Get a reference to the employee
            var employee = ds.Employees.Find(item.Employee);
            if (employee == null) { return; }

            // Get a reference to the supervisor
            var supervisor = ds.Employees.Find(item.Supervisor);
            if (supervisor == null) { return; }

            // Make the changes, save, and exit
            employee.ReportsToEmployee = supervisor;
            employee.ReportsToEmployeeId = supervisor.Id;
            ds.SaveChanges();
        }

        public void ClearEmployeeSupervisor(EmployeeSupervisor item)
        {
            // Get a reference to the employee
            var employee = ds.Employees.Find(item.Employee);
            if (employee == null) { return; }

            // Get a reference to the supervisor
            var supervisor = ds.Employees.Find(item.Supervisor);
            if (supervisor == null) { return; }

            // Make the changes, save, and exit
            if (employee.ReportsToEmployeeId == supervisor.Id)
            {
                employee.ReportsToEmployee = null;
                employee.ReportsToEmployeeId = null;
                ds.SaveChanges();
            }
        }

        public void SetEmployeeJobDuty(EmployeeJobDuty item)
        {
            // Get a reference to the employee
            // Must bring back its collection of job duties
            var employee = ds.Employees
                .Include("JobDuties")
                .SingleOrDefault(e => e.Id == item.Employee);
            if (employee == null) { return; }

            // Get a reference to the job duty
            var jobDuty = ds.JobDuties.Find(item.JobDuty);
            if (jobDuty == null) { return; }

            // Make the changes, save, and exit
            employee.JobDuties.Add(jobDuty);
            ds.SaveChanges();
        }

        public void ClearEmployeeJobDuty(EmployeeJobDuty item)
        {
            // Get a reference to the employee
            // Must bring back its collection of job duties
            var employee = ds.Employees
                .Include("JobDuties")
                .SingleOrDefault(e => e.Id == item.Employee);
            if (employee == null) { return; }

            // Get a reference to the job duty
            // Notice that we're getting it from the employee object (above)
            var jobDuty = employee.JobDuties
                .SingleOrDefault(j => j.Id == item.JobDuty);
            if (jobDuty == null) { return; }

            // Make the changes, save, and exit
            if (employee.JobDuties.Remove(jobDuty))
            {
                ds.SaveChanges();
            }
        }

        // ############################################################
        // Addresses

        public IEnumerable<AddressBase> GetAllAddresses()
        {
            var fetchedObjects = ds.Addresses.OrderBy(p => p.PostalCode);

            return Mapper.Map<IEnumerable<AddressBase>>(fetchedObjects);
        }

        // Include associated data
        public AddressFull GetAddressById(int id)
        {
            var fetchedObject = ds.Addresses
                .Include("Employee")
                .SingleOrDefault(a => a.Id == id);

            return (fetchedObject == null) ? null : Mapper.Map<AddressFull>(fetchedObject);
        }

        public AddressBase AddAddress(AddressAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Must validate "Home" or "Work" address type
                var isAddressTypeValid =
                    (newItem.AddressType.Trim().ToLower() == "home" || newItem.AddressType.Trim().ToLower() == "work")
                    ? true
                    : false;
                if (!isAddressTypeValid) { return null; }

                // Must validate the associated object
                var associatedItem = ds.Employees.Find(newItem.EmployeeId);
                if (associatedItem == null)
                {
                    return null;
                }

                // Add the new object

                // Build the Address object
                Address addedItem = Mapper.Map<Address>(newItem);

                // Set its associated item identifier
                addedItem.EmployeeId = associatedItem.Id;

                // Now, look at this next task from the perspective of the employee object
                // Set the appropriate address object
                if (newItem.AddressType.Trim().ToLower() == "home")
                {
                    associatedItem.HomeAddress = addedItem;
                }
                else
                {
                    associatedItem.WorkAddress = addedItem;
                }

                ds.Addresses.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return Mapper.Map<AddressBase>(addedItem);
            }
        }

        public void DeleteAddress(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Addresses.Find(id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // Remove the reference to the address to be deleted
                    // Must include the associated objects, so that the
                    // changes take place on both ends of the association
                    var associatedItem = ds.Employees
                        .Include("HomeAddress")
                        .Include("WorkAddress")
                        .SingleOrDefault(e => e.Id == storedItem.EmployeeId);

                    // If this fails, throw an exception (as above)
                    // This implementation just prevents an error from bubbling up

                    storedItem.Employee = null;
                    storedItem.EmployeeId = null;
                    ds.Addresses.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        public AddressBase EditAddress(AddressEdit editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            // Must include the required associated object,
            // so that the SetValues() and SaveChanges() methods will work
            var storedItem = ds.Addresses
                .Include("Employee")
                .SingleOrDefault(e => e.Id == editedItem.Id);

            if (storedItem == null)
            {
                return null;
            }
            else
            {
                // Fetch the object from the data store - ds.Entry(storedItem)
                // Get its current values collection - .CurrentValues
                // Set those to the edited values - .SetValues(editedItem)
                ds.Entry(storedItem).CurrentValues.SetValues(editedItem);
                // The SetValues() method ignores missing properties and navigation properties
                ds.SaveChanges();

                return Mapper.Map<AddressBase>(storedItem);
            }
        }

        // ############################################################
        // JobDuties

        public IEnumerable<JobDutyBase> GetAllJobDuties()
        {
            var fetchedObjects = ds.JobDuties.OrderBy(n => n.Name);

            return Mapper.Map<IEnumerable<JobDutyBase>>(fetchedObjects);
        }

        // Include associated data
        public IEnumerable<JobDutyFull> GetAllJobDutiesWithEmployees()
        {
            var fetchedObjects = ds.JobDuties
                .Include("Employees")
                .OrderBy(n => n.Name);

            return Mapper.Map<IEnumerable<JobDutyFull>>(fetchedObjects);
        }

        // Include associated data
        public JobDutyFull GetJobDutyById(int id)
        {
            var fetchedObject = ds.JobDuties
                .Include("Employees")
                .SingleOrDefault(j => j.Id == id);

            return (fetchedObject == null) ? null : Mapper.Map<JobDutyFull>(fetchedObject);
        }

        public JobDutyBase AddJobDuty(JobDutyAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Add the new object
                JobDuty addedItem = Mapper.Map<JobDuty>(newItem);

                ds.JobDuties.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return Mapper.Map<JobDutyBase>(addedItem);
            }
        }

        public void DeleteJobDuty(int id)
        {
            // Attempt to fetch the existing item
            // Must include the associated employees
            var storedItem = ds.JobDuties
                .Include("Employees")
                .SingleOrDefault(j => j.Id == id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // Allow delete only if it is not used by any employees
                    if (storedItem.Employees.Count == 0)
                    {
                        ds.JobDuties.Remove(storedItem);
                        ds.SaveChanges();
                    }
                }
                catch (Exception) { }
            }
        }

        public JobDutyBase EditJobDuty(JobDutyBase editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            var storedItem = ds.JobDuties.Find(editedItem.Id);

            if (storedItem == null)
            {
                return null;
            }
            else
            {
                // Fetch the object from the data store - ds.Entry(storedItem)
                // Get its current values collection - .CurrentValues
                // Set those to the edited values - .SetValues(editedItem)
                ds.Entry(storedItem).CurrentValues.SetValues(editedItem);
                // The SetValues() method ignores missing properties and navigation properties
                ds.SaveChanges();

                return Mapper.Map<JobDutyBase>(storedItem);
            }
        }

        // ############################################################
        // Validator

        // Attention - Model state validation
        public bool ModelStateIsValid(object item)
        {
            // WCF Service does not have built-in model validation
            // Therefore, we will the feature from the more modern Web API framework
            // We construct our own validator below
            // Inspiration from Scott Allen...
            // http://odetocode.com/blogs/scott/archive/2011/06/29/manual-validation-with-data-annotations.aspx

            // Create a validation context
            // It knows the data type of the passed-in item, and can validate it
            var context = new ValidationContext(item, serviceProvider: null, items: null);

            // Create a results container
            var results = new List<ValidationResult>();

            // Validate
            return Validator.TryValidateObject(item, context, results);
        }

    }

}
