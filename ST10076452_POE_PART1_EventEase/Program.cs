using Microsoft.EntityFrameworkCore;
using ST10076452_POE_PART1_EventEase.Data;

var builder = WebApplication.CreateBuilder(args);

// Register the database context with the Azure SQL connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(context);  // <- renamed from SeedBookings
}

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Venues}/{id?}");

app.Run();
