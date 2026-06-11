using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuoteFlowAI.Models;

namespace QuoteFlowAI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Quotation> Quotations => Set<Quotation>();
        public DbSet<FollowUp> FollowUps => Set<FollowUp>();
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Optional: ensure Identity tables use standard names (no passkeys table created)
            builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            builder.Entity<User>().ToTable("AspNetUsers");

            builder.Entity<Quotation>()
                .HasIndex(q => q.QuoteNumber)
                .IsUnique();

            builder.Entity<FollowUp>()
                .HasIndex(f => new { f.QuotationId, f.FollowUpNumber })
                .IsUnique();

            builder.Entity<Quotation>()
                .HasOne(q => q.Customer)
                .WithMany(c => c.Quotations)
                .HasForeignKey(q => q.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FollowUp>()
                .HasOne(f => f.Quotation)
                .WithMany(q => q.FollowUps)
                .HasForeignKey(f => f.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.FollowUp)
                .WithMany(f => f.Messages)
                .HasForeignKey(m => m.FollowUpId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}