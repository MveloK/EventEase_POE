using Microsoft.EntityFrameworkCore;
using ST10076452_POE_PART1_EventEase.Models;

namespace ST10076452_POE_PART1_EventEase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VenueModel> Venues { get; set; }
        public DbSet<EventsModel> Events { get; set; }
        public DbSet<BookingsModel> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define Composite Primary Key for Bookings
            modelBuilder.Entity<BookingsModel>()
                .HasKey(b => new { b.BookingID, b.EventId, b.VenueId });

            // Configure Foreign Key: EventId -> Events
            modelBuilder.Entity<BookingsModel>()
                .HasOne(b => b.Events)
                .WithMany() // Use WithMany(e => e.Bookings) if there's a navigation property in EventsModel
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Foreign Key: VenueId -> Venues
            modelBuilder.Entity<BookingsModel>()
                .HasOne(b => b.Venues)
                .WithMany() // Use WithMany(v => v.Bookings) if there's a navigation property in VenueModel
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
