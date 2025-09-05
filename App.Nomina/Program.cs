using App.Nomina.Data;
using App.Nomina.Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// EF Core - Usar MySQL
builder.Services.AddDbContext<NominaDbContext>(opt =>
    opt.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Auth por Cookies (simple)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Auth/Login";
        opt.AccessDeniedPath = "/Auth/Denied";
    });

builder.Services.AddHttpContextAccessor();

// Servicios de dominio
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IDeptEmpService, DeptEmpService>();
builder.Services.AddScoped<IDeptManagerService, DeptManagerService>();
builder.Services.AddScoped<IUserCrypto, BcryptCrypto>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NominaDbContext>();
    var crypto = scope.ServiceProvider.GetRequiredService<IUserCrypto>();
    if (!db.Users.Any(u => u.Username == "Mateo"))
    {
        var user = new App.Nomina.Data.Entities.User
        {
            Username = "Mateo",
            PasswordHash = crypto.Hash("Admin123"),
            Rol = "Administrador"
        };
        db.Users.Add(user);
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();
