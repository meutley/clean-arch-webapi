using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SourceName.Application.Common.Interfaces;

namespace SourceName.Infrastructure.Cache
{
    public class DebugCache : IApplicationCache
    {
        private readonly TimeSpan _defaultCacheDuration = TimeSpan.FromMinutes(30);
        private readonly IDictionary<object, Tuple<DateTime, TimeSpan, object>> _items =
            new Dictionary<object, Tuple<DateTime, TimeSpan, object>>();

        public async Task AddOrUpdate<TKey>(TKey key, object value, TimeSpan? cacheDuration = null)
        {
            var actualCacheDuration = cacheDuration ?? _defaultCacheDuration;
            if (!_items.ContainsKey(key))
            {
                await Add(key, actualCacheDuration, value);
            }
            else
            {
                _items[key] = new Tuple<DateTime, TimeSpan, object>(DateTime.Now, actualCacheDuration, value);
            }
        }

        public async Task<object> Get<TKey>(TKey key)
        {
            if (_items.ContainsKey(key))
            {
                var (timeAdded, cacheDuration, _) = _items[key];
                var cacheLifetime = DateTime.Now - timeAdded;
                var isExpired = cacheLifetime > cacheDuration;
                if (isExpired)
                {
                    await Remove(key);
                }
                
                return isExpired is false
                    ? Task.FromResult(_items[key].Item3)
                    : null;
            }

            return null;
        }

        public async Task<TResult> GetOrAdd<TResult, TKey>(TKey key, Func<TResult> addFunc, TimeSpan? cacheDuration = null)
        {
            var actualCacheDuration = cacheDuration ?? _defaultCacheDuration;
            if (_items.ContainsKey(key))
            {
                var (timeAdded, valueCacheDuration, value) = _items[key];
                var cacheLifetime = DateTime.Now - timeAdded;
                if (cacheLifetime > valueCacheDuration)
                {
                    await Add(key, actualCacheDuration, addFunc());
                }
                
                return (TResult)_items[key].Item3;
            }

            await Add(key, actualCacheDuration, addFunc());
            return (TResult)_items[key].Item3;
        }

        public Task Remove(object key)
        {
            if (_items.ContainsKey(key))
            {
                _items.Remove(key);
            }

            return Task.CompletedTask;
        }

        private Task Add(object key, TimeSpan cacheDuration, object value)
        {
            _items[key] = new Tuple<DateTime, TimeSpan, object>(DateTime.Now, cacheDuration, value);
            return Task.CompletedTask;
        }
    }
}