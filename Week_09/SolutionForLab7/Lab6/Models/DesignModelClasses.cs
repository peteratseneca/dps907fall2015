using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace Lab6.Models
{
    // Musical instrument design model class

    public class Instrument
    {
        public Instrument()
        {
            MSRP = 0;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(100)]
        public string ManufacturerName { get; set; }

        [StringLength(1000)]
        public string InstrumentName { get; set; }

        [StringLength(100)]
        public string ModelCode { get; set; }

        public int MSRP { get; set; }

        public byte[] PhotoMedia { get; set; }
        public string PhotoContentType { get; set; }

        // BSD requirement
        public byte[] SoundClipMedia { get; set; }
        public string SoundClipContentType { get; set; }

        // Attention - New property for the resource owner
        [Required]
        public string UserName { get; set; }
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
