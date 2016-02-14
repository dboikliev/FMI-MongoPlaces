using System.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace MongoPlaces.Core.DataAccess.Implementations
{
    public abstract class AbstractRepository
    {
        protected static readonly MongoClient Client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoPlacesConnection"].ConnectionString);
        protected static readonly IMongoDatabase Database = Client.GetDatabase("MongoPlaces");

        static AbstractRepository()
        {
            var pack = new ConventionPack();
            var ignoreIfNull = new IgnoreIfNullConvention(true);
            pack.Add(ignoreIfNull);
            ConventionRegistry.Register("ignoreNulls", pack, t => true);

            Migrations.CreatePointsOfInterest();
            Migrations.CreateUsers();
            Migrations.EnsureGeospacialIndex();
            Migrations.EnsureUniqueEmailIndex();
            Migrations.CreateLocationTypes();
        }
    }

    public abstract class AbstractRepository<T> : AbstractRepository
    {
        protected abstract string CollectionName { get; }

        private IMongoCollection<T> _collection; 

        protected IMongoCollection<T> Collection => _collection ?? (_collection = Database.GetCollection<T>(CollectionName));
    }
}
