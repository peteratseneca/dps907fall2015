using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace MediaUpload.Models
{
    // Attention - Book design model class

    /// <summary>
    /// Book object, saved in the persistent store
    /// </summary>
    public class Book
    {
        public Book()
        {
            // Set a default non-null value
            PublishedDate = DateTime.Now.AddYears(-1);
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Author { get; set; }

        [Required, StringLength(13)]
        public string ISBN13 { get; set; }

        public int Pages { get; set; }
        public DateTime PublishedDate { get; set; }

        [Required, StringLength(50)]
        public string Format { get; set; }

        public byte[] Photo { get; set; }
        public string ContentType { get; set; }
    }

    // ExceptionInfo design model class

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
