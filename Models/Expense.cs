namespace TravelSplitAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int PaidByParticipantId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}