using SocialMediaApp.Persistencia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.Interfaces
{
    public interface IAuth
    {
        public Task<int> Register(Usuario user);       

        public Task<Usuario?> getByUsername(string username);

        public Task<Usuario?> getByEmail(string email);

        public Task<int> ChangePassword(Usuario user);

        public Task<int> ChangeQuestion(ChangeQuestionRequest changeQuestion);

        public Task<int> ChangeProfile(Profile updates);

    }
}
