using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.API.Models
{
    public class ContactViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string  Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
