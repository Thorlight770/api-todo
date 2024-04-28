using api.todo.Model;
using api.todo.Repository;

namespace api.todo.Services.Impl
{
    public class ServiceUsers : IServiceUsers
    {
        private readonly IUserRepository _repository;
        public ServiceUsers(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Add(User user)
        {
            return await _repository.Add(user);
        }

        public async Task<bool> Delete(string id)
        {
            return await _repository.Delete(id);
        }

        public async Task<User> GetById(string id)
        {
            return await _repository.GetById(id);
        }

        public async Task<User> Login(string username, string password)
        {
            return await _repository.Login(username, password);
        }

        public async Task<User> Update(User user)
        {
            return await _repository.Update(user);
        }
    }
}
