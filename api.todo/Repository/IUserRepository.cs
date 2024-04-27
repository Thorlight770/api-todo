using api.todo.Model;

namespace api.todo.Repository
{
    public interface IUserRepository
    {
        #region Inquiry
        public Task<User> GetById(string id);
        public Task<User> Login(string username, string password);
        #endregion

        #region CRUD
        public User Add(User user);
        public User Update(User user);
        public bool Delete(string id);
        #endregion
    }
}
