using System;
using System.Threading.Tasks;

namespace SourceName.Application.Common.Interfaces
{
    public interface IApplicationCache
    {
        Task AddOrUpdate<TKey>(
            TKey key,
            object value,
            TimeSpan? cacheDuration = null
        );

        Task<object> Get<TKey>(
            TKey key
        );
        
        Task<TResult> GetOrAdd<TResult, TKey>(
            TKey key,
            Func<TResult> addFunc,
            TimeSpan? cacheDuration = null
        );
    }
}