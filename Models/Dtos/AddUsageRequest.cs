namespace Midterm.Models.Dtos
{
    public class AddUsageRequest
    {
        public int SubscriberNo { get; set; }
        public int Month { get; set; } 
        public required string UsageType { get; set; }

    }
}
