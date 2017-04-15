using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using ContentEngine.Persistence.AzureTable.Models;

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

        public async Task<bool> WriteJson(Guid accountId, Guid contentKey, string data)
        {
            if (!data.IsValidJson()) return false;
            
            // 1. get all links
            var links = await _linkReader.GetLinks(accountId, contentKey);

            // 2. if there are no links, add a root link and a content item
            if (!links.Any())
            {
                links.Add(await GenerateRootContent(accountId, contentKey));
            }

            // 3. for all linked content items, update 'data' and store
            foreach (var link in links) {
                Guid contentId = Guid.Parse(link.RowKey.FirstPartOfCompositeIndex());
                var content = await _contentReader.ReadData(accountId, contentId);
                var mergedData = _dataUpdater.Update(content, contentId, data);
                await WriteJsonValue(accountId, contentId, mergedData);
            }

            return true;
        }

        private async Task<LinkEntity> GenerateRootContent(Guid accountId, Guid contentKey)
        {
            Guid newRootContentId = Guid.NewGuid();
            var standaloneContent = new ContentEntity(accountId.ToString(), newRootContentId.ToString());
            var rootLink = new LinkEntity(accountId.ToString(), newRootContentId.ToString());
            TableOperation insertContent = TableOperation.Insert(standaloneContent);
            TableResult insertContentResult = await _contentTable.ExecuteAsync(insertContent);
            if (insertContentResult.HttpStatusCode != 200) throw new Exception(insertContentResult.Result.ToString());
            TableOperation insertRootLink = TableOperation.Insert(rootLink);
            TableResult insertLinkResult = await _linkTable.ExecuteAsync(insertRootLink);
            if (insertLinkResult.HttpStatusCode != 200) throw new Exception(insertLinkResult.Result.ToString());
            return rootLink;
        }

        private async Task<bool> WriteJsonValue(Guid accountId, Guid contentId, string newFullData)
        {
            var standaloneContent = new ContentEntity(accountId.ToString(), contentId.ToString());
            standaloneContent.Data = newFullData;
            TableOperation insertContent = TableOperation.InsertOrReplace(standaloneContent);
            TableResult insertContentResult = await _contentTable.ExecuteAsync(insertContent);
            if (insertContentResult.HttpStatusCode == 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
