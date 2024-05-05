using api.todo.Model.Enum;
using System.ComponentModel.DataAnnotations;

namespace api.todo.Model
{
    public class User
    {
        [Required(ErrorMessage = "ID can't is null or empty !")]
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }
    }
}
