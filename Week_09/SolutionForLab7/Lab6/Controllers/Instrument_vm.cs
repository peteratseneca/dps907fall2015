using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace Lab6.Controllers
{
    // Instrument resource model classes

    // Use case: Add musical instrument
    public class InstrumentAdd
    {
        public InstrumentAdd()
        {
            MSRP = 0;
        }

        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(100)]
        public string ManufacturerName { get; set; }

        [StringLength(1000)]
        public string InstrumentName { get; set; }

        [StringLength(100)]
        public string ModelCode { get; set; }

        [Range(0,UInt32.MaxValue)]
        public int MSRP { get; set; }
    }

    // Display only, no data annotations

    // Add new template
    public class InstrumentAddTemplate
    {
        public string Category { get { return "Musical instrument category, required, text, 100 character limit"; } }
        public string ManufacturerName { get { return "Manufacturer name, required, text, 100 character limit"; } }
        public string InstrumentName { get { return "Instrument name, required, text, 1000 character limit"; } }
        public string ModelCode { get { return "Model code number, required, text, 100 character limit"; } }
        public string MSRP { get { return "MSRP, required, number, limit of $4B"; } }
    }

    // Display only, no data annotations

    // Use case: Musical instrument data
    public class InstrumentBase : InstrumentAdd
    {
        public int Id { get; set; }
    }

    // Display only, no data annotations

    // Use case: Musical instrument data, with info about its associated media item

    public class InstrumentWithMediaInfo : InstrumentBase
    {
        public int PhotoMediaLength { get; set; }
        public string PhotoContentType { get; set; }
        public int SoundClipMediaLength { get; set; }
        public string SoundClipContentType { get; set; }
    }

    // Use case: For media item delivery

    public class InstrumentWithMedia : InstrumentWithMediaInfo
    {
        public byte[] PhotoMedia { get; set; }
        public byte[] SoundClipMedia { get; set; }
    }

    // ############################################################

    // Classes that have link relations

    public class InstrumentWithLink : InstrumentWithMediaInfo
    {
        public Link Link { get; set; }
    }

    public class InstrumentLinked : LinkedItem<InstrumentWithLink>
    {
        // Constructor - call the base class constructor

        // All use cases except "add new"
        public InstrumentLinked(InstrumentWithLink item) : base(item) { }

        // "Add new" use case
        public InstrumentLinked(InstrumentWithLink item, int id) : base(item, id) { }
    }

    public class InstrumentsLinked : LinkedCollection<InstrumentWithLink>
    {
        // Constructor - call the base class constructor
        public InstrumentsLinked(IEnumerable<InstrumentWithLink> collection) : base(collection)
        {
            Template = new InstrumentAddTemplate();
        }

        public InstrumentAddTemplate Template { get; set; }
    }

}
