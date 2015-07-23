using System;
using System.Threading;
using StackExchange.Redis;

namespace RedisCaching
{
    static class Program
    {
        private const string StringCacheKey = "Some Key";
        private const string StringCacheValue = "Some value for redis cache";
        public const int IntCacheKey = 0xF00D;
        public const int IntCacheValue = 42;

        static void Main(string[] args)
        {
            try
            {
                ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost");
                IDatabase database = connection.GetDatabase();

                _SetAndGetStringCacheValue(database);
                _AppendToCacheValue(database);

                _LockingCacheAndAccess(database);
               
                _SetAndGetIntCacheValue(database);

            }
            catch (Exception ex)
            {
                ConsoleColor oldForgroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error {ex.Message}");
            }
        }

        private static void _LockingCacheAndAccess(IDatabase database)
        {
            var thread1 = new Thread(_LockingCacheAndAccessInThread);
            var thread2 = new Thread(_LockingCacheAndAccessInThread);

            thread1.Start(new RedisCacheLockThreadState
            {
                ThreadNumber = 1,
                Database = database,
            });

            thread2.Start(new RedisCacheLockThreadState
            {
                ThreadNumber = 2,
                Database = database,
            });

            thread1.Join();
            thread2.Join();
        }

        private static void _LockingCacheAndAccessInThread(object obj)
        {
            var threadState = (RedisCacheLockThreadState) obj;
            var database = (IDatabase) threadState.Database;
            var token = Environment.MachineName;
            var lockValidityTimespan = TimeSpan.MaxValue;

            if (!database.LockTake(StringCacheKey, token, lockValidityTimespan))
            {
                Console.WriteLine($"Thread Number {threadState.ThreadNumber}: could not get database lock.");
                return;
            }

            try
            {
                database.StringAppend(StringCacheKey, "Locking append");
                // Simulate a long running function by sleeping 5 seconds
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught. Message = {ex.Message}");
            }
            finally
            {
                database.LockRelease(StringCacheKey, token);
            }
        }

        private static void _SetAndGetIntCacheValue(IDatabase database)
        {
            throw new NotImplementedException();
        }

        private static void _AppendToCacheValue(IDatabase database)
        {
            const string appendString = " appended";
            database.StringAppend(StringCacheValue, appendString);
            var retrievedValue = database.StringGet(StringCacheKey);
            var expected = $"{StringCacheKey}{appendString}";
            Console.WriteLine($"AppendToCacheValue: Expected = {expected}, Actual = {retrievedValue}");
        }

        private static void _SetAndGetStringCacheValue(IDatabase database)
        {
            database.StringSet(StringCacheKey, StringCacheValue);

            var retrievedValue = database.StringGet(StringCacheKey);
            Console.WriteLine($"Expected value = {StringCacheValue}, Retrieved value = {retrievedValue}");
        }
    }
}
