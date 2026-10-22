using TravelSplitAPI.Models;

namespace TravelSplitAPI.Data
{
    public static class TripStore
    {
        public static readonly List<Trip> Trips = new List<Trip>
        {
            new Trip
            {
                Id = 1,
                Name = "金廈小三通",
                Destination = "金門、廈門",
                StartDate = new DateTime(2026, 11, 14),
                EndDate = new DateTime(2026, 11, 16)
            },
            new Trip
            {
                Id = 2,
                Name = "台南美食之旅",
                Destination = "台南",
                StartDate = new DateTime(2026, 12, 5),
                EndDate = new DateTime(2026, 12, 6)
            }
        };
    }
}