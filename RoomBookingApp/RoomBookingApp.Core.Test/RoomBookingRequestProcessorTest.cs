using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Core
{
	public class RoomBookingRequestProcessorTest
	{
		[Fact]
		public void Should_Return_Room_Booking_Response_With_Request_Values()
		{
			//Arrange
			var request = new RoomBookingRequest
			{
				FullName = "Test Name",
				Email = "test@request.com",
				Date = new DateTime(2021, 10, 20)
			};

			var processor = new RoomBookingRequestProcessor();

			//Action
			RoomBookingResult result = processor.Bookroom(request);

			//Assertion
			//With default xUnit Assert
			Assert.NotNull(result);
			Assert.Equal(result.FullName, request.FullName);
			Assert.Equal(result.Email, request.Email);
			Assert.Equal(result.Date, request.Date);


			//With Shouldly
			//result.ShouldNotBeNull();
			//result.FullName.ShouldBe(request.FullName);
			//result.Email.ShouldBe(request.Email);
			//result.Date.ShouldBe(request.Date);
		}
	}
}
