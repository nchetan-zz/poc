using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using SharedCode;

namespace MemoryCaching
{
    class Program
    {
        private static string _contentFilePath;

        static void Main(string[] args)
        {
            using (var tempDirectory = new TempDirectory())
            {
                _contentFilePath = Path.Combine(tempDirectory.FullPath, "FileToMonitor.txt");
                _GenerateFileToMonitor(tempDirectory);
                _SetupMemoryCacheForTest();

                var answer = ' ';
                while (answer != 'q' && answer != 'Q')
                {
                    Console.WriteLine("Press q to quit...");
                    answer = Console.ReadKey().KeyChar;
                }
            }
        }

        private static void _GenerateFileToMonitor(TempDirectory tempDirectory)
        {
            File.WriteAllText(_contentFilePath, "Original contents of file.");
            Console.WriteLine("File to be monitored = {0}", _contentFilePath);
        }

        private static void _SetupMemoryCacheForTest()
        {
            var memoryCache = MemoryCache.Default;

            var fileContents = File.ReadAllText(_contentFilePath);
            var cacheItem = new CacheItem(_contentFilePath, fileContents);
            var cacheItemPolicy = _GetCacheItemPolicy(_contentFilePath);
            memoryCache.Set(cacheItem, cacheItemPolicy);
        }

        private static void CacheEntryUpdateCallback(CacheEntryUpdateArguments arguments)
        {
            Console.WriteLine("Received {0} notification for {1}", arguments.RemovedReason, arguments.Key);
            if (!File.Exists(arguments.Key)) return;

            arguments.UpdatedCacheItem = _GetCacheItem(arguments.Key);
            arguments.UpdatedCacheItemPolicy = _GetCacheItemPolicy(arguments.Key);
        }

        private static CacheItemPolicy _GetCacheItemPolicy(string path)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30),
                UpdateCallback = CacheEntryUpdateCallback,
            };

            // Setup change monitoring for file.
            var filePaths = new List<string> { _contentFilePath };
            cacheItemPolicy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));
            return cacheItemPolicy;
        }

        private static CacheItem _GetCacheItem(string path)
        {
            var cacheValue = File.ReadAllText(_contentFilePath);
            return new CacheItem(path, cacheValue);
        }
    }
}
