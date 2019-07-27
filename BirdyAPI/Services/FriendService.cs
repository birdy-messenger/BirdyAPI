using System;
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

        public void SendFriendRequest(int userId, int currentUserId)
        {
            Friend inverseRequest = _context.Friends.Find(userId, currentUserId);
            if (inverseRequest != null) //Не уверен что это нужно, но пусть пока будет
            {
                AcceptFriendRequest(inverseRequest);
            }
            else
            {
                _context.Friends.Add(new Friend
                    {FirstUserID = currentUserId, SecondUserID = userId, RequestAccepted = false});
                _context.SaveChanges();
            }
        }

        public void AcceptFriendRequest(int userId, int currentUserId)
        {
            Friend inverseRequest = _context.Friends.Find(userId, currentUserId);

            if (inverseRequest == null)
                throw new NullReferenceException("Friend request not found");

            AcceptFriendRequest(inverseRequest);
        }

        private void AcceptFriendRequest(Friend inverseRequest)
        {
            inverseRequest.RequestAccepted = true;
            _context.Friends.Update(inverseRequest);
        }

        public  List<UserFriendDto> GetFriends(int userId)
        {
            return _context.Users.Where(k =>
                _context.Friends.Where(e => e.FirstUserID == userId && e.RequestAccepted)
                    .Any(x => x.SecondUserID == k.Id)).Union(_context.Users.Where(k =>
                _context.Friends.Where(e => e.SecondUserID == userId && e.RequestAccepted)
                    .Any(x => x.FirstUserID == k.Id))).Select(k => new UserFriendDto{Id = k.Id, FirstName = k.FirstName, Avatar = k.AvatarReference}).ToList();
        }

        public void DeleteFriend(int userId, int friendId)
        {
            bool isItInversedRequest = false;
            Friend acceptedRequest = _context.Friends.Find(userId, friendId);
            if (acceptedRequest == null)
            {
                acceptedRequest = _context.Friends.Find(friendId, userId);
                isItInversedRequest = true;
            }

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
        public bool IsItUserFriend(int currentUserId, int userId)
        {
            Friend currentFriend = _context.Friends.Find(userId, currentUserId);
            if (currentFriend == null)
            {
                Friend currentInverseFriend = _context.Friends.Find(currentUserId, userId);
                if (currentInverseFriend.RequestAccepted)
                    return true;

                return false;
            }

            if (currentFriend.RequestAccepted)
                return true;

            return false;
        }
    }
}
