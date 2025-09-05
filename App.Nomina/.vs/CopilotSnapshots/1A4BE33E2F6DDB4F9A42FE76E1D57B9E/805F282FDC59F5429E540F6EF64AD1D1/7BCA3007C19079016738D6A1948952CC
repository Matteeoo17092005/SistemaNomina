// /Controllers/SalariesController.cs
using App.Nomina.Data;
using App.Nomina.Data.Entities;
using App.Nomina.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Nomina.Controllers;

[Authorize(Roles = "Administrador,RRHH")]
public class SalariesController : Controller
{
    private readonly ISalaryService _salaryService;
    private readonly NominaDbContext _db;
    public SalariesController(ISalaryService salaryService, NominaDbContext db)
    {
        _salaryService = salaryService;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int empNo)
    {
        var empleado = await _db.Employees.FindAsync(empNo);
        if (empleado == null) return NotFound();
        if (!empleado.Activo)
        {
            TempData["Error"] = "No se puede asignar salario a un empleado inactivo.";
            return RedirectToAction("Details", "Employees", new { id = empNo });
        }
        var historial = await _db.Salaries
            .Where(s => s.EmpNo == empNo)
            .OrderByDescending(s => s.FromDate)
            .ToListAsync();
        ViewBag.Empleado = empleado;
        ViewBag.Historial = historial;
        return View(model: empNo);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int empNo, decimal amount, DateTime fromDate, DateTime? to_date)
    {
        var usuario = User.Identity?.Name ?? "sistema";
        var empleado = await _db.Employees.FindAsync(empNo);
        if (empleado == null) return NotFound();
        if (!empleado.Activo)
        {
            TempData["Error"] = "No se puede asignar salario a un empleado inactivo.";
            var historial = await _db.Salaries
                .Where(s => s.EmpNo == empNo)
                .OrderByDescending(s => s.FromDate)
                .ToListAsync();
            ViewBag.Empleado = empleado;
            ViewBag.Historial = historial;
            return View(empNo);
        }
        try
        {
            await _salaryService.AddOrChangeSalaryAsync(empNo, amount, fromDate, usuario, to_date);
            return RedirectToAction("Details", "Employees", new { id = empNo });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var historial = await _db.Salaries
                .Where(s => s.EmpNo == empNo)
                .OrderByDescending(s => s.FromDate)
                .ToListAsync();
            ViewBag.Empleado = empleado;
            ViewBag.Historial = historial;
            return View(empNo);
        }
    }
}
