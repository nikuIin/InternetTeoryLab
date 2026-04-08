using Microsoft.EntityFrameworkCore;
using WineShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// PostgreSQL via EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".WineShop.Session";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Для localhost
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // ВАЖНО: UseSession должен быть после UseRouting и до UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    for (int i = 0; i < 10; i++)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            logger.LogWarning("БД не готова, попытка {i}/10...", i + 1);
            Thread.Sleep(3000);
        }
    }
}

app.Run();
