using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midterm.Models
{
    [Table("BillPayments")]
    public class BillPayment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("bill_id")]
        public int BillId { get; set; }

        [Column("payment_date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Column("amount_paid")]
        public decimal AmountPaid { get; set; }

        public Bill Bill { get; set; }
    }
}
