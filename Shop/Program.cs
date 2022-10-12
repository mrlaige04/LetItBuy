using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using Shop.Hubs;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using Shop.BLL.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Shop.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Shop.Core;
using Shop.Core.Providers;
using Shop.Core.Providers.Interfaces;
using Shop.UI.Clients.APICLIENTS;
using Shop.UI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// MVC Services
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.AddRazorPages();
builder.Services.AddSignalR();



// DB and Identity
builder.Services.AddDbContext<ApplicationDBContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("Shop.UI")));

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.User.AllowedUserNameCharacters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz¿‡¡·¬‚√„ƒ‰≈Â®∏∆Ê«Á»Ë…È ÍÀÎÃÏÕÌŒÓœÔ–—Ò“Ú”Û‘Ù’ı÷ˆ◊˜ÿ¯Ÿ˘⁄˙€˚‹¸›˝ﬁ˛ﬂˇ≤≥Øø";
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders()
.AddErrorDescriber<MultiLanguageErrorDescriber>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(x =>
{
    x.LoginPath = "/Account/google-login";
})
.AddTwitter(x=>
{
    x.ConsumerKey = builder.Configuration["Auth:Twitter:ConsumerKey"];
    x.ConsumerSecret = builder.Configuration["Auth:Twitter:ConsumerSecret"];
})
.AddGoogle(config =>
{
    config.SignInScheme = IdentityConstants.ExternalScheme;
    config.ClientId = builder.Configuration["Auth:Google:ClientId"];
    config.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
}).AddFacebook(x =>
{
    x.SignInScheme = IdentityConstants.ExternalScheme;
    x.AppId = builder.Configuration["Auth:Facebook:AppId"];
    x.AppSecret = builder.Configuration["Auth:Facebook:AppSecret"];
}).AddMicrosoftAccount(x =>
{
    x.ClientId = builder.Configuration["Auth:Microsoft:ClientId"];
    x.ClientSecret = builder.Configuration["Auth:Microsoft:ClientSecret"];  
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
builder.Services.AddScoped<ICustomEmailSender, GmailSmtpSender>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AccountService>(); 
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AdminInitializer>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PhotoService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<FilterService>();
builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<ItemApiClient>();


// Logging and Configuration
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("emailsmtpconfig.json");
builder.Configuration.AddJsonFile("admininitialize.json");
builder.Configuration.AddJsonFile("secrets.json");


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
app.MapHub<ContactHub>("/contact");

app.MapRazorPages();

app.Run();
