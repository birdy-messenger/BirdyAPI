using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BirdyAPI.Models;

namespace BirdyAPI.Services
{
    public class GetUserService
    {
        private readonly UserContext _context;

        public GetUserService(UserContext context)
        {
            _context = context;
        }

        public string SearchUserInfo(int id)
        {
            User currentUser = _context.Users.FirstOrDefault(k => k.Id == id);
            if (currentUser == null)
                return JsonConvert.SerializeObject(new {ErrorMessage = "User Not Found"});
            else
                return JsonConvert.SerializeObject(new
                    {FirstName = currentUser.FirstName, AvatarReference = currentUser.AvatarReference});
        }
    }
}
