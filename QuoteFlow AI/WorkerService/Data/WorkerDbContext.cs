using Microsoft.EntityFrameworkCore;
using WorkerService.Models;

namespace WorkerService.Data
{
    public class WorkerDbContext : DbContext
    {
        public WorkerDbContext(DbContextOptions<WorkerDbContext> options) : base(options) { }

        public DbSet<FollowUp> FollowUps => Set<FollowUp>();
        public DbSet<Quotation> Quotations => Set<Quotation>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FollowUp>()
                .HasIndex(f => new { f.QuotationId, f.FollowUpNumber })
                .IsUnique();
        }
    }
}