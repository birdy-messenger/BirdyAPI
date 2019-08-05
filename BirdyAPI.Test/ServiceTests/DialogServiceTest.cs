using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class DialogServiceTest
    {
        [Fact]
        public void GetDialogsPreview_LastMessages()
        {
            int currentUserId = RandomValuesFactory.GetRandomInt();

            User firstUser = DatabaseModelsFactory.GetRandomUser();
            firstUser.Id = currentUserId;

            User secondUser = DatabaseModelsFactory.GetRandomUser();
            User thirdUser = DatabaseModelsFactory.GetRandomUser();

            BirdyContext context = ContextFactory.GetContext();
            context.AddRange(firstUser, secondUser, thirdUser);

            DialogUser firstDialog = new DialogUser
            {
                DialogID = RandomValuesFactory.GetRandomGuid(),
                FirstUserID = firstUser.Id,
                SecondUserID = secondUser.Id
            };

            DialogUser secondDialog = new DialogUser
            {
                DialogID = RandomValuesFactory.GetRandomGuid(),
                FirstUserID = thirdUser.Id,
                SecondUserID = firstUser.Id
            };

            context.DialogUsers.AddRange(firstDialog, secondDialog);

            Message prevMessageFirstDialog = DatabaseModelsFactory.GetMessage(firstUser.Id, firstDialog.DialogID,
                RandomValuesFactory.GetRandomString(), DateTime.Now.AddDays(-1));
            Message lastMessageFirstDialog = Message.Create(secondUser.Id, firstDialog.DialogID, "last message");

            Message prevMessageSecondDialog = DatabaseModelsFactory.GetMessage(firstUser.Id, firstDialog.DialogID,
                RandomValuesFactory.GetRandomString(), DateTime.Now.AddDays(-1));
            Message lastMessageSecondDialog = Message.Create(secondUser.Id, secondDialog.DialogID, "last message");

            context.Messages.AddRange(prevMessageFirstDialog, lastMessageFirstDialog, prevMessageSecondDialog, lastMessageSecondDialog);
            context.SaveChanges();

            DialogService dialogService = new DialogService(context);
            dialogService.GetDialogsPreview(currentUserId).ForEach(k => Assert.Equal("last message", k.LastMessage));
        }

        [Fact]
        public void GetDialog_DialogMessages()
        {
            BirdyContext context = ContextFactory.GetContext();
            DialogService dialogService = new DialogService(context);
            User firstUser = DatabaseModelsFactory.GetRandomUser();
            User secondUser = DatabaseModelsFactory.GetRandomUser();
            DialogUser currentDialog = new DialogUser
            {
                DialogID = RandomValuesFactory.GetRandomGuid(),
                FirstUserID = firstUser.Id,
                SecondUserID = secondUser.Id
            };
            context.Users.AddRange(firstUser, secondUser);
            context.DialogUsers.Add(currentDialog);
            for (int i = 0; i < 5; i++)
            {
                Message message = DatabaseModelsFactory.GetMessage(firstUser.Id, currentDialog.DialogID,
                    i.ToString(), DateTime.Now.AddDays(-5 + i));
                context.Messages.Add(message);
            }

            context.SaveChanges();
            List<MessageDto> dialogMessages = dialogService.GetDialog(firstUser.Id, secondUser.Id, 2, 2);
            Assert.Equal("2", dialogMessages[0].Message);
            Assert.Equal("1", dialogMessages[1].Message);
        }
    }
}
