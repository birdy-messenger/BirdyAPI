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

        public void SendFriendRequest(string userUniqueTag, int currentUserId)
        {
            User userRequest = _context.Users.SingleOrDefault(k => k.UniqueTag == userUniqueTag);
            if (userRequest == null)
                throw new Exception();

            Friend inverseRequest = _context.Friends.Find(userRequest.Id, currentUserId);
            if(inverseRequest != null) //Не уверен что это нужно, но пусть пока будет
                AcceptFriendRequest(inverseRequest);
            else
            {
                _context.Friends.Add(new Friend
                    {FirstUserID = currentUserId, SecondUserID = userRequest.Id, RequestAccepted = false});
                _context.SaveChanges();
            }
        }

        public void AcceptFriendRequest(string userUniqueTag, int currentUserId)
        {
            User userRequest = _context.Users.SingleOrDefault(k => k.UniqueTag == userUniqueTag);
            if (userRequest == null)
                throw new Exception();

            Friend inverseRequest = _context.Friends.Find(userRequest.Id, currentUserId);
            AcceptFriendRequest(inverseRequest);
        }

        private void AcceptFriendRequest(Friend inverseRequest)
        {
            if (inverseRequest == null)
                throw new Exception();
            else
            {
                inverseRequest.RequestAccepted = true;
                _context.Friends.Update(inverseRequest);
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
            return new SimpleAnswerDto{Result = "Friend removed"};
        }
    }
}
