using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AssociationsOther.Models;
using AutoMapper;

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

        public EmployeeBase GetEmployeeById(int id)
        {
            var fetchedObject = ds.Employees.Find(id);

            return (fetchedObject == null) ? null : Mapper.Map<EmployeeBase>(fetchedObject);
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

        // Others...
        // Edit employee
        // Delete employee
        // Employee with associated data
        
        // ############################################################
        // JobDuties


        // ############################################################
        // Addresses

    }

}
