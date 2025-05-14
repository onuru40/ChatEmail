using ChatEmail.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatEmail.Context
{
    public class ChatContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ONUR\\SQLEXPRESS;initial Catalog=ChatEmailDb;integrated security=true;trust server certificate=true");
        }

        public DbSet<Message> Messages { get; set; }
    }
}
