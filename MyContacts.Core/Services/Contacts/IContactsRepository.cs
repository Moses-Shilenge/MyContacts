using MyContacts.API.Models;
using MyContacts.SqlClient.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyContacts.Core.Services.Contacts
{
    public interface IContactsRepository : IRepository<ContactDto, Guid>
    {
        Task<IEnumerable<ContactDto>> GetAll();
        Task<string> GetUsernameWithEmailAsync(string id);
        Task<ContactDto> GetUserWithEmail(string email);
        Task<MyContactsUser> ValidateLoginCredsAsync(LoginViewModel contactToUpdate);
        Task<ContactDto> UpdateContactAsync(ContactDto contactToUpdate);
        Task<string> RegisterUserAsync(MyContactsUser user);
    }
}
