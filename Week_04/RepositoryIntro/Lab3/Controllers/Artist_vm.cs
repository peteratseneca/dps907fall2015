using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace Lab3.Controllers
{
    public class ArtistAdd
    {
        public ArtistAdd()
        {
            // Best practice, constructor for dates and collections, data annotations
            BirthOrStartDate = DateTime.Now.AddYears(-20);
        }

        [Required, StringLength(100)]
        public string ArtistName { get; set; }

        [Required, StringLength(50)]
        public string ArtistType { get; set; }
        public DateTime BirthOrStartDate { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }
    }

    public class ArtistBase : ArtistAdd
    {
        public int Id { get; set; }
    }

    // Resource model with all associated objects
    public class ArtistSelf : ArtistBase
    {
        public int? MemberOfArtistId { get; set; }
        public ArtistBase MemberOfArtist { get; set; }

        public ICollection<ArtistBase> Members { get; set; }
    }

    // Resource model to handle artist member/group associations
    public class ArtistMemberGroup
    {
        public int Member { get; set; }
        public int Group { get; set; }
    }

}
