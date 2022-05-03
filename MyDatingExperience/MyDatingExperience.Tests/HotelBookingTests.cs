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
    public class HotelBookingTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private const string Url = "HotelBookings/";

        public HotelBookingTests(WebApplicationFactory<Program> factory)
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
        public async Task GivenClientDeviceTimeZoneIsUsed_WhenGetHotelBooking_ThenShowDateBasedOnCurrentClientDeviceTimeZone()
        {
            // Arrange
            var bookingDate = new DateTime(2022, 05, 21);
            var clientDeviceTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");

            var request = new HotelBookingRequest
            {
                Name = "Rose",
                BookingDate = TimeZoneInfo.ConvertTimeToUtc(bookingDate, clientDeviceTimeZone),
            };

            HttpRequestMessage createMessage = new HttpRequestMessage(HttpMethod.Post, Url);
            createMessage.Content = new ObjectContent<HotelBookingRequest>(request, new JsonMediaTypeFormatter());
            var createResponse = await _client.SendAsync(createMessage);
            var bookingResponse = await createResponse.Content.ReadAsAsync<HotelBookingResponse>();
            var dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(bookingResponse.BookingDate, clientDeviceTimeZone);
            // Correct date when the device time zone is in Sydney
            Assert.Equal("2022-05-21", dateShownToUser.ToString("yyyy-MM-dd"));
            // Rose arriving to New York
            clientDeviceTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

            // Act
            HttpRequestMessage getMessage = new HttpRequestMessage(HttpMethod.Get, Url + bookingResponse.BookingId);
            var getResponse = await _client.SendAsync(getMessage);
            bookingResponse = await getResponse.Content.ReadAsAsync<HotelBookingResponse>();
            dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(bookingResponse.BookingDate, clientDeviceTimeZone);

            // Assert
            Assert.Equal("2022-05-20", dateShownToUser.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public async Task GivenHotelTimeZoneIsUsed_WhenGetHotelBooking_ThenAlwaysShowDateWithTheSameTimeZone()
        {
            // Arrange
            var bookingDate = new DateTime(2022, 05, 21);
            var hotelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

            var request = new HotelBookingRequest
            {
                Name = "Rose",
                BookingDate = TimeZoneInfo.ConvertTimeToUtc(bookingDate, hotelTimeZone),
            };

            HttpRequestMessage createMessage = new HttpRequestMessage(HttpMethod.Post, Url);
            createMessage.Content = new ObjectContent<HotelBookingRequest>(request, new JsonMediaTypeFormatter());
            var createResponse = await _client.SendAsync(createMessage);
            var bookingResponse = await createResponse.Content.ReadAsAsync<HotelBookingResponse>();
            var dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(bookingResponse.BookingDate, hotelTimeZone);
            Assert.Equal("2022-05-21", dateShownToUser.ToString("yyyy-MM-dd"));

            // Act
            HttpRequestMessage getMessage = new HttpRequestMessage(HttpMethod.Get, Url + bookingResponse.BookingId);
            var getResponse = await _client.SendAsync(getMessage);
            bookingResponse = await getResponse.Content.ReadAsAsync<HotelBookingResponse>();
            dateShownToUser = TimeZoneInfo.ConvertTimeFromUtc(bookingResponse.BookingDate, hotelTimeZone);

            // Assert
            Assert.Equal("2022-05-21", dateShownToUser.ToString("yyyy-MM-dd"));
        }
    }
}
