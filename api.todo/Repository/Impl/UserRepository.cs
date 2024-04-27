using api.todo.Context;
using api.todo.Model;
using System.Linq;

namespace api.todo.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public User Add(User user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(string id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> Login(string username, string password)
        {
            return _context.User.Select(x => x).Where(x => x.Email == username && x.Password == password).SingleOrDefault();
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
