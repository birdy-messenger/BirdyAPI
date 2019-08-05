using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class ChatServiceTest
    {
        [Fact]
        public void SendUserList_CreateChat()
        {
            int chatCreatorId = RandomValuesFactory.GetRandomInt();
            List<int> usersId = new List<int>
            {
                DatabaseModelsFactory.GetRandomUser().Id,
                DatabaseModelsFactory.GetRandomUser().Id
            };

            using (BirdyContext context = ContextFactory.GetContext())
            {
                ChatService chatService = new ChatService(context);
                int newChatNumber = chatService.CreateChat(usersId, chatCreatorId);
                ChatUser currentChat = context.ChatUsers.SingleOrDefault(k =>
                    k.UserInChatID == chatCreatorId && k.Status == ChatStatus.Admin && k.ChatNumber == newChatNumber);

                Assert.NotNull(currentChat);
                Assert.Equal(3, context.ChatUsers.Count(k => k.ChatID == currentChat.ChatID));
            }
        }
        [Fact]
        public void AddUserToChat_UpdateChatMembers()
        {
            int chatCreatorId = RandomValuesFactory.GetRandomInt();
            ChatUser newChat = ChatUser.Create(chatCreatorId, 1);

            using (BirdyContext context = ContextFactory.GetContext())
            {
                ChatService chatService = new ChatService(context);

                context.ChatUsers.Add(newChat);
                context.SaveChanges();

                chatService.AddUserToChat(chatCreatorId, RandomValuesFactory.GetRandomInt(), newChat.ChatNumber);
                
                Assert.Equal(2, context.ChatUsers.Count(k => k.ChatID  == newChat.ChatID));
            }
        }
        [Fact]
        public void SendNewChatName_UpdateChatInfo()
        {
            int chatCreatorId = RandomValuesFactory.GetRandomInt();
            const string newChatName = "new chat";
            ChatUser newChat = ChatUser.Create(chatCreatorId, 1);
            ChatInfo newChatInfo = new ChatInfo(newChat.ChatID, newChatName);

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatUsers.Add(newChat);
                context.ChatInfo.Add(newChatInfo);
                context.SaveChanges();

                ChatService chatService = new ChatService(context);
                chatService.RenameChat(chatCreatorId, 1, RandomValuesFactory.GetRandomString());

                Assert.NotEqual(newChatName, context.ChatInfo.Single(k => k.ChatID == newChat.ChatID).ChatName);
            }
        }

        [Fact]
        public void KickUserFromChat_UpdateUserStatus()
        {
            int chatCreatorId = RandomValuesFactory.GetRandomInt();
            ChatUser newChat = ChatUser.Create(chatCreatorId, 1);
            ChatUser randomChatUser = DatabaseModelsFactory.GetRandomChatUserUser();
            randomChatUser.ChatID = newChat.ChatID;

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatUsers.AddRange(newChat, randomChatUser);
                context.SaveChanges();

                ChatService chatService = new ChatService(context);
                chatService.KickUser(chatCreatorId, 1, randomChatUser.UserInChatID);

                Assert.Equal(randomChatUser.UserInChatID,
                    context.ChatUsers.Single(k => k.ChatID == newChat.ChatID && k.Status == ChatStatus.Kicked)
                        .UserInChatID);
            }
        }

        [Fact]
        public void SendChatNumberAndChatNumber_ChatId()
        {
            ChatUser chatUser = DatabaseModelsFactory.GetRandomChatUserAdmin();
            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatUsers.Add(chatUser);
                context.SaveChanges();

                ChatService chatService = new ChatService(context);
                Assert.Equal(chatUser.ChatID, chatService.GetChatIdByChatNumberAndUserId(chatUser.UserInChatID, chatUser.ChatNumber));
            }
        }

        [Fact]
        public void LeaveFromChatNotAdmin_UpdateChatStatus()
        {
            ChatUser chatAdmin = DatabaseModelsFactory.GetRandomChatUserAdmin();
            ChatUser chatUser = DatabaseModelsFactory.GetRandomChatUserUser();
            chatUser.ChatID = chatAdmin.ChatID;

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatUsers.AddRange(chatAdmin, chatUser);
                context.SaveChanges();

                ChatService chatService = new ChatService(context);
                chatService.LeaveFromChat(chatUser.UserInChatID, chatUser.ChatNumber);

                Assert.Equal(ChatStatus.Left,
                    context.ChatUsers
                        .Single(k => k.ChatID == chatUser.ChatID && k.UserInChatID == chatUser.UserInChatID).Status);
            }
        }
        [Fact]
        public void LeaveFromChatAdmin_NewAdmin()
        {
            ChatUser chatAdmin = DatabaseModelsFactory.GetRandomChatUserAdmin();
            ChatUser chatUser = DatabaseModelsFactory.GetRandomChatUserUser();
            chatUser.ChatID = chatAdmin.ChatID;

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatUsers.AddRange(chatAdmin, chatUser);
                context.SaveChanges();

                ChatService chatService = new ChatService(context);
                chatService.LeaveFromChat(chatAdmin.UserInChatID, chatAdmin.ChatNumber);

                Assert.Equal(ChatStatus.Admin,
                    context.ChatUsers
                        .Single(k => k.ChatID == chatUser.ChatID && k.UserInChatID == chatUser.UserInChatID).Status);
            }
        }

        [Fact]
        public void GetChatsPreview_ChatsInfo()
        {
            const string prevMessageText = "prev";
            const string lastMessageText = "last";

            ChatInfo firstChat = DatabaseModelsFactory.GetRandomChatInfo();
            ChatInfo secondChat = DatabaseModelsFactory.GetRandomChatInfo();

            ChatUser firstChatAdmin = DatabaseModelsFactory.GetRandomChatUserAdmin();
            ChatUser secondChatAdmin = DatabaseModelsFactory.GetRandomChatUserAdmin();

            User firstAdminUser = DatabaseModelsFactory.GetRandomUser();
            User secondAdminUser = DatabaseModelsFactory.GetRandomUser();

            firstChatAdmin.ChatID = firstChat.ChatID;
            firstAdminUser.Id = firstChatAdmin.UserInChatID;

            secondChatAdmin.ChatID = secondChat.ChatID;
            secondAdminUser.Id = secondChatAdmin.UserInChatID;

            ChatUser firstChatUser = new ChatUser
            {
                ChatID = firstChat.ChatID,
                ChatNumber = RandomValuesFactory.GetRandomInt(),
                Status = ChatStatus.User,
                UserInChatID = secondAdminUser.Id
            };
            ChatUser secondChatUser = new ChatUser
            {
                ChatID = secondChat.ChatID,
                ChatNumber = RandomValuesFactory.GetRandomInt(),
                Status = ChatStatus.User,
                UserInChatID = firstAdminUser.Id
            };

            Message firstPrevMessage = DatabaseModelsFactory.GetMessage(secondChatUser.UserInChatID, firstChat.ChatID,
                prevMessageText, DateTime.Now.AddDays(-1));
            Message firstLastMessage = DatabaseModelsFactory.GetMessage(firstChatAdmin.UserInChatID, firstChat.ChatID,
                lastMessageText, DateTime.Now);

            Message secondPrevMessage = DatabaseModelsFactory.GetMessage(firstChatUser.UserInChatID, secondChat.ChatID,
                prevMessageText, DateTime.Now.AddDays(-1));
            Message secondLastMessage = DatabaseModelsFactory.GetMessage(secondChatAdmin.UserInChatID, secondChat.ChatID,
                lastMessageText, DateTime.Now);

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatInfo.AddRange(firstChat, secondChat);
                context.ChatUsers.AddRange(firstChatAdmin, secondChatAdmin, firstChatUser, secondChatUser);
                context.Messages.AddRange(firstPrevMessage, firstLastMessage, secondPrevMessage, secondLastMessage);
                context.Users.AddRange(firstAdminUser, secondAdminUser);
                context.SaveChanges();

                ChatService chatService = new ChatService(context);
                List<ChatPreviewDto> chatsPreview = chatService.GetChatsPreview(firstAdminUser.Id);

                Assert.Equal(2, chatsPreview.Count);
                chatsPreview.ForEach(k =>
                {
                    Assert.Equal(lastMessageText, k.LastMessage);
                    Assert.NotNull(k.LastMessageAuthor);
                });
            }
        }

        [Fact]
        public void GetChat_ChatInfo()
        {
            ChatUser currentChatAdmin = DatabaseModelsFactory.GetRandomChatUserAdmin();
            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatUsers.Add(currentChatAdmin);

                for (int i = 0; i < 5; i++)
                {
                    Message chatMessage = DatabaseModelsFactory.GetMessage(currentChatAdmin.UserInChatID,
                        currentChatAdmin.ChatID, i.ToString(), DateTime.Now.AddDays(-5 + i));
                    context.Messages.Add(chatMessage);
                }

                context.SaveChanges();

                ChatService chatService = new ChatService(context);
                ChatInfoDto chatInfo = chatService.GetChat(currentChatAdmin.UserInChatID, currentChatAdmin.ChatNumber, 2, 2);

                Assert.Single(chatInfo.Users);
                Assert.Equal("2", chatInfo.Messages[0].Message);
                Assert.Equal("1", chatInfo.Messages[1].Message);

            }
        }
    }
}
