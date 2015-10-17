using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using MediaUpload.Models;
using MediaUpload.Controllers;
using AutoMapper;

namespace MediaUpload.ServiceLayer
{
    // Book entity-specific repository

    public class Book_repo : Repository<Book>
    {
        public Book_repo(ApplicationDbContext _ds) : base(_ds) { }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>Collection of BookBase objects</returns>
        public IEnumerable<BookWithMediaInfo> GetAll()
        {
            // Call the base class method
            var fetchedObjects = RGetAll(null);

            return Mapper.Map<IEnumerable<BookWithMediaInfo>>(fetchedObjects);
        }

        /// <summary>
        /// Get book by its identifier
        /// </summary>
        /// <param name="id">Book identifier, int</param>
        /// <returns>BookWithMedia object</returns>
        public BookWithMedia GetById(int id)
        {
            // Call the base class method
            var fetchedObject = RGetById(i => i.Id == id, null);

            return (fetchedObject == null) ? null : Mapper.Map<BookWithMedia>(fetchedObject);
        }

        /// <summary>
        /// Add book
        /// </summary>
        /// <param name="newItem">BookAdd object</param>
        /// <returns>BookBase object</returns>
        public BookWithMediaInfo Add(BookAdd newItem)
        {
            // Add the new object
            var addedItem = RAdd(Mapper.Map<Book>(newItem));

            // Return the object
            return Mapper.Map<BookWithMediaInfo>(addedItem);
        }

        /// <summary>
        /// Set a book's cover photo
        /// </summary>
        /// <param name="id">Book identifier (int)</param>
        /// <param name="contentType">Content-Type value</param>
        /// <param name="photo">Photo bytes</param>
        /// <returns>Success: True or False</returns>
        public bool SetPhoto(int id, string contentType, byte[] photo)
        {
            // Ensure that we can continue
            if (string.IsNullOrEmpty(contentType) | photo == null)
            {
                return false;
            }

            // Attempt to find the matching object
            var storedItem = _dbset.Find(id);

            // Ensure that we can continue
            if (storedItem == null)
            {
                return false;
            }

            // Save the photo
            storedItem.ContentType = contentType;
            storedItem.Photo = photo;

            // Attempt to save changes
            return (SaveChanges() > 0) ? true : false;
        }

    }

}
