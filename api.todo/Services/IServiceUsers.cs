using api.todo.Model;

namespace api.todo.Services
{
    public interface IServiceUsers
    {
        public Task<User> GetById(string id);
        public Task<User> Login(string username, string password);
        public Task<User> Add(User user);
        public Task<User> Update(User user);
        public Task<bool> Delete(string id);
    }
}
