using Core;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Setup the database
if (builder.Environment.IsEnvironment("testing"))
{
    var connection = new SqliteConnection("Data Source=:memory:");
    connection.Open();
    builder.Services.AddSingleton(connection);

    builder.Services.AddDbContext<ChessDbContext>((sp, options) =>
    {
        var conn = sp.GetRequiredService<SqliteConnection>();
        options.UseSqlite(conn);
    });
} else
{
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ChessDbContext>(options =>
        options.UseSqlite(connectionString, b => b.MigrationsAssembly("Infrastructure")));
}


builder.Services.AddDefaultIdentity<User>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = 
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.:;_@+ æøåÆØÅäöüÄÖÜßéèêÉÈÊ";
    })
    .AddEntityFrameworkStores<ChessDbContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ChessDbContext>();

    if (builder.Environment.IsEnvironment("testing"))
    {
        dbContext.Database.EnsureCreated();
    }
    else
    {
        dbContext.Database.Migrate();
    }

    // Seed initial application data
    if(!app.Environment.IsProduction())
    {
        DbInitializer.SeedDatabase(dbContext);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

//Exposes program for integration testing
public partial class Program { }