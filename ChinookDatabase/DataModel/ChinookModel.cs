using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;

namespace ChinookDatabase.DataModel
{
    public class ChinookModel
    {
        private static DbModel _chinookModel;

        public static DbModel CreateModel()
        {
            if (_chinookModel == null)
            {
                var builder = new ModelBuilder();
                builder.Entity<Genre>();
                builder.Entity<MediaType>();
                builder.Entity<Artist>();
                builder.Entity<Album>();
                builder.Entity<Track>();
                builder.Entity<Employee>();
                builder.Entity<Customer>();
                builder.Entity<Invoice>();
                builder.Entity<InvoiceLine>();
                builder.Entity<Playlist>();
                builder.Entity<PlaylistTrack>();
                _chinookModel = builder.CreateModel();
            }

            return _chinookModel;
        }
    }
}
