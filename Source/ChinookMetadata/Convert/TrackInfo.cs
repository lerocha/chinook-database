namespace ChinookMetadata.Convert
{
    /// <summary>
    /// Class that represents iTunes track info.
    /// </summary>
    internal class TrackInfo
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public int MediaTypeId { get; set; }
        public string MediaTypeName { get; set; }
        public int OriginalTrackId { get; set; }
        public string Name { get; set; }
        public string Composer { get; set; }
        public int Time { get; set; }
        public int Size { get; set; }
        public decimal UnitPrice { get; set; }

        public TrackInfo()
        {
            Clear();
        }

        public void Clear()
        {
            AlbumId = 0;
            AlbumName = string.Empty;
            ArtistId = 0;
            ArtistName = string.Empty;
            GenreId = 0;
            GenreName = string.Empty;
            MediaTypeId = 0;
            MediaTypeName = string.Empty;
            OriginalTrackId = 0;
            Name = string.Empty;
            Composer = string.Empty;
            Time = 0;
            Size = 0;
            UnitPrice = new decimal(0.99);
        }
    }
}