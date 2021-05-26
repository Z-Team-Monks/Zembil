using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Views
{
    public class UserGetDto
    {
        public int Id { get; set; }        
        public string Username { get; set; }        
        public string Email { get; set; }                                
        public string Phone { get; set; }
    }
}
