using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using Lab3.Models;
using Lab3.Controllers;
using AutoMapper;

namespace Lab3.ServiceLayer
{
    public class Artist_repo : Repository<Artist>
    {
        // Constructor
        public Artist_repo(ApplicationDbContext ds) : base(ds) { }

        public IEnumerable<ArtistBase> GetAll()
        {
            // Call the base class method
            var fetchedObjects = RGetAll(null);

            // Notice that we can do the sort here if we want to
            return Mapper.Map<IEnumerable<ArtistBase>>(fetchedObjects.OrderBy(a => a.ArtistName));
        }

        public IEnumerable<ArtistSelf> GetAllWithMembers()
        {
            // Call the base class method, ask for associated data
            var fetchedObjects = RGetAll(new[] { "Members" });

            return Mapper.Map<IEnumerable<ArtistSelf>>(fetchedObjects);
        }

        public ArtistBase GetById(int id)
        {
            // Call the base class method
            var fetchedObject = RGetById(i => i.Id == id, null);

            return (fetchedObject == null) ? null : Mapper.Map<ArtistBase>(fetchedObject);
        }

        public ArtistSelf GetByIdWithMembers(int id)
        {
            // Call the base class method, ask for associated data
            var fetchedObject = RGetById(i => i.Id == id, new[] { "MemberOfArtist", "Members" });

            return (fetchedObject == null) ? null : Mapper.Map<ArtistSelf>(fetchedObject);
        }

        public ArtistBase Add(ArtistAdd newItem)
        {
            // Add the new object
            var addedItem = RAdd(Mapper.Map<Artist>(newItem));

            // Return the object
            return Mapper.Map<ArtistBase>(addedItem);
        }

        // This one method services both PUT use cases
        // Both controller methods call this
        public void SetMemberGroup(ArtistMemberGroup item)
        {
            // Get a reference to the "member" artist
            var member = _ds.Artists.Find(item.Member);
            if (member == null) { return; }

            // Get a reference to the "group" artist
            var group = _ds.Artists.Find(item.Group);
            if (group == null) { return; }

            // Make the changes, save, and exit
            member.MemberOfArtist = group;
            member.MemberOfArtistId = group.Id;
            _ds.SaveChanges();
        }

    }

}
