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

namespace MyContacts.Core.Services.Contacts
{
    public class ContactsRepository : EntityFrameworkRepository<ContactDto, Guid>, IContactsRepository
    {
        protected MyContactsContext _context;
        public ContactsRepository(MyContactsContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactDto>> GetAll()
        {
            return await Task.FromResult(_context.Contacts.ToList());
        }

        public async Task<ContactDto> GetUserWithEmail(string email)
        {
            return await Task.FromResult(_context.Contacts.Where(c => c.Email == email).FirstOrDefault());
        }

        public async Task<string> GetUsernameWithEmailAsync(string email)
        {
            return await Task.FromResult(_context.Users.Where(u => u.Email == email).Select(u => u.UserName).FirstOrDefault());
        }

        public Task<ContactDto> UpdateContactAsync(ContactDto contactToUpdate)
        {
            var contact = GetByIdAsync(contactToUpdate.Id);
            var user = _context.Users.Where(u => u.Email == contact.Result.Email).FirstOrDefault();

            contact.Result.Email = contactToUpdate.Email;
            contact.Result.PhoneNumber = contactToUpdate.PhoneNumber;
            user.PhoneNumber = contactToUpdate.PhoneNumber;
            user.Email = contactToUpdate.Email;
            user.UserName = contactToUpdate.User.UserName;

            _context.SaveChangesAsync();

            return Task.FromResult(contactToUpdate);
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
                builder.Append(hash[i].ToString("X2")); // Appends letters and numbers 
            }
            return builder.ToString();
        }

        public async Task<string> RegisterUserAsync(MyContactsUser user)
        {
            var exists = _context.Users.Where(u => u.Email == user.Email).FirstOrDefault();

            if (exists != null && exists.UserName == user.UserName)
            {
                if (exists.PasswordHash == null)
                {
                    exists.PasswordHash = GenerateSHA512String(user.PasswordHash);

                    _context.SaveChanges();
                }
                else return await  Task.FromResult($"{user.Email} is already taken");
            }
            else
            {
                user.PasswordHash = GenerateSHA512String(user.PasswordHash);
                
                var createdUser = CreateAsync(new ContactDto {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    User = user,
                    Role = Role.User
                });
            }

            return await Task.FromResult($"{user.Email} registered successfully");
        }
    }
}