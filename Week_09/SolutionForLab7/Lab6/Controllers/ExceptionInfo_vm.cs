using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace Lab6.Controllers
{
    // Resource model classes for the error info objects

    public class ExceptionInfoAdd
    {
        public ExceptionInfoAdd()
        {
            DateAndTime = DateTime.Now;
        }

        public DateTime DateAndTime { get; set; }

        [StringLength(1000)]
        public string UserName { get; set; }

        [StringLength(5000)]
        public string Message { get; set; }

        [StringLength(1000)]
        public string Source { get; set; }

        [StringLength(1000)]
        public string Method { get; set; }

        public string StackTrace { get; set; }
    }

    public class ExceptionInfoAddTemplate
    {
        public string DateAndTime { get { return "Date and time, required, timestamp"; } }
        public string UserName { get { return "User name, required, string, up to 1000 characters"; } }
        public string Message { get { return "Error message, required, string, up to 5000 characters"; } }
        public string Source { get { return "Error source module, required, string, up to 1000 characters"; } }
        public string Method { get { return "Method name, required, string, up to 1000 characters"; } }
        public string StackTrace { get { return "Stack trace, optional"; } }
    }

    public class ExceptionInfoBase : ExceptionInfoAdd
    {
        public int Id { get; set; }
    }

    // ############################################################
    // Classes that have link relations

    public class ExceptionInfoWithLink : ExceptionInfoBase
    {
        public Link Link { get; set; }
    }

    public class ExceptionInfoLinked : LinkedItem<ExceptionInfoWithLink>
    {
        // Constructor - call the base class constructor

        // All use cases except "add new"
        public ExceptionInfoLinked(ExceptionInfoWithLink item) : base(item) { }

        // "Add new" use case
        public ExceptionInfoLinked(ExceptionInfoWithLink item, int id) : base(item, id) { }
    }

    public class ExceptionInfosLinked : LinkedCollection<ExceptionInfoWithLink>
    {
        // Constructor - call the base class constructor
        public ExceptionInfosLinked(IEnumerable<ExceptionInfoWithLink> collection) : base(collection)
        {
            Template = new ExceptionInfoAddTemplate();
        }

        public ExceptionInfoAddTemplate Template { get; set; }
    }

}
