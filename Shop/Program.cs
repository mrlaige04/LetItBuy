using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Services;
using Microsoft.AspNetCore.Identity;
using Shop.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddDbContext<ApplicationDBContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDBContext>();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=GetWelcomePage}/{id?}");


app.Run();
