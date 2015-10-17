using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace MediaUpload.Controllers
{
    // Books resource model classes

    /// <summary>
    /// Use case: Add book
    /// </summary>
    public class BookAdd
    {
        public BookAdd()
        {
            PublishedDate = DateTime.Now.AddYears(-1);
        }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Author { get; set; }

        [Required, StringLength(13)]
        public string ISBN13 { get; set; }

        [Range(1,UInt16.MaxValue)]
        public int Pages { get; set; }
        public DateTime PublishedDate { get; set; }

        [Required, StringLength(50)]
        public string Format { get; set; }
    }

    // Display only, no data annotations

    /// <summary>
    /// Use case: Tells the user about the shape of a new object
    /// </summary>
    public class BookAddTemplate
    {
        public string Title { get { return "Book title, required, text, 100 character limit"; } }
        public string Author { get { return "Author, required, text, 100 character limit"; } }
        public string ISBN13 { get { return "ISBN-13 number, required, numbers only (no separators), 13 digit limit"; } }
        public string Pages { get { return "Number of pages, required, numbers only, ranges from 1 to 65,536"; } }
        public string PublishedDate { get { return "Date published, required, ISO 8601 text format (e.g. 2015-10-12T08:00"; } }
        public string Format { get { return "Format (hardcover, paperback, etc.), required, text, 50 character limit"; } }
    }

    // Display only, no data annotations
    /// <summary>
    /// Use case: Book data
    /// </summary>
    public class BookBase : BookAdd
    {
        public int Id { get; set; }
    }

    // Display only, no data annotations
    /// <summary>
    /// Use case: Book data, with info about its associated media item
    /// </summary>
    public class BookWithMediaInfo : BookBase
    {
        public int PhotoLength { get; set; }
        public string ContentType { get; set; }
    }

    public class BookWithMedia : BookWithMediaInfo
    {
        public byte[] Photo { get; set; }
    }

    // ############################################################
    // Classes that have link relations

    public class BookWithLink : BookWithMediaInfo
    {
        public Link Link { get; set; }
    }

    public class BookLinked : LinkedItem<BookWithLink>
    {
        // Constructor - call the base class constructor

        // All use cases except "add new"
        public BookLinked(BookWithLink item) : base(item) { }

        // "Add new" use case
        public BookLinked(BookWithLink item, int id) : base(item, id) { }
    }

    public class BooksLinked : LinkedCollection<BookWithLink>
    {
        // Constructor - call the base class constructor
        public BooksLinked(IEnumerable<BookWithLink> collection) : base(collection)
        {
            Template = new BookAddTemplate();
        }

        public BookAddTemplate Template { get; set; }
    }

}
