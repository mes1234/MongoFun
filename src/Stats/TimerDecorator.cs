using DataModel;
using System.Runtime.Caching;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Stats
{
    public class TimerDecorator : IDataAccess
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
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
            var key = $"INSERT_{_inner.GetType().Name}";

            _stopwatch.Restart();
            _stopwatch.Start();
            var result = await _inner.TryInsert(item).ConfigureAwait(false);
            _stopwatch.Stop();
            _logger.LogInformation("{DbType} inserted item in {Time}:ms", _inner.GetType().Name, _stopwatch.ElapsedMilliseconds);

            if (_cache.Contains(key))
                _cache.Set(key, UpdateStat((Statistics)_cache.Get(key)), new CacheItemPolicy());
            else
                _cache.Set(key, new Statistics { Amount = 1, Average = _stopwatch.ElapsedMilliseconds }, new CacheItemPolicy());

            return result;
        }

        private Statistics UpdateStat(Statistics currentStat)
        {
            var newStat = (currentStat.Amount * currentStat.Average + _stopwatch.ElapsedMilliseconds) / (currentStat.Amount + 1);
            currentStat.Average = newStat;
            currentStat.Amount++;
            return currentStat;
        }

        public async Task<IEnumerable<T>> TryGet<T>(T filter, DateTime from, DateTime to) where T : ITimeStamped
        {
            var key = $"GET_{_inner.GetType().Name}";

            _stopwatch.Restart();
            _stopwatch.Start();
            var result = await _inner.TryGet(filter, from, to).ConfigureAwait(false);
            _stopwatch.Stop();
            _logger.LogInformation("{DbType} retrieved item in {Time}:ms", _inner.GetType().Name, _stopwatch.ElapsedMilliseconds);

            if (_cache.Contains(key))
                _cache.Set(key, UpdateStat((Statistics)_cache.Get(key)), new CacheItemPolicy());
            else
                _cache.Set(key, new Statistics { Amount = 1, Average = _stopwatch.ElapsedMilliseconds }, new CacheItemPolicy());

            return result;

        }

    }
}