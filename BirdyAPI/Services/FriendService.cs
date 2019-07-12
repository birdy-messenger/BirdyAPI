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

        //TODO :1 Make exception logic and messages
        public void DeleteFriend(int userId, string friendUniqueTag)
        {
            User friend = _context.Users.SingleOrDefault(k => k.UniqueTag == friendUniqueTag);
            if (friend == null)
                throw new Exception();

            bool isItInversedRequest = false;
            Friend acceptedRequest = _context.Friends.Find(userId, friend.Id);
            if (acceptedRequest == null)
            {
                acceptedRequest = _context.Friends.Find(friend.Id, userId);
                isItInversedRequest = true;
            }

            if (acceptedRequest == null)
                throw new Exception();

            if(acceptedRequest.RequestAccepted == false)
                throw new Exception();

            if (isItInversedRequest)
            {
                int swapHelper = acceptedRequest.FirstUserID;
                acceptedRequest.FirstUserID = acceptedRequest.SecondUserID;
                acceptedRequest.SecondUserID = swapHelper;
                acceptedRequest.RequestAccepted = false;
                _context.Friends.Update(acceptedRequest);
            }
            else
            {
                acceptedRequest.RequestAccepted = false;
                _context.Friends.Update(acceptedRequest);
            }
        }
    }
}
