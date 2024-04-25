namespace api.todo.Model
{
    public class Auth
    {
        public string? Token { get; set; }
    }

    public class AuthRq
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
