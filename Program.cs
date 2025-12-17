using GiftLab.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http.Metadata;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GiftLabDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();


app.MapGet("/debug/routes", (EndpointDataSource dataSource) =>
{
    var lines = dataSource.Endpoints
        .OfType<RouteEndpoint>()
        .Select(e =>
        {
            var methods = e.Metadata.OfType<HttpMethodMetadata>()
                .FirstOrDefault()?.HttpMethods;

            var methodText = methods == null ? "ANY" : string.Join(",", methods);
            return $"{methodText,-10} {e.RoutePattern.RawText,-30} => {e.DisplayName}";
        })
        .OrderBy(x => x);

    return Results.Text(string.Join("\n", lines));
});
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
