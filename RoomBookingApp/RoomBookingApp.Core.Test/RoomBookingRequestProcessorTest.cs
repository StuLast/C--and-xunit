using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core
{
	public class RoomBookingRequestProcessorTest
	{
		private RoomBookingRequestProcessor _processor;
		private RoomBookingRequest _request;
		private Mock<IRoomBookingService> _roomBookingServiceMock;
		private List<Room> _availableRooms;

		public RoomBookingRequestProcessorTest()
		{

			_request = new RoomBookingRequest
			{
				FullName = "Test Name",
				Email = "test@request.com",
				Date = new DateTime(2021, 10, 20)
			};

			_availableRooms = new List<Room> {
				new Room()
			};

			_roomBookingServiceMock = new Mock<IRoomBookingService>();
			_roomBookingServiceMock.Setup(q => q.GetAvailableRooms(_request.Date))
				.Returns(_availableRooms);

			_processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);


		}


		[Fact]
		public void Should_Return_Room_Booking_Response_With_Request_Values()
		{

			//Action
			RoomBookingResult result = _processor.Bookroom(_request);

			//Assertion
			//With default xUnit Assert
			//Assert.NotNull(result);
			//Assert.Equal(result.FullName, request.FullName);
			//Assert.Equal(result.Email, request.Email);
			//Assert.Equal(result.Date, request.Date);


			//With Shouldly
			result.ShouldNotBeNull();
			result.FullName.ShouldBe(_request.FullName);
			result.Email.ShouldBe(_request.Email);
			result.Date.ShouldBe(_request.Date);
		}

		[Fact]
		public void Should_Throw_Exception_For_Null_Request()
		{

			//var exception = Assert.Throws<ArgumentNullException>(() => processor.Bookroom(null));

			var exception = Should.Throw<ArgumentNullException>(() => _processor.Bookroom(null));
			exception.ParamName.ShouldBe("bookingRequest");

		}

		[Fact]
		public void Should_Save_Room_Booking_Request()
		{
			RoomBooking savedBooking = null;
			_roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
				.Callback<RoomBooking>(booking => {
					savedBooking = booking;
				});

			_processor.Bookroom(_request);

			_roomBookingServiceMock.Verify(
				q => q.Save(It.IsAny<RoomBooking>()), Times.Once);


			//With Shouldly
			savedBooking.ShouldNotBeNull();
			savedBooking.FullName.ShouldBe(_request.FullName);
			savedBooking.Email.ShouldBe(_request.Email);
			savedBooking.Date.ShouldBe(_request.Date);

		}

		[Fact]
		public void Should_Not_Save_Room_Booking_Request_If_None_Available()
		{
			_availableRooms.Clear();
			_processor.Bookroom(_request);
			_roomBookingServiceMock.Verify(
				q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
		}
	}
}
