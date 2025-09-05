// /Domain/SalaryService.cs
using App.Nomina.Data;
using App.Nomina.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Nomina.Domain;

public class SalaryService : ISalaryService
{
    private readonly NominaDbContext _db;
    public SalaryService(NominaDbContext db) => _db = db;

    // Salario básico Ecuador 2024 (actualizar según año)
    private const decimal SalarioBasicoEcuador = 460.00m;

    public async Task AddOrChangeSalaryAsync(int empNo, decimal amount, DateTime fromDate, string usuario, DateTime? toDate)
    {
        if (amount < SalarioBasicoEcuador)
            throw new ArgumentException($"El salario no puede ser menor al salario básico de Ecuador (${SalarioBasicoEcuador:N2}).");
        if (amount <= 0) throw new ArgumentException("Salario inválido.");
        // Paso 1: Cerrar salario vigente si existe
        var vigente = await _db.Salaries
            .Where(s => s.EmpNo == empNo && (s.ToDate == null || s.ToDate >= fromDate))
            .OrderByDescending(s => s.FromDate)
            .FirstOrDefaultAsync();

        if (vigente != null && vigente.FromDate <= fromDate && (vigente.ToDate == null || vigente.ToDate >= fromDate))
        {
            vigente.ToDate = fromDate.AddDays(-1);
            await _db.SaveChangesAsync(); // Guardar solo el cierre
        }

        // Verificar no solapamiento con otros registros
        bool solapa = (await _db.Salaries
            .Where(s => s.EmpNo == empNo)
            .ToListAsync())
            .Any(s => DateRange.Overlaps(s.FromDate, s.ToDate, fromDate, toDate));
        if (solapa) throw new InvalidOperationException("Existe un salario que se solapa con la nueva vigencia.");

        // Paso 2: Insertar nuevo salario
        var nuevoSalario = new Salary { EmpNo = empNo, Amount = amount, FromDate = fromDate, ToDate = toDate };
        _db.Salaries.Add(nuevoSalario);

        // Auditoría
        _db.LogAuditoriaSalarios.Add(new LogAuditoriaSalarios
        {
            Usuario = usuario,
            FechaActualizacion = DateTime.UtcNow,
            DetalleCambio = $"Nuevo salario desde {fromDate:yyyy-MM-dd}",
            Salario = amount,
            EmpNo = empNo
        });

        await _db.SaveChangesAsync(); // Guardar el nuevo salario y auditoría
    }

    public async Task<decimal?> GetSalaryOnAsync(int empNo, DateTime date)
    {
        var s = await _db.Salaries
            .Where(x => x.EmpNo == empNo && x.FromDate <= date && (x.ToDate == null || x.ToDate >= date))
            .OrderByDescending(x => x.FromDate)
            .FirstOrDefaultAsync();
        return s?.Amount;
    }
}
