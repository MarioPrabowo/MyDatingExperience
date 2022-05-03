using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using MyDatingExperience.DTOs;
using MyDatingExperience.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace MyDatingExperience.Tests
{
    public class InvoiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private const string Url = "Invoices/";
        private readonly Mock<IDateTimeService> _mockDateTimeService;

        public InvoiceTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _mockDateTimeService = new Mock<IDateTimeService>();
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IDateTimeService>(_mockDateTimeService.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GivenTimeZoneIsDoneUsingOffset_WhenSendInvoiceDuringDst_ThenShowWrongSentTime()
        {
            // Arrange
            var currentTime = new DateTime(2022, 02, 06, 00, 55, 0);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/Melbourne");
            var currentTimeUtc = TimeZoneInfo.ConvertTimeToUtc(currentTime, timeZone);
            _mockDateTimeService.Setup(s => s.GetUtcNow()).Returns(currentTimeUtc);
            var melbourneTimeOffsetFromUtc = 10;

            var request = new SendInvoiceRequest
            {
                Amount = 10,
                Sender = "Natasha"
            };

            // Act
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Url);
            message.Content = new ObjectContent<SendInvoiceRequest>(request, new JsonMediaTypeFormatter());
            var response = await _client.SendAsync(message);
            var sentInvoice = await response.Content.ReadAsAsync<SendInvoiceResponse>();
            var dateShownToUser = sentInvoice.SentDate.AddHours(melbourneTimeOffsetFromUtc);
            
            // Assert
            Assert.Equal("2022-02-05 23:55:00", dateShownToUser.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [Fact]
        public async Task GivenTimeZoneIsDoneUsingIana_WhenSendInvoiceDuringDst_ThenShowCorrectSentTime()
        {
            // Arrange
            var currentTime = new DateTime(2022, 02, 06, 00, 55, 0);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/Melbourne");
            var currentTimeUtc = TimeZoneInfo.ConvertTimeToUtc(currentTime, timeZone);
            _mockDateTimeService.Setup(s => s.GetUtcNow()).Returns(currentTimeUtc);

            var request = new SendInvoiceRequest
            {
                Amount = 10,
                Sender = "Natasha"
            };

            // Act
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Url);
            message.Content = new ObjectContent<SendInvoiceRequest>(request, new JsonMediaTypeFormatter());
            var response = await _client.SendAsync(message);
            var sentInvoice = await response.Content.ReadAsAsync<SendInvoiceResponse>();
            var dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(sentInvoice.SentDate, timeZone);

            // Assert
            Assert.Equal("2022-02-06 00:55:00", dateShownToUser.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
