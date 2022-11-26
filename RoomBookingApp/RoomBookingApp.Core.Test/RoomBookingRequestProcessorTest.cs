using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;
using RoomBookingApp.Core.Enums;
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
				new Room(){ Id= 1 }
			};

			_roomBookingServiceMock = new Mock<IRoomBookingService>();
			_roomBookingServiceMock.Setup(rbs => rbs.GetAvailableRooms(_request.Date))
				.Returns(_availableRooms);

			_processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
		}


		[Fact]
		public void Should_Return_Room_Booking_Response_With_Request_Values()
		{

			//Action
			RoomBookingResult result = _processor.BookRoom(_request);

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

			var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
			exception.ParamName.ShouldBe("bookingRequest");

		}

		[Fact]
		public void Should_Save_Room_Booking_Request()
		{
			RoomBooking savedBooking = null;
			_roomBookingServiceMock.Setup(rbs => rbs.Save(It.IsAny<RoomBooking>()))
				.Callback<RoomBooking>(booking => {
					savedBooking = booking;
				});

			_processor.BookRoom(_request);

			_roomBookingServiceMock.Verify(
				q => q.Save(It.IsAny<RoomBooking>()), Times.Once);


			//With Shouldly
			savedBooking.ShouldNotBeNull();
			savedBooking.FullName.ShouldBe(_request.FullName);
			savedBooking.Email.ShouldBe(_request.Email);
			savedBooking.Date.ShouldBe(_request.Date);
			savedBooking.RoomId.ShouldBe(_availableRooms.First().Id);

		}

		[Fact]
		public void Should_Not_Save_Room_Booking_Request_If_None_Available()
		{
			_availableRooms.Clear();
			_processor.BookRoom(_request);
			_roomBookingServiceMock.Verify(
				q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
		}

		[Theory]
		[InlineData(BookingResultFlag.Failure, false)]
		[InlineData(BookingResultFlag.Success, true)]
		public void Should_Return_SuccessFailure_Flag_In_Result(BookingResultFlag bookingResultFlag, bool isAvailable)
		{
			if(!isAvailable)
			{
				_availableRooms.Clear();
			}

			var result = _processor.BookRoom(_request);
			bookingResultFlag.ShouldBe(result.Flag);
			
		}

		[Theory]
		[InlineData(1, true)]
		[InlineData(null, false)]
		public void Should_Return_RoomBookingId_In_Result (int? roomBookingId, bool isAvailable)
		{
			if(!isAvailable)
			{
				_availableRooms.Clear();
			} 
			else
			{
				_roomBookingServiceMock.Setup(rbs => rbs.Save(It.IsAny<RoomBooking>()))
					.Callback<RoomBooking>(booking => {
						booking.Id = roomBookingId.Value;
				});
			}

			var result = _processor.BookRoom(_request);
			result.RoomBookingId.ShouldBe(roomBookingId);
		}
	}
}
