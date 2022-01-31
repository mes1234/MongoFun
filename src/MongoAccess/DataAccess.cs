using DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MongoAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly DbConifg _dbConfig;
        private readonly MongoClient _client;
        private readonly ILogger<DataAccess> _logger;

        public DataAccess(
            IOptions<DbConifg> options,
            IConfiguration configuration,
            ILogger<DataAccess> logger)
        {
            var connectionString = configuration.GetConnectionString("mongo");

            _dbConfig = options.Value;

            _client = new(connectionString);
            _logger = logger;
        }

        public async Task<IEnumerable<T>> TryGet<T>(T filter, DateTime from, DateTime to)
            where T : ITimeStamped
        {

            try
            {
                var collection = GettCollection<Item>();

                var builder = Builders<Item>.Filter;


                var startFilter = builder.Gt(x => x.TimeStamp, from);
                var stopFilter = builder.Lte(x => x.TimeStamp, to);



                var dateFilter = builder.And(new[] { startFilter, stopFilter });

                if (dateFilter == null) throw new Exception("Filter builder failed");

                var results = await collection.FindAsync<T>(dateFilter);



                return results.ToList<T>() ?? throw new Exception("Data retrieval failed"); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during retrieval data from DB");
                throw;
            }
        }

        public async Task<bool> TryInsert<T>(T item)
            where T : ITimeStamped
        {
            try
            {
                var collection = GettCollection<T>();

                await collection.InsertOneAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during inserting data to DB");
                return false;
            }
        }

        private IMongoCollection<T> GettCollection<T>()
        {
            var db = _client.GetDatabase(_dbConfig.DbName);
            var collection = db.GetCollection<T>(_dbConfig.CollectionName);
            return collection;
        }
    }
}