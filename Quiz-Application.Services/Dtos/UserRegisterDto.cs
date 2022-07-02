using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Application.Services.Dtos
{
    public class UserRegisterDto
    {
        public int Sl_No { get; set; }
        public string Candidate_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender  { get; set; }
        public string Role { get; set; }
    }
}
