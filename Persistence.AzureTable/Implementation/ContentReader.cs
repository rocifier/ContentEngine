using ContentEngine.Persistence.AzureTable.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace ContentEngine.Persistence.AzureTable.Implementation
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
            return ((ContentEntity)retrievedResult.Result).Data;
        }
    }
}
