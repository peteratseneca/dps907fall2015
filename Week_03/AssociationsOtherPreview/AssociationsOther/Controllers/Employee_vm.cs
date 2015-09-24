using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Controllers
{
	public class EmployeeAdd
	{
        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }
    }

    public class EmployeeBase : EmployeeAdd
    {
        public int Id { get; set; }
    }

}