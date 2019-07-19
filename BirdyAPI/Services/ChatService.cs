using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Types;
using BirdyAPI.Tools.Exceptions;

namespace BirdyAPI.Services
{
    public class ChatService
    {
        private readonly BirdyContext _context;
        public ChatService(BirdyContext context)
        {
            _context = context;
        }

        public List<ChatInfoDto> GetChats(int userId)
        {
            return _context.ChatUsers.Where(k => k.UserInChatID == userId).Select(k => GetChatInfo(k.ChatID)).ToList();
        }

        public ChatInfoDto GetChatInfo(int userId, Guid chatId)
        {
            ChatUser userChat = _context.ChatUsers.Find( userId, chatId);
            if(userChat == null)
                throw new ArgumentException();

            return GetChatInfo(chatId);
        }

        private ChatInfoDto GetChatInfo(Guid chatId)
        {
            ChatInfo currentChat = _context.ChatInfo.Find(chatId);

            Message chatLastMessage = _context.Messages.Where(k => k.ChatID == currentChat.ChatID)
                .OrderByDescending(k => k.SendDate).FirstOrDefault();
            return new ChatInfoDto
            {
                ChatID = currentChat.ChatID,
                ChatName = currentChat.ChatName,
                LastMessage = chatLastMessage?.Text,
                LastMessageTime = chatLastMessage?.SendDate ?? DateTime.MinValue
            };
        }

        public void CreateChat(List<int> usersId, int chatCreatorId)
        {
            ChatUser newChatAdmin = new ChatUser
            {
                ChatID = Guid.NewGuid(),
                UserInChatID = chatCreatorId,
                Status = ChatStatus.Admin,
                ChatNumber = _context.ChatUsers.Count(k => k.UserInChatID == chatCreatorId) + 1
            };
            _context.ChatUsers.Add(newChatAdmin);
            usersId.ForEach(k =>
            {
                _context.ChatUsers.Add(new ChatUser
                {
                    ChatID = newChatAdmin.ChatID,
                    UserInChatID = k,
                    Status = ChatStatus.User,
                    ChatNumber = GetNextChatNumber(k)
                });
            });
            _context.SaveChanges();
        }

        public void AddUserToChat(int currentUserId, int userId, int chatNumber)
        {
            Guid currentChatID =
                _context.ChatUsers.Single(k => k.ChatNumber == chatNumber && k.UserInChatID == currentUserId).ChatID;

            _context.ChatUsers.Add(new ChatUser
            {
                ChatID = currentChatID,
                UserInChatID = userId,
                ChatNumber = GetNextChatNumber(userId),
                Status = ChatStatus.User
            });
        }

        private int GetNextChatNumber(int userId)
        {
            return _context.ChatUsers.Count(e => e.UserInChatID == userId) + 1;
        }
    }
}