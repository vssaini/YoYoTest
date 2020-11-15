using System.Collections.Generic;
using YoYo.Domain.Entities;

namespace YoYo.Service
{
    /// <summary>
    /// Provide methods for retrieving and setting values in cache.
    /// </summary>
    public interface ICacheHelper
    {
        /// <summary>
        /// Get fitness ratings from cache
        /// </summary>
        /// <returns>List of fitness ratings.</returns>
        List<FitnessRating> GetFitnessRatingsFromCache();

        /// <summary>
        /// Set fitness ratings in cache.
        /// </summary>
        void SetFitnessRatingsInCache(List<FitnessRating> fitnessRatings);
    }
}
