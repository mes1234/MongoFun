using DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;

namespace MongoAccess
{
    public partial class MongoDataAccess
    {
        private static FilterDefinition<T> BuildDateFilter<T>(DateTime from, DateTime to) where T : ITimeStamped
        {
            var builder = Builders<T>.Filter;

            var startFilter = builder.Gt(x => x.TimeStamp, new BsonDateTime(from));
            var stopFilter = builder.Lte(x => x.TimeStamp, new BsonDateTime(to));

            return builder.And(new[] { startFilter, stopFilter });
        }



        private IMongoCollection<T> GetCollection<T>()
        {
            var db = _client.GetDatabase(_dbConfig.DbName);
            var collection = db.GetCollection<T>(_dbConfig.CollectionName);
            return collection;
        }
    }
}