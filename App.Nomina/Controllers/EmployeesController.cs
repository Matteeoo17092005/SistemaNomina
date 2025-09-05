// /Controllers/EmployeesController.cs
using App.Nomina.Data;
using App.Nomina.Data.Entities;
using App.Nomina.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Nomina.Controllers;

[Authorize] // proteger controladores según rol
public class EmployeesController : Controller
{
    private readonly NominaDbContext _db;
    private readonly ISalaryService _salaryService;
    public EmployeesController(NominaDbContext db, ISalaryService salaryService)
    {
        _db = db;
        _salaryService = salaryService;
    }

    public async Task<IActionResult> Index(string? q, int page = 1, int pageSize = 20)
    {
        var query = _db.Employees.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(e => e.FirstName.Contains(q) || e.LastName.Contains(q) || e.Ci.Contains(q) || e.Correo.Contains(q));
        }
        var total = await query.CountAsync();
        var data = await query.OrderBy(e => e.EmpNo)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        ViewBag.Total = total; ViewBag.Page = page; ViewBag.PageSize = pageSize; ViewBag.Q = q;
        return View(data);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,RRHH")]
    public IActionResult Create() => View(new Employee());

    [HttpPost]
    [Authorize(Roles = "Administrador,RRHH")]
    public async Task<IActionResult> Create(Employee model)
    {
        if (!ModelState.IsValid) return View(model);

        // Validar CI único
        if (await _db.Employees.AnyAsync(e => e.Ci == model.Ci))
        {
            ModelState.AddModelError("Ci", "Ya existe un empleado con esta cédula");
            return View(model);
        }

        // Validar correo único
        if (await _db.Employees.AnyAsync(e => e.Correo == model.Correo))
        {
            ModelState.AddModelError("Correo", "Ya existe un empleado con este correo");
            return View(model);
        }

        _db.Employees.Add(model);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador,RRHH")]
    public async Task<IActionResult> Edit(int id)
    {
        var e = await _db.Employees.FindAsync(id);
        if (e == null) return NotFound();
        return View(e);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador,RRHH")]
    public async Task<IActionResult> Edit(Employee model)
    {
        if (!ModelState.IsValid) return View(model);

        // Validar CI único (excluyendo el registro actual)
        if (await _db.Employees.AnyAsync(e => e.Ci == model.Ci && e.EmpNo != model.EmpNo))
        {
            ModelState.AddModelError("Ci", "Ya existe un empleado con esta cédula");
            return View(model);
        }

        // Validar correo único (excluyendo el registro actual)
        if (await _db.Employees.AnyAsync(e => e.Correo == model.Correo && e.EmpNo != model.EmpNo))
        {
            ModelState.AddModelError("Correo", "Ya existe un empleado con este correo");
            return View(model);
        }

        _db.Update(model);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador,RRHH")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var e = await _db.Employees.FindAsync(id);
        if (e == null) return NotFound();
        e.Activo = false;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador,RRHH")]
    public async Task<IActionResult> Activate(int id)
    {
        var e = await _db.Employees.FindAsync(id);
        if (e == null) return NotFound();
        e.Activo = true;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var e = await _db.Employees.FindAsync(id);
        if (e == null) return NotFound();
        var salarioActual = await _salaryService.GetSalaryOnAsync(id, DateTime.Today);
        ViewBag.SalarioActual = salarioActual;
        return View(e);
    }

    [Authorize]
    public async Task<IActionResult> Salaries(int id)
    {
        var e = await _db.Employees.FindAsync(id);
        if (e == null) return NotFound();
        var salarios = await _db.Salaries
            .Where(s => s.EmpNo == id)
            .OrderByDescending(s => s.FromDate)
            .ToListAsync();
        ViewBag.Empleado = e;
        return View(salarios);
    }
}
