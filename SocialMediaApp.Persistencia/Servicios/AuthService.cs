using System.Security.Cryptography;

namespace SocialMediaApp.Dominio.Interfaces
{
    public class AuthService : IAuthService
    {
        private const int saltSize = 128 / 8; // 128 bits
        private const int keySize = 256 / 8; // 256 bits
        private const int iterations = 100000;
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

        public string CreateHashPassword(string password, out string salt)
        {
            salt = GenerateSalt();

            return HashPassword(password, salt);
        }

        public string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            string computedHash = HashPassword(password, salt);
            return computedHash == hashedPassword;
        }

        public string GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            return Convert.ToBase64String(salt);
        }

    }
}
