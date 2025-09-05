// /Controllers/AuthController.cs
using App.Nomina.Data;
using App.Nomina.Data.Entities;
using App.Nomina.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Nomina.Controllers;

public class AuthController : Controller
{
    private readonly NominaDbContext _db;
    private readonly IUserCrypto _crypto;
    public AuthController(NominaDbContext db, IUserCrypto crypto)
    {
        _db = db; _crypto = crypto;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !_crypto.Verify(password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Credenciales inválidas");
            return View();
        }
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Rol)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    public IActionResult Denied() => View();
}
