using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ChinookDatabase.DataModel
{
    [DebuggerDisplay("PlaylistId = {PlaylistId}, TrackId = {TrackId}")]
    public class PlaylistTrack
    {
        [Key, DataMember(Order=1)]
        public int PlaylistId { get; set; }

        [Key, DataMember(Order = 2)]
        public int TrackId { get; set; }

        [RelatedTo(ForeignKey = "PlaylistId")]
        public Playlist Playlist { get; set; }

        [RelatedTo(ForeignKey = "TrackId")]
        public Track Track { get; set; }
    }
}
