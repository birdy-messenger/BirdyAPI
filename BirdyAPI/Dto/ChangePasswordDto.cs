using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class ChangePasswordDto
    {
        public string OldPassorwdHash { get; set; }
        public string NewPasswordHash { get; set; }
    }
}
