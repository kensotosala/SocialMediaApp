using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IAuthService
    {
        public string CreateHashPassword(string password, out string salt);

        public string HashPassword(string password, string salt);

        public bool VerifyPassword(string password, string hashedPassword, string salt);
        
        public string GenerateSalt();
    }
}
