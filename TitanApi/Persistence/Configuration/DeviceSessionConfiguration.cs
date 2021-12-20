using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using TitanApi.Core.Models;

namespace TitanApi.Persistence.Configuration
{
    public class DeviceSessionConfiguration : CollectionConfiguration<DeviceSession>
    {
        public DeviceSessionConfiguration(IMongoDatabase db) : base(db)
        {

        }

        protected override Action<BsonClassMap<DeviceSession>> Mapper => cm =>
        {
            cm.MapIdMember(c => c.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));

            cm.MapMember(c => c.DeviceId)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));

            cm.MapMember(c => c.StartedAt)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local))
                .SetIsRequired(true);

            cm.MapMember(c => c.EndedAt)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Local)));
        };
    }
}
