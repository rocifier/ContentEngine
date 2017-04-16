using System;

namespace ContentEngine.Persistence
{
    public interface IDataUpdater
    {
        /// <summary>
        /// Updates destination (normalized) data by overriding the first occurence of contentId with contentData
        /// </summary>
        /// <param name="denormalizedData">Full data to be updated</param>
        /// <param name="contentId">Content Id to update in normalized data</param>
        /// <param name="contentData">New data to update normalizedData with</param>
        /// <returns>updated data</returns>
        string Update(string denormalizedData, Guid contentId, string contentData);
    }
}
