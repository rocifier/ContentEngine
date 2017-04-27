using ContentEngine.Persistence.Azure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentEngine.Persistence.Azure
{
    public interface ILinkReader
    {
        Task<IList<LinkEntity>> GetLinks(Guid accountId, Guid contentKey);
    }
}
