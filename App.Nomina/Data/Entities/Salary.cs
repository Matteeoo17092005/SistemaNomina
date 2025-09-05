using System.ComponentModel.DataAnnotations.Schema;

// /Data/Entities/Salary.cs
namespace App.Nomina.Data.Entities;
public class Salary
{
    [Column("emp_no")]
    public int EmpNo { get; set; }
    [Column("salary")]
    public decimal Amount { get; set; }
    [Column("from_date")]
    public DateTime FromDate { get; set; }
    [Column("to_date")]
    public DateTime? ToDate { get; set; }

    public Employee Employee { get; set; } = default!;
}
