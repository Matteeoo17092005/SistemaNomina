// /Data/Entities/Title.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Nomina.Data.Entities;
public class Title
{
    [Column("emp_no")]
    public int EmpNo { get; set; }
    [Column("title")]
    public string TitleName { get; set; } = default!;
    [Column("from_date")]
    public DateTime FromDate { get; set; }
    [Column("to_date")]
    public DateTime? ToDate { get; set; }

    public Employee Employee { get; set; } = default!;
}