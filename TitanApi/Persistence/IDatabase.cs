using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanApi.Core.Models;

namespace TitanApi.Persistence
{
    public interface IDatabase
    {
        IMongoCollection<T> GetCollection<T>() where T : IPersistent;

        IMongoQueryable<T> GetQueryable<T>() where T : IPersistent;

        Task<IEnumerable<T>> GetListAsync<T>() where T : IPersistent;

        Task InsertAsync<T>(T document) where T : IPersistent;

        Task ReplaceAsync<T>(T document) where T : IPersistent;
    }
}
