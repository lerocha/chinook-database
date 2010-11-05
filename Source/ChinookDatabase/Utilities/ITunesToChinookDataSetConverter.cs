using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.XPath;
using ChinookMetadata.Schema;

namespace ChinookMetadata.Convert
{
    public class ITunesToChinookDataSetConverter
    {
        private readonly string _filename;
        private readonly string _xmlNonMediaDataFilename;
        private readonly List<string> _excludedPlaylists = new List<string>();
        private readonly Dictionary<int, int> _trackIds = new Dictionary<int, int>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iTunesFilename">Full file name of iTunes library file.</param>
        public ITunesToChinookDataSetConverter(string iTunesFilename)
            : this(iTunesFilename, null, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iTunesFilename">Full file name of iTunes library file.</param>
        /// <param name="xmlNonMediaDataFilename">Xml file containing non media data (customers and employees).</param>
        public ITunesToChinookDataSetConverter(string iTunesFilename, string xmlNonMediaDataFilename)
            : this(iTunesFilename, xmlNonMediaDataFilename, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iTunesFilename">Full file name of iTunes library file.</param>
        /// <param name="xmlNonMediaDataFilename">Xml file containing non media data (customers and employees).</param>
        /// <param name="excludePlayLists">A list of playlists to be excluded when converting.</param>
        public ITunesToChinookDataSetConverter(string iTunesFilename, string xmlNonMediaDataFilename, IEnumerable<string> excludePlayLists)
        {
            _filename = iTunesFilename;
            _xmlNonMediaDataFilename = xmlNonMediaDataFilename;
            if (excludePlayLists != null)
            {
                _excludedPlaylists.AddRange(excludePlayLists);
            }
        }

        /// <summary>
        /// Builds a dataset and populates it with iTunes library information.
        /// </summary>
        /// <returns>Populated dataset.</returns>
        public ChinookDataSet BuildDataSet()
        {
            // Create a dataset and prepopulate it.
            var ds = new ChinookDataSet();

            // If non media data is supplied, then prepopulate the dataset.
            if (_xmlNonMediaDataFilename != null && File.Exists(_xmlNonMediaDataFilename))
            {
                ds.ReadXml(_xmlNonMediaDataFilename);
            }

            // Create an XPath navigator for the iTunes library.
            if (File.Exists(_filename))
            {
                var document = new XPathDocument(_filename);
                var navigator = document.CreateNavigator();

                // Import tracks (including album, artist, media type, etc.) and playlists.
                ImportTracks(ds, navigator);
                ImportPlaylists(ds, navigator);
            }

            return ds;
        }

        /// <summary>
        /// Import media track information from iTunes library file.
        /// </summary>
        /// <param name="ds">ChinookDataSet to be populated.</param>
        /// <param name="navigator">XPath navigator of iTunes library.</param>
        private void ImportTracks(ChinookDataSet ds, XPathNavigator navigator)
        {
            var trackInfo = new TrackInfo();
            // Creates an XPath to the track info nodes.
            var nodes = navigator.Select("/plist/dict/dict/dict");
            while (nodes.MoveNext())
            {
                trackInfo.Clear();
                var clone = nodes.Current.Clone();
                var iterator = clone.SelectChildren("", "");

                if (ParseTrackNode(trackInfo, iterator))
                {
                    trackInfo.ArtistId = AddArtist(trackInfo, ds);
                    trackInfo.AlbumId = AddAlbum(trackInfo, ds);
                    trackInfo.GenreId = AddGenre(trackInfo, ds);
                    trackInfo.MediaTypeId = AddMediaType(trackInfo, ds);

                    // Add a new track to the dataset.
                    ChinookDataSet.TrackRow row = ds.Track.AddTrackRow(trackInfo.Name, trackInfo.AlbumId,
                                                                       trackInfo.MediaTypeId, trackInfo.GenreId,
                                                                       trackInfo.Composer, trackInfo.Time,
                                                                       trackInfo.Size, trackInfo.UnitPrice);

                    // Maps the track ids used by iTunes in order to use it later when importing playlists.
                    _trackIds[trackInfo.OriginalTrackId] = row.TrackId;
                }
            }
        }

        /// <summary>
        /// Parses a track node of iTunes library.
        /// </summary>
        /// <param name="trackInfo">Parsed iTunes track info.</param>
        /// <param name="iterator">Iterator over iTunes track info nodes.</param>
        /// <returns>Returns false if this track should not be parsed, returns true otherwise.</returns>
        private static bool ParseTrackNode(TrackInfo trackInfo, XPathNodeIterator iterator)
        {
            while (iterator.MoveNext())
            {
                switch (iterator.Current.Value)
                {
                    case "Artist":
                        trackInfo.ArtistName = GetNextString(iterator);
                        break;
                    case "Album":
                        trackInfo.AlbumName = GetNextString(iterator);
                        break;
                    case "Genre":
                        trackInfo.GenreName = GetNextString(iterator);
                        break;
                    case "Kind":
                        trackInfo.MediaTypeName = GetNextString(iterator);
                        break;
                    case "Name":
                        trackInfo.Name = GetNextString(iterator);
                        break;
                    case "Composer":
                        trackInfo.Composer = GetNextString(iterator);
                        break;
                    case "Track ID":
                        trackInfo.OriginalTrackId = GetNextInteger(iterator);
                        break;
                    case "Size":
                        trackInfo.Size = GetNextInteger(iterator);
                        break;
                    case "Total Time":
                        trackInfo.Time = GetNextInteger(iterator);
                        break;
                    case "TV Show":
                        trackInfo.UnitPrice = (decimal) 1.99;
                        break;
                    case "Movie":
                        trackInfo.UnitPrice = (decimal) 9.99;
                        break;
                    case "Podcast":
                        return false;
                    default:
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the album information from the iTunes track nodes, and add a new album if it does not exist yet.
        /// </summary>
        /// <param name="trackInfo">Parsed iTunes track info.</param>
        /// <param name="ds">ChinookDataSet to be populated.</param>
        /// <returns>Returns the AlbumId.</returns>
        private static int AddAlbum(TrackInfo trackInfo, ChinookDataSet ds)
        {
            DataRow[] results = ds.Album.Select("Title = '" + trackInfo.AlbumName.Replace("'", "''") + "'");

            if (results == null || results.Length == 0)
            {
                var row = ds.Album.AddAlbumRow(trackInfo.AlbumName, trackInfo.ArtistId);
                return row.AlbumId;
            }

            return (int)results[0][0];
        }

        /// <summary>
        /// Get the artist information from the iTunes track nodes, and add a new artist if it does not exist yet.
        /// </summary>
        /// <param name="trackInfo">Parsed iTunes track info.</param>
        /// <param name="ds">ChinookDataSet to be populated.</param>
        /// <returns>Returns the ArtistId.</returns>
        private static int AddArtist(TrackInfo trackInfo, ChinookDataSet ds)
        {
            DataRow[] results = ds.Artist.Select("Name = '" + trackInfo.ArtistName.Replace("'", "''") + "'");

            if (results == null || results.Length == 0)
            {
                var row = ds.Artist.AddArtistRow(trackInfo.ArtistName);
                return row.ArtistId;
            }
            
            return (int)results[0][0];
        }

        /// <summary>
        /// Get the genre information from the iTunes track nodes, and add a new genre if it does not exist yet.
        /// </summary>
        /// <param name="trackInfo">Parsed iTunes track info.</param>
        /// <param name="ds">ChinookDataSet to be populated.</param>
        /// <returns>Returns the GenreId.</returns>
        private static int AddGenre(TrackInfo trackInfo, ChinookDataSet ds)
        {
            DataRow[] results = ds.Genre.Select("Name = '" + trackInfo.GenreName.Replace("'", "''") + "'");

            if (results == null || results.Length == 0)
            {
                var row = ds.Genre.AddGenreRow(trackInfo.GenreName);
                return row.GenreId;
            }

            return (int)results[0][0];
        }

        /// <summary>
        /// Get the media type information from the iTunes track nodes, and add a new media type if it does not exist yet.
        /// </summary>
        /// <param name="trackInfo">Parsed iTunes track info.</param>
        /// <param name="ds">ChinookDataSet to be populated.</param>
        /// <returns>Returns the MediaTypeId.</returns>
        private static int AddMediaType(TrackInfo trackInfo, ChinookDataSet ds)
        {
            DataRow[] results = ds.MediaType.Select("Name = '" + trackInfo.MediaTypeName.Replace("'", "''") + "'");

            if (results == null || results.Length == 0)
            {
                var row = ds.MediaType.AddMediaTypeRow(trackInfo.MediaTypeName);
                return row.MediaTypeId;
            }

            return (int)results[0][0];
        }

        /// <summary>
        /// Import playlist information from iTunes library.
        /// </summary>
        /// <param name="ds">ChinookDataSet to be populated.</param>
        /// <param name="navigator">XPath navigator of iTunes library.</param>
        private void ImportPlaylists(ChinookDataSet ds, XPathNavigator navigator)
        {
            // Navigate to the node that contains the playlist name.
            var playlistNodes = navigator.Select("/plist/dict/array/dict/string[position()=1]");

            while (playlistNodes.MoveNext())
            {
                var clone = playlistNodes.Current.Clone();
                var name = clone.Value;

                // If it is one of the excluded playlists, then skip it.
                if (_excludedPlaylists.Contains(name)) continue;

                // Add a new Playlist
                var playlistRow = ds.Playlist.AddPlaylistRow(name);

                clone.MoveToParent();
                var trackNodes = clone.Select("./array/dict/integer");

                while (trackNodes.MoveNext())
                {
                    int originalTrackId;
                    if (int.TryParse(trackNodes.Current.Value, out originalTrackId) && _trackIds.ContainsKey(originalTrackId))
                    {
                        // Add a new Playlist
                        ds.PlaylistTrack.AddPlaylistTrackRow(playlistRow.PlaylistId, _trackIds[originalTrackId]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the string of the next node in the iterator node set.
        /// </summary>
        /// <param name="iterator">Iterator over iTunes info nodes.</param>
        /// <returns>A string containing the value of the next node.</returns>
        private static string GetNextString(XPathNodeIterator iterator)
        {
            iterator.MoveNext();
            return iterator.Current.Value;
        }

        /// <summary>
        /// Gets the integer of the next node in the iterator node set.
        /// </summary>
        /// <param name="iterator">Iterator over iTunes info nodes.</param>
        /// <returns>The integer value of the next node. If the next node does not contain an integer value then return zero.</returns>
        private static int GetNextInteger(XPathNodeIterator iterator)
        {
            iterator.MoveNext();
            int result;
            int.TryParse(iterator.Current.Value, out result);
            return result;
        }
    }
}