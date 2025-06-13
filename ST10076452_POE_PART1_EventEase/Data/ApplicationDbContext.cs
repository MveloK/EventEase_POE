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
        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<BookingDetailsViewModel> BookingDetailsView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookingsModel>()
                .HasKey(b => new { b.BookingID, b.EventId, b.VenueId });

            modelBuilder.Entity<BookingsModel>()
                .HasOne(b => b.Events)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingsModel>()
                .HasOne(b => b.Venues)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BookingDetailsViewModel>()
                .HasNoKey()
                .ToView("BookingDetailsView");
        }
    }
}

