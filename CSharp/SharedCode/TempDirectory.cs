using System;
using System.IO;

namespace SharedCode
{
    public class TempDirectory : IDisposable
    {
        public string FullPath { get; private set; }

        public TempDirectory()
        {
            var guid = Guid.NewGuid();
            var directoryName = guid.ToString("N");
            var tempPath = Path.GetTempPath();
            FullPath = Path.Combine(tempPath, directoryName);

            Directory.CreateDirectory(FullPath);
        }

        public void Dispose()
        {
            Directory.Delete(FullPath, true);
        }
    }
}