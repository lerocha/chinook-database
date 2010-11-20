using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ChinookDatabase.DataModel
{
    [DebuggerDisplay("{Name} (TrackId = {TrackId})")]
    public class Track
    {
        [Key]
        public int TrackId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public int AlbumId { get; set; }

        public int MediaTypeId { get; set; }

        public int GenreId { get; set; }

        [MaxLength(220)]
        public string Composer { get; set; }

        public int Miliseconds { get; set; }

        public int Bytes { get; set; }

        public decimal UnitPrice { get; set; }

        [RelatedTo(ForeignKey = "AlbumId")]
        public Album Album { get; set; }

        [RelatedTo(ForeignKey = "MediaTypeId")]
        public MediaType MediaType { get; set; }

        [RelatedTo(ForeignKey = "GenreId")]
        public Genre Genre { get; set; }
    }
}
