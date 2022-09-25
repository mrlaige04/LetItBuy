using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Shop.Data;
using Shop.Errors;
using Shop.Models;
using Shop.Services;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using Shop.Hubs;
//using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// MVC Services
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();



// DB and Identity
builder.Services.AddDbContext<ApplicationDBContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders()
.AddErrorDescriber<MultiLanguageErrorDescriber>();


builder.Services.AddAuthentication()
    .AddCookie(x =>
    {
        x.ExpireTimeSpan = TimeSpan.FromMinutes(30);  
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("uk"),
        new CultureInfo("de")
    };
    options.DefaultRequestCulture = new RequestCulture("ua");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});



//Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


// Custom Services
builder.Services.AddSingleton<ICustomEmailSender, GmailSmtpSender>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<AdminService>();
builder.Services.AddTransient<AdminInitializer>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<PhotoService>();
builder.Services.AddScoped<CategoryService>(); 



// Logging and Configuration
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("emailsmtpconfig.json");
builder.Configuration.AddJsonFile("admininitialize.json");

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var adminInitializer = services.GetRequiredService<AdminInitializer>();
    await adminInitializer.InitializeAdminAsync();
}
app.UseRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=GetWelcomePage}/{id?}");

app.MapHub<ChatHub>("/chatHub");

app.MapRazorPages();

app.Run();
