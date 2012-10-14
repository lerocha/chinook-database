using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ChinookDatabase.DataModel
{
    [DebuggerDisplay("PlaylistId = {PlaylistId}, TrackId = {TrackId}")]
    public class PlaylistTrack
    {
        [Key, Column(Order=1)]
        public int PlaylistId { get; set; }

        [Key, Column(Order = 2)]
        public int TrackId { get; set; }

        [ForeignKey("PlaylistId")]
        public Playlist Playlist { get; set; }

        [ForeignKey("TrackId")]
        public Track Track { get; set; }
    }
}
