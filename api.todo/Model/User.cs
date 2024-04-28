﻿using api.todo.Model.Enum;

namespace api.todo.Model
{
    public class User
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }
    }
}
