using System;
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

        public IQueryable<ChatInfoDto> GetAllChats(int userId)
        {
            return  _context.ChatUsers.Where(k => k.UserInChatID == userId)
                .Join(_context.ChatInfo, k => k.ChatID, e => e.ChatID,
                    (k, e) => new ChatInfoDto {ChatID = e.ChatID, ChatName = e.ChatName});
            //Доделать
        }

        public ChatInfoDto GetChat(int userId, Guid chatId)
        {
            ChatInfo currentChat =  _context.ChatInfo.Find(
                _context.ChatUsers.SingleOrDefault(k => k.UserInChatID == userId && k.ChatID == chatId));
            Message chatLastMessage = _context.Messages.Where(k => k.ChatID == currentChat.ChatID)
                .OrderByDescending(k => k.SendDate).FirstOrDefault();
            return new ChatInfoDto
                {ChatID = currentChat.ChatID, ChatName = currentChat.ChatName, LastMessage = chatLastMessage.Text, LastMessageTime = chatLastMessage.SendDate};
        }
    }
}
