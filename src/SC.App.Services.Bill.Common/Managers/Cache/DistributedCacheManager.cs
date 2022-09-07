using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Common.Managers.Cache
{
    public class DistributedCacheManager : IDistributedCacheManager
    {
        /// <summary>
        /// The cache
        /// </summary>
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Constructor of DistributedCacheManager
        /// </summary>
        /// <param name="cache">The cache</param>
        public DistributedCacheManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <inheritdoc />
        public string Get(string key)
        {
            var raw = _cache.Get(key);

            return raw == null ? null : Encoding.UTF8.GetString(raw);
        }

        /// <inheritdoc />
        public T Get<T>(string key)
            where T : new()
        {
            var raw = _cache.Get(key);
            if (raw == null)
            {
                return default(T);
            }

            var decoded = Encoding.UTF8.GetString(raw);

            return JsonConvert.DeserializeObject<T>(decoded);
        }

        /// <inheritdoc />
        public void Set(string key, string value)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(value);
            _cache.Set(key, encoded);
        }

        /// <inheritdoc />
        public void Set(string key, string value, int cacheTimeInSeconds)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(value);
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(cacheTimeInSeconds)
            };

            _cache.Set(key, encoded, options);
        }

        /// <inheritdoc />
        public void Set<T>(string key, T value) where T : new()
        {
            var serialized = JsonConvert.SerializeObject(value);
            byte[] encoded = Encoding.UTF8.GetBytes(serialized);
            _cache.Set(key, encoded);
        }

        /// <inheritdoc />
        public void Set<T>(string key, T value, int cacheTimeInSeconds)
            where T : new()
        {
            var serialized = JsonConvert.SerializeObject(value);
            byte[] encoded = Encoding.UTF8.GetBytes(serialized);
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(cacheTimeInSeconds)
            };

            _cache.Set(key, encoded, options);
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}