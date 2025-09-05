using System.ComponentModel.DataAnnotations.Schema;

namespace App.Nomina.Data.Entities;
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("emp_no")]
    public int? EmpNo { get; set; } // opcionalmente vinculado

    [Column("username")]
    public string Username { get; set; } = default!;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = default!;

    [Column("rol")]
    public string Rol { get; set; } = "RRHH"; // Administrador | RRHH
}
