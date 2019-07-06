using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BirdyAPI.Services
{
    public class FriendService
    {
        private readonly BirdyContext _context;

        public FriendService(BirdyContext context)
        {
            _context = context;
        }

        public void AddFriend()
        {

        }

        public List<FriendInfoDto> GetFriends(int userId)
        {
            //TODO :2 Сделать адекватный подзапрос/джоин
            IQueryable<Friends> leftUserFriends =
                _context.Friends.Where(k => (k.FirstUserID == userId && k.RequestAccepted));

            IQueryable<Friends> rightUserFriends =
                _context.Friends.Where(k => (k.SecondUserID == userId && k.RequestAccepted));

            List<FriendInfoDto> friends = new List<FriendInfoDto>();
            foreach (var friend in leftUserFriends)
            {
                User currentFriend = _context.Users.Find(friend.FirstUserID);
                friends.Add(new FriendInfoDto(currentFriend.Id, currentFriend.FirstName, currentFriend.AvatarReference));
            }
            foreach (var friend in leftUserFriends)
            {
                User currentFriend = _context.Users.Find(friend.SecondUserID);
                friends.Add(new FriendInfoDto(currentFriend.Id, currentFriend.FirstName, currentFriend.AvatarReference));
            }

            return friends;
        }
    }
}
