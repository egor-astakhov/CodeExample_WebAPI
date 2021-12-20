using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;

namespace TitanApi.Persistence.Configuration
{
    public abstract class CollectionConfiguration<T> : ICollectionConfiguration where T : class
    {
        public CollectionConfiguration(IMongoDatabase db)
        {
            Database = db;
        }

        protected IMongoDatabase Database { get; }

        public virtual void Configure()
        {
            BsonClassMap.RegisterClassMap(Mapper);
        }

        protected abstract Action<BsonClassMap<T>> Mapper { get; }

        protected void AddUnique(string fieldName)
        {
            var model = new CreateIndexModel<T>(Builders<T>.IndexKeys.Ascending(fieldName), new CreateIndexOptions()
            {
                Unique = true,
                Name = $"INDEX_UNIQUE_{fieldName.ToUpper()}"
            });

            Database.GetCollection<T>(typeof(T).Name).Indexes.CreateOne(model);
        }
    }
}
