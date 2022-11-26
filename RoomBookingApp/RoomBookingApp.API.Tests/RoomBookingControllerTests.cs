


namespace RoomBookingApp.API.Tests
{
	public class RoomBookingControllerTests
	{
        private Mock<IRoomBookingRequestProcessor> _roomBookingProcessor;
        private RoomBookingController _controller;
        private RoomBookingRequest _request;
        private RoomBookingResult _result;

        public RoomBookingControllerTests()
		{
			_roomBookingProcessor= new Mock<IRoomBookingRequestProcessor>();
			_controller = new RoomBookingController(_roomBookingProcessor.Object);
			_request = new RoomBookingRequest();
			_result = new RoomBookingResult();

			_roomBookingProcessor.Setup(x => x.BookRoom(_request))
				.Returns(_result);
		}

		[Theory]
		[InlineData(1, true, typeof(OkObjectResult))]
        [InlineData(0, false, typeof(BadRequestObjectResult))]
		public async Task Should_Call_Booking_Method_When_Valid(
			int expectedMethodCalls,
			bool isModelValid,
			Type expectedActionResultType)
		{
			if(!isModelValid)
			{
				_controller.ModelState.AddModelError("Key", "ErrorMessage");
			}

			var result = await _controller.BookRoom(_request);

			result.ShouldBeOfType(expectedActionResultType);
			_roomBookingProcessor.Verify(x => x.SaveBooking(_request), Times.Exactly(expectedMethodCalls));
		}
    }
}

