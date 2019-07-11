using System;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;

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
                return new FriendRequestAnswerDto("Request sent");
            }
            else
            {
                counterRequest.RequestAccepted = true;
                _context.Friends.Update(counterRequest);
                string newFriendName = _context.Users.Find(counterRequest.FirstUserID).FirstName;
                return new FriendRequestAnswerDto($"{newFriendName} added as friend");
            }

        }

        public IQueryable<UserFriend> GetFriends(int userId)
        {
            return _context.Users.Where(k =>
                _context.Friends.Where(e => e.FirstUserID == userId && e.RequestAccepted)
                    .Any(x => x.SecondUserID == k.Id)).Union(_context.Users.Where(k =>
                _context.Friends.Where(e => e.SecondUserID == userId && e.RequestAccepted)
                    .Any(x => x.FirstUserID == k.Id))).Select(k => new UserFriend(k.Id, k.FirstName, k.AvatarReference));

        }

        public SimpleAnswerDto DeleteFriend(int userId, int friendId)
        {
            Friend currentFriend = _context.Friends.FirstOrDefault(k =>
                (k.FirstUserID == friendId && k.SecondUserID == userId) ||
                (k.SecondUserID == friendId && k.FirstUserID == userId));

            if (currentFriend == null)
            {
                throw new Exception("User is not your friend");
            }
            else
            {
                if (currentFriend.FirstUserID == userId)
                {
                    int swapHelper = currentFriend.FirstUserID;
                    currentFriend.FirstUserID = currentFriend.SecondUserID;
                    currentFriend.SecondUserID = swapHelper;
                    currentFriend.RequestAccepted = false;
                    _context.Friends.Update(currentFriend);
                }
                else
                {
                    currentFriend.RequestAccepted = false;
                    _context.Friends.Update(currentFriend);
                }
            }
            return new SimpleAnswerDto("Friend removed");
        }
    }
}
