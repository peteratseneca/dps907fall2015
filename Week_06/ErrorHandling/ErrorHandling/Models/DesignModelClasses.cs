using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace ErrorHandling.Models
{
    // Attention - ExceptionInfo design model class

    /// <summary>
    /// Exception information object, saved in the persistent store
    /// </summary>
    public class ExceptionInfo
    {
        public ExceptionInfo()
        {
            DateAndTime = DateTime.Now;
        }

        public int Id { get; set; }
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

}
