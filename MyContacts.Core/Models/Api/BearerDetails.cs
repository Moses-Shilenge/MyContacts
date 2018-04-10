using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.API.Models
{
    public class BearerToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
