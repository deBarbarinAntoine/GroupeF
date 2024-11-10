using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using YellowDirectory.Models;

// Fetch the environment variables from the .env file
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

// Check if there is an ADMIN_PASSWORD environment variable
if (Environment.GetEnvironmentVariable("ADMIN_PASSWORD") == null)
    Environment.Exit(1);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Set the connection to the PostgreSQL database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var dataSourceBuilder = new NpgsqlDataSourceBuilder($"Host={Environment.GetEnvironmentVariable("DB_HOST")};Username={Environment.GetEnvironmentVariable("DB_USER")};Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};Database={Environment.GetEnvironmentVariable("DB_NAME")};SSL Mode=Disable;Trust Server Certificate=true");

    options.UseNpgsql(dataSourceBuilder.Build());
});

// Add the User management service
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Create a Scope to execute the migrations
builder.Services.AddScoped<SeedData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    
    // DEBUG
    // YamlDump.DumpAsYaml(Environment.GetEnvironmentVariables());
}

// Uncomment to activate the HTTPS protocol & redirection
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the migrations
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var seedData = scope.ServiceProvider.GetRequiredService<SeedData>();

await dbContext.Database.MigrateAsync();
await seedData.SeedAsync(dbContext);

// Launch the server
app.Run();