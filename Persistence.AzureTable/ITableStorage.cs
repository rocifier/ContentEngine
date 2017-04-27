using Microsoft.WindowsAzure.Storage.Table;

namespace ContentEngine.Persistence.Azure
{
    public interface ITableStorage
    {
        CloudTableClient TableClient { get;set; }
    }
}
