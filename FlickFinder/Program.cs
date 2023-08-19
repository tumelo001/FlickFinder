using FlickFinder.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlickFinder.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IWrapperRepository, WrapperRepository>();
builder.Services.AddDbContext<AppDbContext>(options => 
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
/*builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbConnection")));*/


builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
	options.User.RequireUniqueEmail = true;	
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRouting(options =>
{
	options.AppendTrailingSlash = true;
	options.LowercaseUrls = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "details",
    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData.EnsureDataPopulated(app);
SeedIdentityData.EnsureIdentityDataPopulated(app);

app.Run();
