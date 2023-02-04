using Microsoft.EntityFrameworkCore;
using TKGMParsel.Business.Repositories;
using TKGMParsel.Data.Cache;
using TKGMParsel.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration.GetConnectionString("WebCS");
builder.Services.AddDbContext<Context>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(WebRepository<>));
builder.Services.AddSingleton(typeof(DataCacheRedis));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
