using DataModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoAccess;

namespace Publisher
{
    public class Worker : BackgroundService
    {
        private readonly IDataAccess _dataAccess;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IDataAccess dataAccess,
            ILogger<Worker> logger)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var random = new Random(stoppingToken.GetHashCode());

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                await _dataAccess.TryInsert<Item>(new Item
                {
                    ContentType = (ContentType)random.Next(Enum.GetNames(typeof(ContentType)).Length),
                    Data = new byte[100],
                    Name = "dummy",
                    TimeStamp = DateTime.UtcNow,

                }).ConfigureAwait(false);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}