// /Domain/BcryptCrypto.cs
using BCrypt.Net;

namespace App.Nomina.Domain;
public class BcryptCrypto : IUserCrypto
{
    public string Hash(string plain) => BCrypt.Net.BCrypt.HashPassword(plain);
    public bool Verify(string plain, string hash) => BCrypt.Net.BCrypt.Verify(plain, hash);
}
