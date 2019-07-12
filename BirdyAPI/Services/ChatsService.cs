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
            List<ChatInfoDto> chats = new List<ChatInfoDto>();

            foreach (var chat in _context.ChatUsers.Where(k => k.UserInChatID == userId))
            {
                chats.Add(GetChat(chat.UserInChatID, chat.ChatID));
            }

            return chats;
        }

        public ChatInfoDto GetChat(int userId, Guid chatId)
        {
            ChatUsers userChat = _context.ChatUsers.Find( userId, chatId);
            if(userChat == null)
                throw new ArgumentException("Chat wasn't found");

            ChatInfo currentChat = _context.ChatInfo.Find(userChat.ChatID);

            Message chatLastMessage = _context.Messages.Where(k => k.ChatID == currentChat.ChatID)
                .OrderByDescending(k => k.SendDate).FirstOrDefault();
            return new ChatInfoDto
            {
                ChatID = currentChat.ChatID, ChatName = currentChat.ChatName, LastMessage = chatLastMessage?.Text,
                LastMessageTime = chatLastMessage?.SendDate ?? DateTime.MinValue
            };
        }
    }
}