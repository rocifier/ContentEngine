using ContentEngine.Persistence;
using ContentEngine.Persistence.Azure.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace ContentEngine.Persistence.Azure.Implementation
{
    public class ContentReader : IContentReader
    {
        private readonly ITableStorage _tableStorage;
        private readonly CloudTable _contentTable;

        public ContentReader(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _contentTable = tableStorage.TableClient.GetTableReference(Constants.CONTENT_TABLE);
            _contentTable.CreateIfNotExistsAsync();
        }

        public async Task<string> ReadData(Guid accountId, Guid contentKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<ContentEntity>(accountId.ToString(), contentKey.ToString());
            TableResult retrievedResult = await _contentTable.ExecuteAsync(retrieveOperation);
            var contentResult = ((ContentEntity)retrievedResult.Result);
            return contentResult.Data.AddJsonProperty(Constants.ETAG_CLIENT, contentResult.ETag);
        }
    }
}
