using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using Moq;

namespace ECP.Master.tests
{
    public static class TestHelpers
    {
        // Helper function to help mock document queries
        public interface IFakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T>
        {
        }

        public static HttpRequestMessage SetupHttp(HttpRequestMessage requestMessage)
        {
            var services = new Mock<IServiceProvider>(MockBehavior.Strict);
            var formatter = new XmlMediaTypeFormatter();
            var context = new DefaultHttpContext();

            var contentNegotiator = new Mock<IContentNegotiator>();
            contentNegotiator
                .Setup(c => c.Negotiate(It.IsAny<Type>(), It.IsAny<HttpRequestMessage>(), It.IsAny<IEnumerable<MediaTypeFormatter>>()))
                .Returns(new ContentNegotiationResult(formatter, mediaType: null));

            var options = new WebApiCompatShimOptions();

            if (formatter == null)
            {
                options.Formatters.AddRange(new MediaTypeFormatterCollection());
            }
            else
            {
                options.Formatters.Add(formatter);
            }

            var optionsAccessor = new Mock<IOptions<WebApiCompatShimOptions>>();
            optionsAccessor.SetupGet(o => o.Value).Returns(options);

            services.Setup(s => s.GetService(typeof(IOptions<WebApiCompatShimOptions>))).Returns(optionsAccessor.Object);

            if (contentNegotiator != null)
            {
                services.Setup(s => s.GetService(typeof(IContentNegotiator))).Returns(contentNegotiator);
            }

            context.RequestServices = CreateServices(contentNegotiator.Object, formatter);
            requestMessage.Properties.Add(nameof(HttpContext), context);
            return requestMessage;
        }

        // Helper function that mocks a document query
        public static Mock<IFakeDocumentQuery<T>> GetMockDocumentQuery<T>(IQueryable<T> dataSource, FeedResponse<T> response)
        {
            var mockDocumentQuery = new Mock<IFakeDocumentQuery<T>>();
            mockDocumentQuery
                .SetupSequence(_ => _.HasMoreResults)
                .Returns(true)
                .Returns(false);

            mockDocumentQuery
                .Setup(_ => _.ExecuteNextAsync<T>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var provider = new Mock<IQueryProvider>();

            provider
            .Setup(_ => _.CreateQuery<T>(It.IsAny<Expression>()))
            .Returns(mockDocumentQuery.Object);

            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.Provider).Returns(provider.Object);
            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.Expression).Returns(() => dataSource.Expression);
            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(() => dataSource.ElementType);
            mockDocumentQuery.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => dataSource.GetEnumerator());

            return mockDocumentQuery;
        }

        private static IServiceProvider CreateServices(
            IContentNegotiator contentNegotiator = null,
            MediaTypeFormatter formatter = null)
        {
            var options = new WebApiCompatShimOptions();

            if (formatter == null)
            {
                options.Formatters.AddRange(new MediaTypeFormatterCollection());
            }
            else
            {
                options.Formatters.Add(formatter);
            }

            var optionsAccessor = new Mock<IOptions<WebApiCompatShimOptions>>();
            optionsAccessor.SetupGet(o => o.Value).Returns(options);

            var services = new Mock<IServiceProvider>(MockBehavior.Strict);
            services.Setup(s => s.GetService(typeof(IOptions<WebApiCompatShimOptions>))).Returns(optionsAccessor.Object);

            if (contentNegotiator != null)
            {
                services.Setup(s => s.GetService(typeof(IContentNegotiator))).Returns(contentNegotiator);
            }

            return services.Object;
        }
    }
}
