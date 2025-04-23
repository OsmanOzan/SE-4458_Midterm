using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Midterm.Models
{
    [Table("Subscribers")]
    public class Subscriber
    {
        [Key]
        [Column("subscriber_no")]
        public int SubscriberNo { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public ICollection<Usage> Usages { get; set; }
        public ICollection<Bill> Bills { get; set; }
    }
}
