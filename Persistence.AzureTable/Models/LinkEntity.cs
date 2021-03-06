using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ContentEngine.Persistence.Azure.Models
{
    /// Enumerates content entities stored in all aggregates
    public class LinkEntity : TableEntity
    {
        public LinkEntity(string accountId, string contentId)
        {
            this.PartitionKey = accountId;
            this.RowKey = contentId + "_" + Guid.NewGuid();
        }

        public LinkEntity() { }

        public bool Primary { get; set; }
    }
}