using ContentEngine.Persistence.AzureTable.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentEngine.Persistence.AzureTable
{
    public interface ILinkReader
    {
        Task<IList<LinkEntity>> GetLinks(Guid accountId, Guid contentKey);
    }
}
