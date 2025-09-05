// /Domain/DeptManagerService.cs
using App.Nomina.Data;
using App.Nomina.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Nomina.Domain;

public class DeptManagerService : IDeptManagerService
{
    private readonly NominaDbContext _db;
    public DeptManagerService(NominaDbContext db) => _db = db;

    public async Task AssignManagerAsync(string deptNo, int empNo, DateTime fromDate, DateTime? toDate)
    {
        if (toDate.HasValue && toDate < fromDate)
            throw new ArgumentException("to_date no puede ser anterior a from_date.");

        // Validar único manager activo por fecha
        bool otroManagerActivo = await _db.DeptManagers
            .Where(dm => dm.DeptNo == deptNo)
            .AnyAsync(dm => DateRange.Overlaps(dm.FromDate, dm.ToDate, fromDate, toDate));
        if (otroManagerActivo) throw new InvalidOperationException("Ya existe un manager activo en esa vigencia.");

        _db.DeptManagers.Add(new DeptManager { DeptNo = deptNo, EmpNo = empNo, FromDate = fromDate, ToDate = toDate });
        await _db.SaveChangesAsync();
    }
}
