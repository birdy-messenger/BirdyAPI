﻿using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Tools;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        public List<UserFriend> GetFriends(int userId)
        {
            IQueryable<User> userFriendsInfo = _context.Users.Where(k =>
                _context.Friends.Where(e => e.FirstUserID == userId && e.RequestAccepted)
                    .Any(x => x.SecondUserID == k.Id)).Union(_context.Users.Where(k =>
                _context.Friends.Where(e => e.SecondUserID == userId && e.RequestAccepted)
                    .Any(x => x.FirstUserID == k.Id)));

            List <UserFriend> userFriends = userFriendsInfo.Select(k => new UserFriend
                {Avatar = k.AvatarReference, Id = k.Id, FirstName = k.FirstName}).ToList();

            return userFriends;
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
