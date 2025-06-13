using ST10076452_POE_PART1_EventEase.Data;
using ST10076452_POE_PART1_EventEase.Models;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Seed EventTypes
        if (!context.EventTypes.Any())
        {
            context.EventTypes.AddRange(
                new EventType { Name = "Conference" },
                new EventType { Name = "Wedding" },
                new EventType { Name = "Concert" }
            );
            context.SaveChanges();
        }


    }
}
