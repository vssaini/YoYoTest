using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using YoYo.Domain.Entities;

namespace YoYo.Service
{
    public class CacheHelper : ICacheHelper
    {
        private const string CacheKey = "YoYo_WebApp";

        private readonly IMemoryCache _memoryCache;
        public CacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<FitnessRating> GetFitnessRatingsFromCache()
        {
            return _memoryCache.TryGetValue(CacheKey, out List<FitnessRating> fitnessRatings) ? fitnessRatings : null;
        }

        public void SetFitnessRatingsInCache(List<FitnessRating> fitnessRatings)
        {
            var memoryCacheOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(15));
            _memoryCache.Set(CacheKey, fitnessRatings, memoryCacheOption);
        }
    }
}
