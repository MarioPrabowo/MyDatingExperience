using Microsoft.AspNetCore.Mvc.Testing;
using MyDatingExperience.DTOs;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace MyDatingExperience.Tests
{
    public class FlightBookingTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private const string Url = "FlightBookings/";
        private const string NonAmbiguousDateFormat = "yyyy-MM-dd";

        public FlightBookingTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GivenAmbiguousDateFormatInRequest_WhenCreateFlightBooking_ThenReturnWrongDate()
        {
            // Arrange
            var bookingDate = new DateTime(2022, 06, 10);
            var request = new FlightBookingRequest
            {
                Name = "Jack",
                // Short date format in US culture. 
                BookingDate = bookingDate.ToString("d", CultureInfo.GetCultureInfo("en-us")),
            };

            // Act
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Url);
            message.Content = new ObjectContent<FlightBookingRequest>(request, new JsonMediaTypeFormatter());
            var response = await _client.SendAsync(message);

            // Assert
            var result = await response.Content.ReadAsAsync<FlightBookingResponse>();
            Assert.Equal("2022-10-06",result.BookingDate.ToString(NonAmbiguousDateFormat));
        }

        [Fact]
        public async Task GivenNonAmbiguousDateFormatInRequest_WhenCreateFlightBooking_ThenReturnCorrectDate()
        {
            // Arrange
            var bookingDate = new DateTime(2022, 06, 10);
            var request = new FlightBookingRequest
            {
                Name = "Jack",
                BookingDate = bookingDate.ToString(NonAmbiguousDateFormat),
            };

            // Act
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Url);
            message.Content = new ObjectContent<FlightBookingRequest>(request, new JsonMediaTypeFormatter());
            var response = await _client.SendAsync(message);

            // Assert
            var result = await response.Content.ReadAsAsync<FlightBookingResponse>();
            Assert.Equal("2022-06-10", result.BookingDate.ToString(NonAmbiguousDateFormat));
        }
    }
}