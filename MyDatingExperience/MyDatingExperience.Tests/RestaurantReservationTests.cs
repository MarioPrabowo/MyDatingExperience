using Microsoft.AspNetCore.Mvc.Testing;
using MyDatingExperience.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyDatingExperience.Tests
{
    public class RestaurantReservationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private const string Url = "RestaurantReservations/";

        public RestaurantReservationTests(WebApplicationFactory<Program> factory)
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
        public async Task GivenTimeZoneIsNotConvertedInClient_WhenGetHotelBooking_ThenShowWrongTime()
        {
            // Arrange
            var bookingDate = new DateTime(2022, 05, 21, 18, 0, 0);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/Melbourne");

            var request = new RestaurantReservationsRequest
            {
                Name = "Annie",
                BookingDate = TimeZoneInfo.ConvertTimeToUtc(bookingDate, timeZone),
            };

            HttpRequestMessage createMessage = new HttpRequestMessage(HttpMethod.Post, Url);
            createMessage.Content = new ObjectContent<RestaurantReservationsRequest>(request, new JsonMediaTypeFormatter());
            var createResponse = await _client.SendAsync(createMessage);
            var bookingResponse = await createResponse.Content.ReadAsAsync<RestaurantReservationsResponse>();
            var dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(bookingResponse.BookingDate, timeZone);
            Assert.Equal("2022-05-21 18:00:00", dateShownToUser.ToString("yyyy-MM-dd HH:mm:ss"));

            // Act
            HttpRequestMessage getMessage = new HttpRequestMessage(HttpMethod.Get, Url + bookingResponse.BookingId);
            var getResponse = await _client.SendAsync(getMessage);
            bookingResponse = await getResponse.Content.ReadAsAsync<RestaurantReservationsResponse>();
            dateShownToUser = bookingResponse.BookingDate;

            // Assert
            Assert.Equal("2022-05-21 08:00:00", dateShownToUser.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [Fact]
        public async Task GivenTimeZoneIsConvertedCorrectlyInClient_WhenGetHotelBooking_ThenShowCorrectReservationTime()
        {
            // Arrange
            var bookingDate = new DateTime(2022, 05, 21, 18, 0, 0);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/Melbourne");

            var request = new RestaurantReservationsRequest
            {
                Name = "Annie",
                BookingDate = TimeZoneInfo.ConvertTimeToUtc(bookingDate, timeZone),
            };

            HttpRequestMessage createMessage = new HttpRequestMessage(HttpMethod.Post, Url);
            createMessage.Content = new ObjectContent<RestaurantReservationsRequest>(request, new JsonMediaTypeFormatter());
            var createResponse = await _client.SendAsync(createMessage);
            var bookingResponse = await createResponse.Content.ReadAsAsync<RestaurantReservationsResponse>();
            var dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(bookingResponse.BookingDate, timeZone);
            Assert.Equal("2022-05-21 18:00:00", dateShownToUser.ToString("yyyy-MM-dd HH:mm:ss"));

            // Act
            HttpRequestMessage getMessage = new HttpRequestMessage(HttpMethod.Get, Url + bookingResponse.BookingId);
            var getResponse = await _client.SendAsync(getMessage);
            bookingResponse = await getResponse.Content.ReadAsAsync<RestaurantReservationsResponse>();
            dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(bookingResponse.BookingDate, timeZone);

            // Assert
            Assert.Equal("2022-05-21 18:00:00", dateShownToUser.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
