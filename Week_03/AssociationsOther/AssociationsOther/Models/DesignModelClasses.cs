using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Models
{
    public class Employee
    {
        public Employee()
        {
            this.EmployeesSupervised = new List<Employee>();
            this.JobDuties = new List<JobDuty>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        // One-to-one, to the same entity
        // Include an int property to hold the identifier of the pointed-to object
        // It must be nullable, because it is optional (in most situations)
        public int? ReportsToEmployeeId { get; set; }
        // Next, include a nav property to this class 
        public Employee ReportsToEmployee { get; set; }

        // One-to-many, to the same entity
        // An employee who is a supervisor has a collection of employees
        // who report to the supervisor
        public ICollection<Employee> EmployeesSupervised { get; set; }

        // An employee can OPTIONALLY have address properties
        // This is the "principal" end of the Employee-Address association
        public Address HomeAddress { get; set; }
        public Address WorkAddress { get; set; }

        public ICollection<JobDuty> JobDuties { get; set; }
    }

    // Rules...
    // An employee can OPTIONALLY have address properties (principal)
    // An address MUST be associated with an employee (dependent)

    public class Address
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required, StringLength(100)]
        public string CityAndProvince { get; set; }
        [Required, StringLength(20)]
        public string PostalCode { get; set; }

        public int? EmployeeId { get; set; }
        // An address MUST be associated with an employee (dependent)
        // This is the "dependent" end of the Employee-Address association
        [Required]
        public Employee Employee { get; set; }
    }

    public class JobDuty
    {
        public JobDuty()
        {
            this.Employees = new List<Employee>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(1000)]
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
