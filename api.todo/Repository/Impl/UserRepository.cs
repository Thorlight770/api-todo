using api.todo.Context;
using api.todo.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace api.todo.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        private readonly string _connString;

        public UserRepository(UserContext context)
        {
            _context = context;
            _connString = context.Database.GetDbConnection().ConnectionString;
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

        public Task<List<TrackStep>> ListTrack(long requestMasterID)
        {
            var response = new List<TrackStep>();

            string query = @"SELECT * FROM request_master_track WHERE request_master_id = @requestMasterID";

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@requestMasterID", requestMasterID);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Add(new TrackStep
                            {
                                ID = reader.GetInt64(0),
                                RequestMasterID = reader.GetInt64(1),
                                Track = reader.GetString(2),
                                SubTrack = reader.GetString(3),
                                Detail1 = reader.GetString(4),
                                Detail2 = reader.GetString(5),
                                Status = reader.GetString(6),
                                CreatedDate = reader.GetDateTime(7),
                                UpdatedDate = reader.GetDateTime(8),
                                UserID = reader.GetString(9),
                                SupervisorID = reader.GetString(10),
                            });
                        }
                    }
                }
            }

            return Task.FromResult(response);
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
