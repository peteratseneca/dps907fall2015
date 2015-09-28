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
            var fetchedObjects = RGetAll();

            // Notice that we can do the sort here
            return Mapper.Map<IEnumerable<ArtistBase>>(fetchedObjects.OrderBy(a => a.ArtistName));
        }

        public IEnumerable<ArtistSelf> GetAllWithMembers()
        {
            // Use the data context directly for this custom logic
            var fetchedObjects = _ds.Artists
                .Include("Members")
                .OrderBy(an => an.ArtistName);

            return Mapper.Map<IEnumerable<ArtistSelf>>(fetchedObjects);
        }

        public ArtistSelf GetById(int id)
        {
            // Use the data context directly for this custom logic
            var fetchedObject = _ds.Artists
                .Include("MemberOfArtist")
                .Include("Members")
                .SingleOrDefault(a => a.Id == id);

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
