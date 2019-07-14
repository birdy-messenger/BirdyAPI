using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;

namespace BirdyAPI.Services
{
    public class ChatsService
    {
        private readonly BirdyContext _context;
        public ChatsService(BirdyContext context)
        {
            _context = context;
        }

        public List<ChatInfoDto> GetChats(int userId)
        {
            return _context.ChatUsers.Where(k => k.UserInChatID == userId).Select(k => GetChatInfo(k.ChatID)).ToList();
        }

        public ChatInfoDto GetChatInfo(int userId, Guid chatId)
        {
            ChatUsers userChat = _context.ChatUsers.Find( userId, chatId);
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
    }
}