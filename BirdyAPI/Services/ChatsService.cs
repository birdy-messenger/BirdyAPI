using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IQueryable<ChatInfoDto> GetAllChats(int userId)
        {
           return _context.ChatUsers.Where(k => k.UserInChatID == userId).Join(_context.ChatInfo, k => k.ChatID, e => e.ChatID,
               (k, e) => new ChatInfoDto {ChatID = k.ChatID, ChatName = e.ChatName, LastMessage = "Ыы нету пока сообщений"});
        }

        public ChatInfoDto GetChat(int userId, Guid chatId)
        {
            //Потом здесь будет джоин с табличкой с сообщениями
            ChatInfo currentChat =  _context.ChatInfo.Find(
                _context.ChatUsers.SingleOrDefault(k => k.UserInChatID == userId && k.ChatID == chatId));
            return new ChatInfoDto
                {ChatID = currentChat.ChatID, ChatName = currentChat.ChatName, LastMessage = "Ыы нету пока сообщений"};
        }
    }
}
