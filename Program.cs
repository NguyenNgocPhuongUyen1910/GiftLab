using Microsoft.EntityFrameworkCore;
using GiftLab.Data;

var builder = WebApplication.CreateBuilder(args);

// =====================
// ADD SERVICES
// =====================

// MVC
builder.Services.AddControllersWithViews();

// EF Core - SQL Server
builder.Services.AddDbContext<GiftLabDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// =====================
// MIDDLEWARE
// =====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// =====================
// ROUTING
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

