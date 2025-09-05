using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// /Data/Entities/Employee.cs
namespace App.Nomina.Data.Entities;
public class Employee
{
    [Column("emp_no")]
    public int EmpNo { get; set; }                 // PK

    [Required(ErrorMessage = "La c�dula es obligatoria")]
    [StringLength(20, ErrorMessage = "La c�dula no puede tener m�s de 20 caracteres")]
    [Column("ci")]
    public string Ci { get; set; } = default!;     // �nico

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [Column("birth_date")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener m�s de 50 caracteres")]
    [Column("first_name")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(50, ErrorMessage = "El apellido no puede tener m�s de 50 caracteres")]
    [Column("last_name")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "El g�nero es obligatorio")]
    [StringLength(1)]
    [Column("gender")]
    public string Gender { get; set; } = default!; // M/F/O

    [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
    [Column("hire_date")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato v�lido")]
    [Column("correo")]
    public string Correo { get; set; } = default!; // �nico y v�lido

    [Column("activo")]
    public bool Activo { get; set; } = true;

    public ICollection<DeptEmp> DeptEmp { get; set; } = new List<DeptEmp>();
    public ICollection<Title> Titles { get; set; } = new List<Title>();
    public ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
