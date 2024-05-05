using System.ComponentModel.DataAnnotations;

namespace api.todo.Model
{
    // model validation https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-6.0
    public class Auth
    {
        public string? Token { get; set; }
    }

    public class AuthRq
    {
        [Required(ErrorMessage = "Username is null or empty !")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is null or empty !")]
        public string? Password { get; set; }
    }
}
