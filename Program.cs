using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Data;
using SalesWebMVC.Services;
using System.Configuration;
var builder = WebApplication.CreateBuilder(args);

// builder antigo \/
//builder.Services.AddDbContext<SalesWebMVCContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMVCContext"), new MySqlServerVersion(new Version()) , builder =>
//	builder.MigrationsAssembly("SalesWebMVC")));

var connectionStringMysql = builder.Configuration.GetConnectionString("SalesWebMvcContext");
builder.Services.AddDbContext<SalesWebMVCContext>(options => options.UseMySql(connectionStringMysql, ServerVersion.Parse("8.0.25-mysql")));

// Solução do erro em options.MySql (cs1503) tirada de:
// https://stackoverflow.com/questions/66720614/cannot-convert-from-string-to-microsoft-entityframeworkcore-serverversion
// onde sugere adicionar new MySqlServerVersion(new Version()) como argumento

builder.Services.AddScoped<SeedingService>();

builder.Services.AddScoped<SellerService>();

builder.Services.AddScoped<DepartmentService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
