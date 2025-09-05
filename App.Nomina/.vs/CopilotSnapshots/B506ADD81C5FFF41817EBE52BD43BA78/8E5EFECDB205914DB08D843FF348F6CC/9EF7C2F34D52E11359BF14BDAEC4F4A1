using System.ComponentModel.DataAnnotations.Schema;

namespace App.Nomina.Data.Entities;
[Table("log_auditoriasalarios")]
public class LogAuditoriaSalarios
{
    [Column("id")]
    public int Id { get; set; }
    [Column("usuario")]
    public string Usuario { get; set; } = default!;
    [Column("fecha_actualizacion")]
    public DateTime FechaActualizacion { get; set; }
    [Column("detalle_cambio")]
    public string DetalleCambio { get; set; } = default!;
    [Column("salario")]
    public decimal Salario { get; set; }
    [Column("emp_no")]
    public int EmpNo { get; set; }
}
