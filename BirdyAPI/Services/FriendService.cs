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
                _context.Friends.Add(Friend.Create(userId, currentUserId));
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
            _context.SaveChanges();
        }

        public  List<UserFriendDto> GetFriends(int userId)
        {
            return _context.Users
                .Where(k =>
                _context.Friends
                    .Where(e => e.FirstUserID == userId && e.RequestAccepted)
                    .Any(x => x.SecondUserID == k.Id))
                .Union(_context.Users
                    .Where(k =>
                _context.Friends
                    .Where(e => e.SecondUserID == userId && e.RequestAccepted)
                    .Any(x => x.FirstUserID == k.Id)))
                .Select(k => new UserFriendDto(k.Id, k.FirstName, k.AvatarReference))
                .ToList();
        }

        public void DeleteFriend(int userId, int friendId)
        {
            bool isItOutgoingRequest = false;
            Friend acceptedRequest = _context.Friends.Find(userId, friendId);
            if (acceptedRequest == null)
            {
                acceptedRequest = _context.Friends.Find(friendId, userId);
                isItOutgoingRequest = true;
            }

            if (isItOutgoingRequest)
            {
                _context.Friends.Remove(acceptedRequest);
                Friend inversedRequest = Friend.Create(acceptedRequest.SecondUserID, acceptedRequest.FirstUserID);
                _context.Friends.Add(inversedRequest);
                _context.SaveChanges();
            }
            else
            {
                acceptedRequest.RequestAccepted = false;
                _context.Friends.Update(acceptedRequest);
                _context.SaveChanges();
            }
        }
        public bool IsItUserFriend(int currentUserId, int userId)
        {
            Friend currentFriend = _context.Friends.Find(userId, currentUserId);
            if (currentFriend == null)
            {
                Friend currentInverseFriend = _context.Friends.Find(currentUserId, userId);
                return currentInverseFriend != null && currentInverseFriend.RequestAccepted;
            }

            return currentFriend.RequestAccepted;
        }
    }
}
