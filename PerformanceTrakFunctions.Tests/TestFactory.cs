using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PerformanceTrakFunctions.Tests
{
    public class TestFactory
    {
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static DefaultHttpRequest CreateHttpRequest(string body) => new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = GenerateStreamFromString(body)
        };

        public static ILogger CreateLogger() => NullLoggerFactory.Instance.CreateLogger("Null Logger");
    }
}
