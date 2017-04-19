using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using ContentEngine.Persistence.AzureTable.Models;
using System.Collections.Generic;

namespace ContentEngine.Persistence.AzureTable.Implementation
{
    public class ContentWriter : IContentWriter
    {
        private readonly CloudTable _linkTable;
        private readonly CloudTable _contentTable;
        private readonly ILinkReader _linkReader;
        private readonly IDataUpdater _dataUpdater;
        private readonly IContentReader _contentReader;

        public ContentWriter(ITableStorage tableStorage, ILinkReader linkReader, IContentReader contentReader, IDataUpdater dataUpdater)
        {
            _contentReader = contentReader;
            _linkReader = linkReader;
            _dataUpdater = dataUpdater;

            _linkTable = tableStorage.TableClient.GetTableReference(Constants.LINKS_TABLE);
            _linkTable.CreateIfNotExistsAsync();
            _contentTable = tableStorage.TableClient.GetTableReference(Constants.CONTENT_TABLE);
            _contentTable.CreateIfNotExistsAsync();
        }

        public async Task<bool> WriteJson(Guid accountId, string data)
        {
            if (!data.IsValidJson()) return false;
            if (!data.IsValidContentModel()) return false;

            // Get content key
            Guid contentKey = data.ExtractContentKey();

            // 1. get all links
            var links = await _linkReader.GetLinks(accountId, contentKey);

            // 2. if there are no links, add a root link and a content item
            if (!links.Any())
            {
                links.Add(await GenerateRootContent(accountId, contentKey, data));
            }

            // 3. for all linked content items, update 'data' and store
            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (var link in links) {
                Guid contentId = Guid.Parse(link.RowKey.FirstPartOfCompositeIndex());
                var content = await _contentReader.ReadData(accountId, contentId);
                var mergedData = _dataUpdater.Update(content, contentId, data);
                BatchInsertContent(accountId, contentId, mergedData, batchOperation);
            }

            // commit batch update
            IList<TableResult> insertContentResults = await _contentTable.ExecuteBatchAsync(batchOperation);
            if (insertContentResults.Any(r => !r.HttpStatusCode.IsSuccessfulHttpStatusCode()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<LinkEntity> GenerateRootContent(Guid accountId, Guid contentKey, string initialData)
        {
            var standaloneContent = new ContentEntity(accountId.ToString(), contentKey.ToString());
            standaloneContent.Data = initialData;
            var rootLink = new LinkEntity(accountId.ToString(), contentKey.ToString());
            rootLink.Primary = true;
            TableOperation insertContent = TableOperation.Insert(standaloneContent);
            TableResult insertContentResult = await _contentTable.ExecuteAsync(insertContent);
            if (!insertContentResult.HttpStatusCode.IsSuccessfulHttpStatusCode()) throw new Exception(insertContentResult.Result.ToString());
            TableOperation insertRootLink = TableOperation.Insert(rootLink);
            TableResult insertLinkResult = await _linkTable.ExecuteAsync(insertRootLink);
            if (!insertLinkResult.HttpStatusCode.IsSuccessfulHttpStatusCode()) throw new Exception(insertLinkResult.Result.ToString());
            return rootLink;
        }

        private void BatchInsertContent(Guid accountId, Guid contentId, string newFullData, TableBatchOperation batch)
        {
            var standaloneContent = new ContentEntity(accountId.ToString(), contentId.ToString());
            standaloneContent.Data = newFullData;
            batch.InsertOrReplace(standaloneContent);
        }
    }
}
