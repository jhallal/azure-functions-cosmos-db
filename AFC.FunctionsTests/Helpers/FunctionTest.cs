using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AFC.Functions.Tests
{
    public abstract class FunctionTest
    {
        protected ILogger logger;

        public FunctionTest()
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            logger = loggerFactory.CreateLogger<FunctionTest>();
        }

        public HttpRequest HttpRequestSetup()
        {
            var reqMock = new Mock<HttpRequest>();

            reqMock.Setup(req => req.HttpContext).Returns(new DefaultHttpContext());
            return reqMock.Object;
        }

        public HttpRequest HttpRequestSetup(Dictionary<String, StringValues> query, string body)
        {
            var reqMock = new Mock<HttpRequest>();

            reqMock.Setup(req => req.Query).Returns(new QueryCollection(query));
            reqMock.Setup(req => req.HttpContext).Returns(new DefaultHttpContext());
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            reqMock.Setup(req => req.Body).Returns(stream);
            return reqMock.Object;
        }
    }

    public class AsyncCollector<T> : IAsyncCollector<T>
    {
        public readonly List<T> Items = new List<T>();

        public Task AddAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {

            Items.Add(item);

            return Task.FromResult(true);
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }
    }
}
