using MyContacts.API.Models;
using MyContacts.SqlClient.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyContacts.Core.Services.Aunthenticate
{
    public interface IAunthenticateRepository : IRepository<ContactDto, Guid>
    {
        Task<MyContactsUser> ValidateLoginCredsAsync(LoginViewModel contactToUpdate);
    }
}
