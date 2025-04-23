using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midterm.Models
{
    [Table("Usage")]
    public class Usage
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

        [Column("usage_type")]
        public string UsageType { get; set; }  

        [Column("amount")]
        public int Amount { get; set; }

        public Subscriber Subscriber { get; set; }
    }
}
