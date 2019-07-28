using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Types;
using BirdyAPI.Tools.Exceptions;
using BirdyAPI.Tools.Extensions;

namespace BirdyAPI.Services
{
    public class ChatService
    {
        private readonly BirdyContext _context;
        public ChatService(BirdyContext context)
        {
            _context = context;
        }

        public List<ChatPreviewDto> GetChatsPreview(int userId)
        {
            return _context.ChatUsers
                .Where(k => k.UserInChatID == userId && k.Status >= ChatStatus.User)
                .Select(k => GetChatPreview(k.ChatID, userId))
                .ToList();
        }

        public ChatInfoDto GetChat(int userId, int chatNumber, int? offset, int? count)
        {
            Guid chatId = GetChatIdByChatNumberAndUserId(userId, chatNumber);
            List<Message> lastMessages = _context.Messages
                .Where(k => k.ConversationID == chatId)
                .OrderByDescending(k => k.SendDate)
                .Skip(offset ?? 0)
                .Take(count ?? 50)
                .ToList();

            List<ChatUser> chatUsers = _context.ChatUsers
                .Where(k => k.ChatID == chatId && k.Status >= ChatStatus.User)
                .ToList();

            return new ChatInfoDto
            {
                Messages = lastMessages
                    .Select(k => 
                        new MessageDto(GetUserUniqueTag(k.AuthorID), k.Text, k.SendDate))
                    .ToList(),
                Users = chatUsers
                    .Select(k => 
                        new ChatMemberDto(GetUserUniqueTag(k.UserInChatID), k.Status))
                    .ToList()
            };
        }

        private ChatPreviewDto GetChatPreview(Guid chatId, int currentUserId)
        {
            ChatInfo currentChat = _context.ChatInfo.Find(chatId);

            Message chatLastMessage = _context.Messages
                .Where(k => k.ConversationID == currentChat.ChatID)
                .OrderByDescending(k => k.SendDate)
                .FirstOrDefault();

            string lastMessageAuthor = GetUserUniqueTag(chatLastMessage?.AuthorID);

            return ChatPreviewDto.Create(GetChatNumber(chatId, currentUserId), currentChat.ChatName, lastMessageAuthor, chatLastMessage);
        }

        public void CreateChat(List<int> usersId, int chatCreatorId)
        {
            ChatUser newChatAdmin = ChatUser.Create(chatCreatorId, GetNewChatNumber(chatCreatorId));
            _context.ChatUsers.Add(newChatAdmin);

            usersId.ForEach(k =>
                {
                    _context.ChatUsers.Add(ChatUser.Create(newChatAdmin.ChatID, k, GetNewChatNumber(k)));
                });

            _context.ChatInfo.Add(new ChatInfo(newChatAdmin.ChatID, "New chat"));
            _context.SaveChanges();
        }

        public void AddUserToChat(int currentUserId, int userId, int chatNumber)
        {
            Guid currentChatId =
                _context.ChatUsers.Single(k => k.ChatNumber == chatNumber && k.UserInChatID == currentUserId).ChatID;

            _context.ChatUsers.Add(ChatUser.Create(currentChatId, userId, GetNewChatNumber(userId)));
        }

        private int GetNewChatNumber(int userId)
        {
            return _context.ChatUsers.Count(e => e.UserInChatID == userId) + 1;
        }

        private int GetChatNumber(Guid chatId, int currentUserId)
        {
            return _context.ChatUsers.Single(k => k.ChatID == chatId && k.UserInChatID == currentUserId).ChatNumber;
        }

        private string GetUserUniqueTag(int? userId)
        {
            return _context.Users.Find(userId)?.UniqueTag;
        }

        public void RenameChat(int userId, int chatNumber, string newChatName)
        {
            Guid currentChatId  = GetChatIdByChatNumberAndUserId(userId, chatNumber);
            ChatInfo currentChatInfo = _context.ChatInfo.Find(currentChatId);
            currentChatInfo.ChatName = newChatName;
            _context.ChatInfo.Update(currentChatInfo);
            _context.SaveChanges();
        }

        public void KickUser(int chatAdminId, int chatNumber, int userId)
        {
            Guid chatId = GetChatIdByChatNumberAndUserId(chatAdminId, chatNumber);
            ChatUser currentUser = _context.ChatUsers.SingleOrDefault(k => k.UserInChatID == userId && k.ChatID == chatId);
            if(currentUser == null)
                throw new DataNotFoundException("User not found");

            if(currentUser.Status != ChatStatus.User)
                throw new InsufficientRightsException("User haven't got permission for this action");
            currentUser.Status = ChatStatus.Kicked;
            _context.ChatUsers.Update(currentUser);
            _context.SaveChanges();
        }

        public void LeaveFromChat(int currentUserId, int chatNumber)
        {
            ChatUser currentUser = _context.ChatUsers.Single(k => k.UserInChatID == currentUserId && k.ChatNumber == chatNumber);
            if(currentUser.Status == ChatStatus.Admin)
            {
                ChatUser newAdmin = GetNewChatAdmin(currentUser.ChatID);
                newAdmin.Status = ChatStatus.Admin;
                _context.Update(newAdmin);
                _context.SaveChanges();
            }
            currentUser.Status = ChatStatus.Left;
            _context.ChatUsers.Update(currentUser);
            _context.SaveChanges();
        }

        public Guid GetChatIdByChatNumberAndUserId(int userId, int chatNumber)
        {
            return _context.ChatUsers.Single(k => k.UserInChatID == userId && k.ChatNumber == chatNumber).ChatID;
        }

        private ChatUser GetNewChatAdmin(Guid chatId)
        {
            return _context.ChatUsers.Where(k => k.ChatID == chatId).ToList().RandomItem();
        }
    }
}