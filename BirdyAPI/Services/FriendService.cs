using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;

namespace BirdyAPI.Services
{
    public class FriendService
    {
        private readonly BirdyContext _context;

        public FriendService(BirdyContext context)
        {
            _context = context;
        }

        public FriendRequestAnswerDto AddFriend(FriendRequestDto friendRequest)
        {
            Friend counterRequest = _context.Friends.Find(friendRequest.IncomingUserID, friendRequest.OutgoingUserID);
            if (counterRequest == null)
            {
                _context.Add(new Friend(friendRequest.IncomingUserID, friendRequest.OutgoingUserID, false));
                _context.SaveChanges();
                return new FriendRequestAnswerDto("Запрос отправлен");
            }
            else
            {
                counterRequest.RequestAccepted = true;
                _context.Update(counterRequest);
                string newFriendName = _context.Users.Find(counterRequest.FirstUserID).FirstName;
                return new FriendRequestAnswerDto($"{newFriendName} добавлен в друзья");
            }

        }

        public List<FriendInfoDto> GetFriends(int userId)
        {
            //TODO :2 Сделать адекватный подзапрос/джоин
            IQueryable<Friend> leftUserFriends =
                _context.Friends.Where(k => (k.FirstUserID == userId && k.RequestAccepted));

            IQueryable<Friend> rightUserFriends =
                _context.Friends.Where(k => (k.SecondUserID == userId && k.RequestAccepted));

            List<FriendInfoDto> friends = new List<FriendInfoDto>();
            foreach (var friend in leftUserFriends)
            {
                User currentFriend = _context.Users.Find(friend.FirstUserID);
                friends.Add(new FriendInfoDto(currentFriend.Id, currentFriend.FirstName, currentFriend.AvatarReference));
            }
            foreach (var friend in rightUserFriends)
            {
                User currentFriend = _context.Users.Find(friend.SecondUserID);
                friends.Add(new FriendInfoDto(currentFriend.Id, currentFriend.FirstName, currentFriend.AvatarReference));
            }

            return friends;
        }
    }
}
