using System;
using System.Collections.Generic;
using System.Text;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Types;

namespace BirdyAPI.Test.Factories
{
    public static class DatabaseModelsFactory
    {
        public static User GetRandomUser()
        {
            User user = new User
            {
                AvatarReference = RandomValuesFactory.GetRandomString(),
                CurrentStatus = UserStatus.Confirmed,
                Email = RandomValuesFactory.GetRandomString(),
                FirstName = RandomValuesFactory.GetRandomString(),
                Id = RandomValuesFactory.GetRandomInt(),
                PasswordHash = RandomValuesFactory.GetRandomString(),
                RegistrationDate = DateTime.Now,
                UniqueTag = RandomValuesFactory.GetRandomString()
            };
            return user;
        }

        public static Message GetMessage(int authorId, Guid conversationId, string text, DateTime messageTime)
        {
            Message message = Message.Create(authorId, conversationId, text);
            message.SendDate = messageTime;
            return message;
        }

        public static ChatUser GetRandomChatUserAdmin()
        {
            return new ChatUser
            {
                ChatID = RandomValuesFactory.GetRandomGuid(),
                ChatNumber = RandomValuesFactory.GetRandomInt(),
                UserInChatID = RandomValuesFactory.GetRandomInt(),
                Status = ChatStatus.Admin
            };
        }
        public static ChatUser GetRandomChatUserUser()
        {
            ChatUser chatUser = GetRandomChatUserAdmin();
            chatUser.Status = ChatStatus.User;
            return chatUser;
        }

        public static UserSession GetRandomUserSession()
        {
            return new UserSession
            {
                Token = RandomValuesFactory.GetRandomGuid(),
                UserId = RandomValuesFactory.GetRandomInt()
            };
        }

        public static ChatInfo GetRandomChatInfo()
        {
            return new ChatInfo
            {
                ChatID = RandomValuesFactory.GetRandomGuid(),
                ChatName = RandomValuesFactory.GetRandomString()
            };
        }
    }
}
