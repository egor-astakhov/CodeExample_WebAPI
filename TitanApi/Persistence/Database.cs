using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanApi.Core.Models;

namespace TitanApi.Persistence
{
    public class Database : IDatabase
    {
        private readonly IMongoDatabase _db;

        public Database(IMongoDatabase db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> GetListAsync<T>() where T : IPersistent
        {
            return await GetQueryable<T>().ToListAsync();
        }

        public IMongoQueryable<T> GetQueryable<T>() where T : IPersistent
        {
            return GetCollection<T>().AsQueryable();
        }

        public async Task InsertAsync<T>(T document) where T : IPersistent
        {
            await GetCollection<T>().InsertOneAsync(document);
        }

        public async Task ReplaceAsync<T>(T document) where T : IPersistent
        {
            if (string.IsNullOrEmpty(document.Id))
            {
                throw new ApplicationException("Cannot replace document without Id");
            }

            var filter = Builders<T>.Filter.Eq(m => m.Id, document.Id);

            await GetCollection<T>().ReplaceOneAsync(filter, document);
        }    

        public IMongoCollection<T> GetCollection<T>() where T : IPersistent
        {
            return _db.GetCollection<T>(typeof(T).Name);
        }
    }
}
