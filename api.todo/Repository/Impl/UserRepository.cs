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

        public async Task<User> Add(User user)
        {
            user.ID = Guid.NewGuid().ToString();

            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        public async Task<bool> Delete(string id)
        {
            User data = _context.User.Where(x =>  x.ID == id).FirstOrDefault();
            
            if (data != null)
            {
                _context.User.Remove(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<User> GetById(string id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> Login(string username, string password)
        {
            return _context.User.Select(x => x).Where(x => x.Email == username && x.Password == password).SingleOrDefault();
        }

        public async Task<User> Update(User user)
        {
            _context.User.Update(user);
            _context.SaveChanges();
            return user;
        }
    }
}
