using api.todo.Model;

namespace api.todo.Services
{
    public interface IServiceUsers
    {
        public Task<User> GetById(string id);
        public Task<User> Login(string username, string password);
    }
}
