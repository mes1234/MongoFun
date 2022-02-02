using DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MongoAccess
{
    public partial class DataAccess : IDataAccess
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
                var filterBuilder = new FilterBuilder<T>();

                var collection = GetCollection<T>();

                var dateFilter = BuildDateFilter<T>(from, to);

                var customFilter = filterBuilder.BuildFilter(filter);

                var filters = dateFilter & customFilter;

                var results = await collection.FindAsync<T>(filters);

                return await results.ToListAsync<T>();
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
                var collection = GetCollection<T>();

                await collection.InsertOneAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during inserting data to DB");
                return false;
            }
        }
    }
}