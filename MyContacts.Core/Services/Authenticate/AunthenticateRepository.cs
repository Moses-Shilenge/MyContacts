using Microsoft.EntityFrameworkCore;
using MyContacts.API.Models;
using MyContacts.SqlClient;
using MyContacts.SqlClient.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyContacts.Core.Services.Aunthenticate
{
    public class AunthenticateRepository : EntityFrameworkRepository<ContactDto, Guid>, IAunthenticateRepository
    {
        protected MyContactsContext _context;
        public AunthenticateRepository(MyContactsContext context) : base(context)
        {
            _context = context;
        }

        public Task<MyContactsUser> ValidateLoginCredsAsync(LoginViewModel login)
        {
            var passwordEncrypted = GenerateSHA512String(login.Password);

            return Task.FromResult(_context.Users.Where(u => u.PasswordHash == passwordEncrypted).FirstOrDefault());
        }

        private string GenerateSHA512String(string password)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetstringFromHash(hash);
        }

        private string GetstringFromHash(byte[] hash)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString());
            }
            return builder.ToString();
        }
    }
}