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
            return _context.ChatUsers.Where(k => k.UserInChatID == userId && k.Status >= ChatStatus.User)
                .Select(k => GetChatPreview(k.ChatID, userId)).ToList();
        }

        public ChatInfoDto GetChat(int userId, int chatNumber)
        {
            Guid chatId = GetChatIdByChatNumberAndUserId(userId, chatNumber);
            List<Message> lastMessages = _context.Messages.Where(k => k.ConversationID == chatId).OrderByDescending(k => k.SendDate)
                .Take(50).ToList();

            List<ChatUser> chatUsers = _context.ChatUsers.Where(k => k.ChatID == chatId && k.Status >= ChatStatus.User).ToList();

            return new ChatInfoDto
            {
                Messages = lastMessages
                    .Select(k => 
                        new MessageDto
                        {
                            AuthorUniqueTag = GetUserUniqueTag(k.AuthorID),
                            Message = k.Text,
                            MessageTime = k.SendDate
                        })
                    .ToList(),
                Users = chatUsers
                    .Select(k => 
                        Tuple.Create(GetUserUniqueTag(k.UserInChatID), k.Status)).ToList()
            };
        }

        private ChatPreviewDto GetChatPreview(Guid chatId, int currentUserId)
        {
            ChatInfo currentChat = _context.ChatInfo.Find(chatId);

            Message chatLastMessage = _context.Messages.Where(k => k.ConversationID == currentChat.ChatID)
                .OrderByDescending(k => k.SendDate).FirstOrDefault();

            return new ChatPreviewDto
            {
                ChatNumber = GetChatNumber(chatId, currentUserId),
                ChatName = currentChat.ChatName,
                LastMessage = chatLastMessage?.Text,
                LastMessageAuthor = chatLastMessage == null ? null : GetUserUniqueTag(chatLastMessage.AuthorID),
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
                    ChatNumber = GetNewChatNumber(k)
                });
            });

            _context.ChatInfo.Add(new ChatInfo {ChatID = newChatAdmin.ChatID, ChatName = "New chat"});
            _context.SaveChanges();
        }

        public void AddUserToChat(int currentUserId, int userId, int chatNumber)
        {
            Guid currentChatId =
                _context.ChatUsers.Single(k => k.ChatNumber == chatNumber && k.UserInChatID == currentUserId).ChatID;

            _context.ChatUsers.Add(new ChatUser
            {
                ChatID = currentChatId,
                UserInChatID = userId,
                ChatNumber = GetNewChatNumber(userId),
                Status = ChatStatus.User
            });
        }

        private int GetNewChatNumber(int userId)
        {
            return _context.ChatUsers.Count(e => e.UserInChatID == userId) + 1;
        }

        private int GetChatNumber(Guid chatId, int currentUserId)
        {
            return _context.ChatUsers.Single(k => k.ChatID == chatId && k.UserInChatID == currentUserId).ChatNumber;
        }

        private string GetUserUniqueTag(int userId)
        {
            return _context.Users.Find(userId).UniqueTag;
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