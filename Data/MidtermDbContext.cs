using Microsoft.EntityFrameworkCore;
using Midterm.Models;

namespace Midterm.Data
{
    public class MidtermDbContext : DbContext
    {
        public MidtermDbContext(DbContextOptions<MidtermDbContext> options) : base(options) { }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Usage> Usage { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillPayment> BillPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usage>()
                .HasOne(u => u.Subscriber)
                .WithMany(s => s.Usages)
                .HasForeignKey(u => u.SubscriberNo);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Subscriber)
                .WithMany(s => s.Bills)
                .HasForeignKey(b => b.SubscriberNo);

            modelBuilder.Entity<BillPayment>()
                .HasOne(p => p.Bill)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BillId);
        }
    }
}
