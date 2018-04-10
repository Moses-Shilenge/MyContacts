using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyContacts.SqlClient.Tables;

namespace MyContacts.SqlClient
{
    public class MyContactsContext : IdentityDbContext<MyContactsUser>
    {
        public MyContactsContext(DbContextOptions<MyContactsContext> options) : base(options)
        {
        }

        public DbSet<ContactDto> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Solves the error: 'The entity type 'IdentityUserLogin<string>' requires a primary key to be defined.'
        }

    }
}