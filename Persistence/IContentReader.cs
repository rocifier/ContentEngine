using System;

namespace ContentEngine.Persistence
{
    public interface IContentReader
    {
        /// <param name="accountId">Partition Key / Unique Project Id</param>
        /// <param name="contentKey">Row Key / Aggregate Root Id</param>
        /// <param name="fetchAggregate">True to recursively fetch all child items under the aggregate root `contentKey`</param>
        /// <returns>The JSON representation of the value stored at the requested location.</returns>
        string ReadJson(Guid accountId, Guid contentKey, bool fetchAggregate);
    }
}
