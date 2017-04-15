using System;
using System.Threading.Tasks;

namespace ContentEngine.Persistence.AzureTable.Implementation
{
    public class ContentReader : IContentReader
    {
        public async Task<string> ReadData(Guid accountId, Guid contentKey)
        {
            throw new NotImplementedException();
        }
    }
}
