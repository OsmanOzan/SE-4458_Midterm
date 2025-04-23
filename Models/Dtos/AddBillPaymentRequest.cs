namespace Midterm.Models.Dtos
{
    public class AddBillPaymentRequest
    {
        public int BillId { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
