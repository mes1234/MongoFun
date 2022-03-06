using DataModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoAccess;
using MsSqlAccess;

namespace Publisher
{
    public class Worker : BackgroundService
    {
        private readonly IEnumerable<IDataAccess> _dataAccesses;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IEnumerable<IDataAccess> dataAccesses,
            ILogger<Worker> logger)
        {
            _dataAccesses = dataAccesses ?? throw new ArgumentNullException(nameof(dataAccesses));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var names = new List<string>
            {
                "one",
                "two",
                "three",
                "four",
                "five"
            };

            var random = new Random(stoppingToken.GetHashCode());

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                var data = new Item
                {
                    ContentType = (ContentType)random.Next(Enum.GetNames(typeof(ContentType)).Length),
                    Data = new byte[100],
                    Name = names[random.Next(names.Count)],
                    Description = names[random.Next(names.Count)],
                    TimeStamp = DateTime.Now,

                };

                foreach (var dataAccess in _dataAccesses)
                    await dataAccess.TryInsert<Item>(data).ConfigureAwait(false);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}