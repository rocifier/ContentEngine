using System;

namespace ContentEngine.Persistence
{
    public interface IContentWriter
    {
        /// <param name="accountId">Partition Key / Unique Project Id</param>
        /// <param name="contentKey">Row Key / Aggregate Root Id</param>
        /// <param name="data">The JSON data to store. </param>
        /// <returns>The JSON representation of the value stored at the requested location.</returns>
        bool WriteJson(Guid accountId, Guid contentKey, string data);
    }
}
