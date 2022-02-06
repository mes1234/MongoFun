using DataModel;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Stats
{
    public class TimerDecorator : IDataAccess
    {
        private readonly IDataAccess _inner;
        private readonly ILogger<TimerDecorator> _logger;
        private readonly Stopwatch _stopwatch;
        public TimerDecorator(IDataAccess inner, ILogger<TimerDecorator> logger)
        {
            _stopwatch = new Stopwatch();
            _inner = inner;
            _logger = logger;
        }

        public async Task<bool> TryInsert<T>(T item) where T : ITimeStamped
        {
            _stopwatch.Restart();
            _stopwatch.Start();
            var result = await _inner.TryInsert(item).ConfigureAwait(false);
            _stopwatch.Stop();
            _logger.LogInformation("{DbType} inserted item in {Time}:ms", _inner.GetType().Name, _stopwatch.ElapsedMilliseconds);
            return result;
        }

        public async Task<IEnumerable<T>> TryGet<T>(T filter, DateTime from, DateTime to) where T : ITimeStamped
        {
            _stopwatch.Restart();
            _stopwatch.Start();
            var result = await _inner.TryGet(filter, from, to).ConfigureAwait(false);
            _stopwatch.Stop();
            _logger.LogInformation("{DbType} retrieved item in {Time}:ms", _inner.GetType().Name, _stopwatch.ElapsedMilliseconds);
            return result;

        }

    }
}