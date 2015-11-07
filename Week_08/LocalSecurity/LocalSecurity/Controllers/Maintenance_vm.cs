using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace LocalSecurity.Controllers
{
    public class UserAdd
    {
        public UserAdd()
        {
            Roles = new List<string>();
        }

        [Required, StringLength(256)]
        public string Email { get; set; }

        [Required, StringLength(128)]
        public string Surname { get; set; }

        [Required, StringLength(128)]
        public string GivenName { get; set; }

        public ICollection<string> Roles { get; set; }
    }

    public class UserDelete
    {
        [Required]
        public string UserName { get; set; }
    }

}
