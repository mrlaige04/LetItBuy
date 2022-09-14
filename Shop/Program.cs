using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Services;
using Microsoft.AspNetCore.Identity;
using Shop.Models;
using Microsoft.Extensions.FileProviders;
//using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();



builder.Services.AddRazorPages();

// DB and Identity
builder.Services.AddDbContext<ApplicationDBContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => {
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();



// Custom Services
builder.Services.AddSingleton<ICustomEmailSender, GmailSmtpSender>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<AdminService>();
builder.Services.AddTransient<AdminInitializer>();


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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=GetWelcomePage}/{id?}");

app.MapRazorPages();

app.Run();
