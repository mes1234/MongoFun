using DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;

namespace MongoAccess
{
    public partial class DataAccess
    {
        private static FilterDefinition<T> BuildDateFilter<T>(DateTime from, DateTime to) where T : ITimeStamped
        {
            var builder = Builders<T>.Filter;

            var startFilter = builder.Gt(x => x.TimeStamp, from);
            var stopFilter = builder.Lte(x => x.TimeStamp, to);

            return (from == DateTime.MinValue)
                ? startFilter
                : builder.And(new[] { startFilter, stopFilter });
        }



        private IMongoCollection<T> GetCollection<T>()
        {
            var db = _client.GetDatabase(_dbConfig.DbName);
            var collection = db.GetCollection<T>(_dbConfig.CollectionName);
            return collection;
        }
    }
}