using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ContentEngine.Persistence.AzureTable.Models
{
    public class ContentEntity : TableEntity
    {
        public ContentEntity(string accountId, string contentId)
        {
            this.PartitionKey = accountId;
            this.RowKey = contentId;
        }

        public ContentEntity() { }

        public string Data { get; set; }
    }
}