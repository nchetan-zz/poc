using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageOperations
{
    class Application
    {
        public static void Main()
        {
            ITableStorageRepository<Person> personRespository = new TableStorageRepository<Person>();
            
            Person userToStore = new Person
            {
                Id = 1,
                Name = "Chetan",
                PartitionKey = "Person",
                RowKey = string.Format("{0}-{1}", 1, "Chetan"),
            };

            personRespository.CreateTable();
            personRespository.Put(userToStore);

            var retrievedUser = personRespository.Get(userToStore.RowKey);
        }
    }
}
