using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;
// for JsonConstructor
using Newtonsoft.Json;

namespace ClientAppInstruments.Controllers
{
    // Instrument resource model classes
    // These were copied from the Instruments web service
    // They have been modified with data annotations for web apps

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
        [Display(Name = "Manufacturer Name")]
        public string ManufacturerName { get; set; }

        [StringLength(1000)]
        [Display(Name = "Instrument Name")]
        public string InstrumentName { get; set; }

        [StringLength(100)]
        [Display(Name = "Model Code")]
        public string ModelCode { get; set; }

        [Range(0, UInt32.MaxValue)]
        public int MSRP { get; set; }

        // Attention - Property to hold the photo upload
        // Must have the JsonIgnore attribute so that the photo bytes will not be serialized
        // We could do this another way, and maybe we should (this feels hacky)
        [JsonIgnore]
        public HttpPostedFileBase PhotoUpload { get; set; }
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

    public class InstrumentEditPhoto
    {
        public InstrumentEditPhoto() { }
        public InstrumentEditPhoto(int id, string description, int mediaSize)
        {
            Id = id;
            Description = description;
            MediaSize = mediaSize;
        }

        public int Id { get; set; }

        public string Description { get; set; }
        public int MediaSize { get; set; }

        // Attention - Property to hold the photo upload
        // Must have the JsonIgnore attribute so that the photo bytes will not be serialized
        // We could do this another way, and maybe we should (this feels hacky)
        [JsonIgnore]
        public HttpPostedFileBase PhotoUpload { get; set; }
    }

    public class InstrumentEditSoundClip
    {
        public InstrumentEditSoundClip() { }
        public InstrumentEditSoundClip(int id, string description, int mediaSize)
        {
            Id = id;
            Description = description;
            MediaSize = mediaSize;
        }

        public int Id { get; set; }

        public string Description { get; set; }
        public int MediaSize { get; set; }

        // Attention - Property to hold the sound clip upload
        // Must have the JsonIgnore attribute so that the photo bytes will not be serialized
        // We could do this another way, and maybe we should (this feels hacky)
        [JsonIgnore]
        public HttpPostedFileBase SoundClipUpload { get; set; }
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

    public class InstrumentAppCredentials
    {
        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }
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
        // Remove the attribute to see the error message
        // (two constructors, deserializer can't figure out which one to use)
        [JsonConstructor]
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
