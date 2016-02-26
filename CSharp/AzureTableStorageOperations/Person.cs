using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorageOperations
{
    public class Person : TableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
