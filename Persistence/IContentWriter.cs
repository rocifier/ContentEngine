using System;
using System.Threading.Tasks;

namespace ContentEngine.Persistence
{
    public interface IContentWriter
    {
        /// <param name="accountId">Partition Key / Unique Project Id</param>
        /// <param name="data">The JSON data to store. Must be a valid content object with an id property.</param>
        /// <returns>The JSON representation of the value stored at the requested location.</returns>
        Task<bool> WriteJson(Guid accountId, string data);
    }
}
