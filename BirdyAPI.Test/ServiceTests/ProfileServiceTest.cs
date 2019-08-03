using System.Data;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class ProfileServiceTest
    {
        [Fact]
        public void SendPicture_AvatarReference()
        {
            BirdyContext context = ContextFactory.GetContext();
            User user = DatabaseModelsFactory.GetRandomUser();

            context.Users.Add(user);
            context.SaveChanges();

            ProfileService profileService = new ProfileService(context);
            profileService.SetAvatar(user.Id, new byte[1]);

            Assert.NotNull(context.Users.SingleOrDefault(k => k.Id == user.Id)?.AvatarReference);
        }

        [Fact]
        public void SendInvalidTag_DuplicateNameException()
        {
            BirdyContext context = ContextFactory.GetContext();
            ProfileService profileService = new ProfileService(context);

            User user = DatabaseModelsFactory.GetRandomUser();
            const string usedTag = "testTag";
            user.UniqueTag = usedTag;

            context.Users.Add(user);
            context.SaveChanges();

            Assert.Throws<DuplicateNameException>(() =>
                profileService.SetUniqueTag(RandomValuesFactory.GetRandomInt(), usedTag));
        }

        [Fact]
        public void SendValidTag_UpdateTag()
        {
            BirdyContext context = ContextFactory.GetContext();
            ProfileService profileService = new ProfileService(context);

            User user = DatabaseModelsFactory.GetRandomUser();
            context.Users.Add(user);
            context.SaveChanges();
            string newTag = RandomValuesFactory.GetRandomString();
            profileService.SetUniqueTag(user.Id, newTag);

            Assert.NotNull(context.Users.SingleOrDefault(k => k.Id == user.Id && k.UniqueTag == newTag));
        }
    }
}
