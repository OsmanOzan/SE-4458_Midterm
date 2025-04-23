namespace Midterm.Models.Dtos
{
    public class BillDto
    {
        public int Id { get; set; }
        public int SubscriberNo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int PhoneMinutesUsed { get; set; }
        public int InternetUsedMb { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}

