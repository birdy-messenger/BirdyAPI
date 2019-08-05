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
            User user = DatabaseModelsFactory.GetRandomUser();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                ProfileService profileService = new ProfileService(context);
                profileService.SetAvatar(user.Id, new byte[1]);

                Assert.NotNull(context.Users.SingleOrDefault(k => k.Id == user.Id)?.AvatarReference);
            }
        }

        [Fact]
        public void SendInvalidTag_DuplicateNameException()
        {
            string usedTag = RandomValuesFactory.GetRandomString();
            User user = DatabaseModelsFactory.GetRandomUser();
            user.UniqueTag = usedTag;

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                ProfileService profileService = new ProfileService(context);

                Assert.Throws<DuplicateNameException>(() =>
                    profileService.SetUniqueTag(RandomValuesFactory.GetRandomInt(), usedTag));
            }
        }

        [Fact]
        public void SendValidTag_UpdateTag()
        {
            string newTag = RandomValuesFactory.GetRandomString();
            User user = DatabaseModelsFactory.GetRandomUser();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                ProfileService profileService = new ProfileService(context);
                profileService.SetUniqueTag(user.Id, newTag);

                Assert.NotNull(context.Users.SingleOrDefault(k => k.Id == user.Id && k.UniqueTag == newTag));
            }
        }
    }
}
