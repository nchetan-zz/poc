using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageOperations
{
    public interface ITableStorageRepository<T> where T : TableEntity
    {
        void Put(T entity);
        T Get(string rowKey);
        void CreateTable();
    }

    public class TableStorageRepository<T> : ITableStorageRepository<T> where T : TableEntity
    {
        CloudTable m_cloudTable;

        public TableStorageRepository()
        {
            var storageAcount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudTableClient    tableClient = storageAcount.CreateCloudTableClient();

            m_cloudTable = tableClient.GetTableReference("Person");
        }

        public void Put(T entity)
        {
            TableBatchOperation tableBatchOperation = new TableBatchOperation();

            tableBatchOperation.InsertOrReplace(entity);
            var tableResults = m_cloudTable.ExecuteBatch(tableBatchOperation);
            if (tableResults == null)
                throw new ApplicationException("Insert failed");

            var insertResult = tableResults.Single();
            var isOperationSuccessful = !(insertResult.HttpStatusCode < 200 || insertResult.HttpStatusCode > 299);
            if (!isOperationSuccessful)
            {
                throw new ApplicationException(String.Format("Failed to create. Status = {0}", insertResult.HttpStatusCode));
            }
        }

        public T Get(string rowKey)
        {
            var tableBatchOperation = new TableBatchOperation();
            
            tableBatchOperation.Retrieve("Person", rowKey);
            var tableResults = m_cloudTable.ExecuteBatch(tableBatchOperation);

            if (tableResults == null)
                throw new ApplicationException();

            var retrieveResult = tableResults.Single();
            if (retrieveResult.HttpStatusCode != 200)
            {
                throw new ApplicationException(String.Format("Failed to retrieve. Status = {0}", retrieveResult.HttpStatusCode));
            }

            return retrieveResult.Result as T;
        }

        public void CreateTable()
        {
            m_cloudTable.CreateIfNotExists();
        }
    }
}
