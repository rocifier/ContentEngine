using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentEngine.Persistence.AzureTable
{
    public interface ITableStorage
    {
        CloudTableClient TableClient { get;set; }
    }
}
