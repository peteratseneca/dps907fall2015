using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using System.ComponentModel.DataAnnotations;

namespace Lab3.Models
{
    public class Artist
    {
        public Artist()
        {
            BirthOrStartDate = DateTime.Now.AddYears(-20);
            Members = new List<Artist>();
            Albums = new List<Album>();
            Songs = new List<Song>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string ArtistName { get; set; }

        [Required, StringLength(50)]
        public string ArtistType { get; set; }
        public DateTime BirthOrStartDate { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        // Self-referencing to-one
        // This individual artist could be a member of a group
        public int? MemberOfArtistId { get; set; }
        public Artist MemberOfArtist { get; set; }

        // Self-referencing to-many
        // This group artist could have members
        public ICollection<Artist> Members { get; set; }

        // To-many, to other entities
        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; }
    }

    public class Album
    {
        public Album()
        {
            DateReleased = DateTime.Now.AddMonths(1);
            Songs = new List<Song>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string AlbumName { get; set; }
        public DateTime DateReleased { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        // Required to-one, back to Artist entity (same idea with Song, below)
        [Required]
        public Artist Artist { get; set; }

        // To-many, with songs
        public ICollection<Song> Songs { get; set; }
    }

    public class Song
    {
        public Song()
        {
            DateWritten = DateTime.Now.AddMonths(-6);
            Albums = new List<Album>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string SongName { get; set; }
        public DateTime DateWritten { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        [Required]
        public Artist Artist { get; set; }

        // To-many, with albums
        public ICollection<Album> Albums { get; set; }
    }

}
