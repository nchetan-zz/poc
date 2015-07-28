using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SqlDependencyCallback
{
    static class Program
    {
        // NOTE: This program assumes "ProofOfConceptDb" is deployed on localmachine.
        private const string GetUsersSqlQuery = "SELECT Id, UserName FROM [User]";
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ProofOfConceptDb"].ConnectionString;
        private static SqlConnection _connection;

        static void Main(string[] args)
        {
            _StartSqlDependency();

            try
            {
                _connection = new SqlConnection(ConnectionString);
                _connection.Open();
                _RegisterDependency();
                Console.WriteLine("Press any key to exit....");
                Console.ReadKey();
            }
            finally
            {
                _EndSqlDependency();
                _connection?.Dispose();
                _connection = null;
            }
        }

        private static void _RegisterDependency()
        {
            using (var sqlCommand = new SqlCommand(GetUsersSqlQuery, _connection))
            {
                var sqlDependency = new SqlDependency(sqlCommand);
                sqlDependency.OnChange += new OnChangeEventHandler(SqlDependencyOnChange);

                using (var dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var id = dataReader.GetInt32(0);
                        var userName = dataReader.GetString(1);
                        Console.WriteLine($"Id = {id}, UserName = {userName}");
                    }
                }
            }
        }

        // NOTE: If you recieve Subscribe notification immediatly after execution, it's possibly because of "The statement is not valid for notificationThe statement is not valid for notification"
        // For more information read http://www.codeproject.com/Articles/12335/Using-SqlDependency-for-data-change-events
        private static void SqlDependencyOnChange(object sender, SqlNotificationEventArgs eventArgs)
        {
            Console.WriteLine($"Notification recieved. Source = {eventArgs.Source}, Type = {eventArgs.Type}, Info = {eventArgs.Info}");
            if (eventArgs.Type != SqlNotificationType.Change) return;

            _RegisterDependency();
        }

        private static void _StartSqlDependency()
        {
            SqlDependency.Start(ConnectionString);
        }

        private static void _EndSqlDependency()
        {
            SqlDependency.Stop(ConnectionString);
        }
    }
}
