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

        public ContentWriter(ITableStorage tableStorage, ILinkReader linkReader, IDataUpdater dataUpdater) {

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
                links.Add(GenerateRootContent(accountId, contentKey));
            }

            // 3. for all linked content items, update or add 'data' and store
            foreach (var link in links) {
                var content = await GetLinkedContent(link);
                _dataUpdater.Update(content);
            }

            return true;
        }

        private LinkEntity GenerateRootContent(Guid accountId, Guid contentKey)
        {
            Guid newRootContentId = Guid.NewGuid();
            var standaloneContent = new ContentEntity(accountId.ToString(), newRootContentId.ToString());
            var rootLink = new LinkEntity(accountId.ToString(), newRootContentId.ToString());
            TableOperation insertContent = TableOperation.Insert(standaloneContent);
            TableResult insertContentResult = _contentTable.ExecuteAsync(insertContent).Result;
            if (insertContentResult.HttpStatusCode != 200) throw new Exception(insertContentResult.Result.ToString());
            TableOperation insertRootLink = TableOperation.Insert(rootLink);
            TableResult insertLinkResult = _linkTable.ExecuteAsync(insertRootLink).Result;
            if (insertLinkResult.HttpStatusCode != 200) throw new Exception(insertLinkResult.Result.ToString());
            return rootLink;
        }
    }
}
