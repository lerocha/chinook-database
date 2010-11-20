using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ChinookDatabase.DataModel
{
    [DebuggerDisplay("{Title} (AlbumId = {AlbumId})")]
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required, MaxLength(160)]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [RelatedTo(ForeignKey = "ArtistId")]
        public Artist Artist { get; set; }
    }
}