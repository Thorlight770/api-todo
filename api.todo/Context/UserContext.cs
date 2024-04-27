using api.todo.Model;
using Microsoft.EntityFrameworkCore;

namespace api.todo.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
    }
}
