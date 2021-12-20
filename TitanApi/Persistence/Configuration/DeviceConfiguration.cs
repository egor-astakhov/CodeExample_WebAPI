using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using TitanApi.Core.Models;

namespace TitanApi.Persistence.Configuration
{
    public class DeviceConfiguration : CollectionConfiguration<Device>
    {
        public DeviceConfiguration(IMongoDatabase db) : base(db)
        {

        }

        public override void Configure()
        {
            base.Configure();

            AddUnique(nameof(Device.Name));
            AddUnique(nameof(Device.Url));
        }

        protected override Action<BsonClassMap<Device>> Mapper => cm =>
        {
            cm.MapIdMember(c => c.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));

            cm.MapMember(c => c.Name).SetIsRequired(true);
            cm.MapMember(c => c.Url).SetIsRequired(true);
            cm.MapMember(c => c.Type).SetIsRequired(true);
        };
    }
}
