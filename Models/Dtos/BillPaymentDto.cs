namespace Midterm.Models.Dtos
{
    public class BillPaymentDto
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
    }
}

