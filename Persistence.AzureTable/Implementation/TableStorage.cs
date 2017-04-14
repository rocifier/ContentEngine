using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using System;

namespace ContentEngine.Persistence.AzureTable.Implementation
{
    public class TableStorage : ITableStorage
    {
        public CloudTableClient TableClient { get; set; }

        public TableStorage(IConfigurationRoot configuration)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("AzureStorage"));
            TableClient = storageAccount.CreateCloudTableClient();
        }
        
    }
}
