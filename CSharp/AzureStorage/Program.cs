using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureStorage
{
    class Program
    {

        static void Main(string[] args)
        {
            _ExecuteTableStorageOperations();
        }

        private static void _ExecuteTableStorageOperations()
        {
            var tableStorageOperations = new TableStorageOperations();
            const string usersTableName = "UsersTable";
            tableStorageOperations.CreateTable(usersTableName);

            var numberOfUsersToCreate = new Random().Next(1, 10);

            for (var currentUser = 1; currentUser <= numberOfUsersToCreate; ++currentUser)
            {
                tableStorageOperations.InsertUser(usersTableName, currentUser);
            }

            tableStorageOperations.DisplayAllUsers(usersTableName);
            tableStorageOperations.DeleteTable(usersTableName);
        }
    }
}
