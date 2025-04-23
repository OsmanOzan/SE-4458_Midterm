namespace Midterm.Models.Dtos
{
    public class UsageDto
    {
        public int Id { get; set; }
        public int SubscriberNo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string UsageType { get; set; }
        public int Amount { get; set; }
    }
}
