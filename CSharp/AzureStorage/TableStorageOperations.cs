using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace AzureStorage
{
    public class TableStorageOperations
    {
        private CloudStorageAccount _cloudStorageAccount;
        private static Guid _partitionKey = Guid.NewGuid();
        private static Random random = new Random();
        public TableStorageOperations()
        {
            //var cloudStorageConnectionString =
            //    CloudConfigurationManager.GetSetting(CloudConfigurationName.DefaultStorageConnectionString);
            var cloudStorageConnectionString =
                ConfigurationManager.ConnectionStrings[CloudConfigurationName.DefaultStorageConnectionString].ConnectionString;            _cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
        }

        public void CreateTable(string tableName)
        {
            var tableReference = _GetTableReference(tableName);

            if (!tableReference.CreateIfNotExists())
            {
                Console.WriteLine($"Did not create {tableName}");
            }
        }

        public void InsertUser(string tableName, int currentUser)
        {
            var rowKey = random.Next();

            var user = new Customer
            {
                ETag = "*", // For now blindly over write (as we expect this to be creation only)
                Id = rowKey,
                Name = $"User {currentUser}",
                RowKey = rowKey.ToString(),
                PartitionKey = _partitionKey.ToString(),
            };

            var tableOperation = TableOperation.Insert(user);
            var batchOperation = new TableBatchOperation {tableOperation};
            _ExecuteBatch(batchOperation, tableName);
            Console.WriteLine($"Inserted Customer with Id = {rowKey}");
        }

        private void _ExecuteBatch(TableBatchOperation batchOperation, string tabelName)
        {
            var tableReference = _GetTableReference(tabelName);
            tableReference.ExecuteBatch(batchOperation);
        }

        private CloudTable _GetTableReference(string tableName)
        {
            var tableClient = _cloudStorageAccount.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(tableName);
            return tableReference;
        }

        public void DisplayAllUsers(string usersTableName)
        {
            var tableClient = _cloudStorageAccount.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(usersTableName);

            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _partitionKey.ToString());

            var tableQuery = new TableQuery<Customer>()
                .Where(partitionKeyFilter);

            var customers = tableReference.ExecuteQuery(tableQuery);
            foreach (var customer in customers)
            {
                Console.WriteLine("========");
                Console.WriteLine($"Id = {customer.Id}, Name = {customer.Name}");
            }

        }

        public void DeleteTable(string tableName)
        {
            var tableClient = _cloudStorageAccount.CreateCloudTableClient();
            var tableReference = tableClient.GetTableReference(tableName);
            var deleteSuccess = tableReference.DeleteIfExists();
            if (!deleteSuccess)
            {
                Console.WriteLine($"Failed to delete table {tableName}");
            }
        } 
    }

    public class Customer : TableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}