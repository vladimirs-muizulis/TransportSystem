using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using TransportSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Adding Local Database for the application (using in-memory storage for transport data)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=TransportDb.db"));


// Adding support for sessions
builder.Services.AddDistributedMemoryCache(); // Using memory-based cache to store session data
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout (inactive session timeout)
    options.Cookie.HttpOnly = true; // Make session cookie HttpOnly for security
    options.Cookie.IsEssential = true; // Make the session cookie essential for the app to function
});

// Add MVC controllers and views support
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// Adding Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Enable Swagger for API documentation
app.UseSwagger();
app.UseSwaggerUI();

// Enable static file support (for serving CSS, JS, images, etc.)
app.UseStaticFiles();

// Enable session middleware to manage user session data
app.UseSession();

// Map controllers and set up route for MVC (default route for Home controller)
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();