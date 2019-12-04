using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PerformanceTrakEmail.Test
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

        public static DefaultHttpRequest CreateHttpRequest(string body)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(body)
            };

            return request;
        }

        public static ILogger CreateLogger()
        {
            ILogger logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            return logger;
        }
    }
}