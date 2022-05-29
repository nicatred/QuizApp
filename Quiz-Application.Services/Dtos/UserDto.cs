using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.Concrete
{
    public class UserDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string Password { get; set; }
    }
}
