using System.IO;
using Microsoft.Azure.WebJobs;

namespace WebJobTest
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var jobHost = new JobHost();
            jobHost.RunAndBlock();
        }

        public static void CopyContentToOutput([BlobTrigger("input/{name}")] Stream inputStream,
            [BlobTrigger("output/{name}")] Stream outputStream)
        {
            var streamWriter = new StreamWriter(outputStream);
            streamWriter.WriteLine("42");
        }
    }
}
