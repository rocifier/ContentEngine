using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ContentEngine.Persistence.AzureTable
{
    public class ContentWriter : IContentWriter
    {
        private readonly CloudTableClient _tableClient;
        private readonly CloudTable _linkTable;

        public ContentWriter() {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=contentengine;AccountKey=+dD0y9FE9/BCvyO4VseQGOp+nXLzqEImpRwpJVlU5yzgKY+lRNE1r6L1WiwExvj/W8sI/lP+qC/e/RtIAK2JNA==;EndpointSuffix=core.windows.net");
            _tableClient = storageAccount.CreateCloudTableClient();
            _linkTable = _tableClient.GetTableReference("links");
            _linkTable.CreateIfNotExistsAsync();
        }

        public bool WriteJson(Guid accountId, Guid contentKey, string data)
        {
            throw new NotImplementedException();
        }
    }
}
