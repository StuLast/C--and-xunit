using RoomBookingApp.Domain;

namespace RoomBookingApp.Core.DataServices
{
    public interface IRoomBookingService
	{
		public void Save(RoomBooking roomBooking);
		public IEnumerable<Room> GetAvailableRooms(DateTime date);
	}
}
