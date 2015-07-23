using System;
using StackExchange.Redis;

namespace RedisCaching
{
    static class Program
    {
        private const string CacheKey = "Some Key";
        private const string CacheValue = "Some value for redis cache";

        static void Main(string[] args)
        {
            try
            {
                ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost");
                IDatabase database = connection.GetDatabase();

                _SetAndGetCacheValue(database);
                _AppendToCacheValue(database);
            }
            catch (Exception ex)
            {
                ConsoleColor oldForgroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error {ex.Message}");
            }
        }

        private static void _AppendToCacheValue(IDatabase database)
        {
            const string appendString = " appended";
            database.StringAppend(CacheValue, appendString);
            var retrievedValue = database.StringGet(CacheKey);
            var expected = $"{CacheKey}{appendString}";
            Console.WriteLine($"AppendToCacheValue: Expected = {expected}, Actual = {retrievedValue}");
        }

        private static void _SetAndGetCacheValue(IDatabase database)
        {
            database.StringSet(CacheKey, CacheValue);

            RedisValue retrievedValue = database.StringGet(CacheKey);
            Console.WriteLine($"Expected value = {CacheValue}, Retrieved value = {retrievedValue}");
        }
    }
}
