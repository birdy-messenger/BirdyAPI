using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class AuthenticationDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
