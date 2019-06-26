using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APItest.Models;

namespace APItest.Services
{
    public class RegistrationService
    {
        private readonly UserContext _context;

        public RegistrationService(UserContext context)
        {
            _context = context;
        }

    }
}
