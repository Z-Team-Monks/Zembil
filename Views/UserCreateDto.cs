using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class UserCreateDto
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
