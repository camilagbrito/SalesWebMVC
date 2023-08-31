﻿using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

string mySqlConnection = builder.Configuration
    .GetConnectionString("SalesWebMVCContext");

builder.Services.AddDbContext<SalesWebMVCContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.Parse("10.4.28 - MariaDB")));
    
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
