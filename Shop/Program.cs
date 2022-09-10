using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Services;
using Microsoft.AspNetCore.Identity;
using Shop.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ICustomEmailSender, GmailSmtpSender>();

builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDBContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Logging.AddConsole();
builder.Configuration.AddJsonFile("emailsmtpconfig.json");

    
builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.SignIn.RequireConfirmedEmail = true;   
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    })
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=GetWelcomePage}/{id?}");

app.MapRazorPages();

app.Run();
