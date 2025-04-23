using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Midterm.Models
{
    [Table("Bills")]
    public class Bill
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("subscriber_no")]
        public int SubscriberNo { get; set; }

        [Column("year")]
        public int Year { get; set; }

        [Column("month")]
        public int Month { get; set; }

        [Column("phone_minutes_used")]
        public int PhoneMinutesUsed { get; set; }

        [Column("internet_used_mb")]
        public int InternetUsedMb { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("is_paid")]
        public bool IsPaid { get; set; }

        public Subscriber Subscriber { get; set; }
        public ICollection<BillPayment> Payments { get; set; }
    }
}
