using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Controllers
{
    public class JobDutyAdd
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }
    }

    // Can use this for the "edit" function too
    public class JobDutyBase : JobDutyAdd
    {
        public int Id { get; set; }
    }

    public class JobDutyFull : JobDutyBase
    {
        public JobDutyFull()
        {
            Employees = new List<EmployeeBase>();
        }

        public IEnumerable<EmployeeBase> Employees { get; set; }
    }

}