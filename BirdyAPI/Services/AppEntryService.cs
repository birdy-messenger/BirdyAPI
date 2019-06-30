using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Services
{
    public class AppEntryService
    {
        private readonly UserContext _context;
        public AppEntryService(UserContext context)
        {
            _context = context;
        }
    }
}
