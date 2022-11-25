using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public interface IRoomBookingRequestProcessor
    {
        RoomBookingResult Bookroom(RoomBookingRequest bookingRequest);
        void SaveBooking(RoomBookingRequest request);
    }
}