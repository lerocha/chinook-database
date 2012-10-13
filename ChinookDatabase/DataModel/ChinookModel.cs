using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ChinookDatabase.DataModel
{
    public class ChinookModel
    {
        private static DbModel _chinookModel;

        public static DbModel CreateModel(DbConnection connection)
        {
            if (_chinookModel == null)
            {
                var builder = new DbModelBuilder();
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
                _chinookModel = builder.Build(connection);
            }

            return _chinookModel;
        }
    }
}
