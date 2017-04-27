using ContentEngine.Persistence.Azure.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentEngine.Persistence.Azure.Implementation
{
    public class LinkReader : ILinkReader
    {
        private readonly ITableStorage _tableStorage;

        public LinkReader(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IList<LinkEntity>> GetLinks(Guid accountId, Guid contentKey)
        {
            CloudTable linksTable = _tableStorage.TableClient.GetTableReference(Constants.LINKS_TABLE);
            TableQuery<LinkEntity> query = new TableQuery<LinkEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, accountId.ToString()),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, contentKey.ToString())));

            List<LinkEntity> returnedEntities = new List<LinkEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<LinkEntity> resultSegment = await linksTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                returnedEntities.AddRange(resultSegment.Results);
            } while (token != null);
            return returnedEntities;
        }
        
    }
}
