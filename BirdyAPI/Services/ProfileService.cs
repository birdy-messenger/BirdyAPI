using System.Data;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;

namespace BirdyAPI.Services
{
    public class ProfileService
    {
        private readonly BirdyContext _context;
        public ProfileService(BirdyContext context)
        {
            _context = context;
        }

        public void SetUniqueTag(int userId, string newUniqueTag)
        {
            if(_context.Users.Count(k => k.UniqueTag == newUniqueTag) != 0)
                throw new DuplicateNameException("This unique tag is already occupied");

            User currentUser  = _context.Users.Find(userId);
                currentUser.UniqueTag = newUniqueTag;
                _context.Users.Update(currentUser);
                _context.SaveChanges();
        }
        public SimpleAnswerDto SetAvatar(int userId, byte[] imageBytes)
        {
            CloudBlockBlob blob = InitAzureBlob(userId);

            blob.UploadFromByteArray(imageBytes, 0, imageBytes.Length);
            CloudBlob avatarCloudBlob = blob.Container.GetBlobReference("avatar.png");
            _context.Users.Find(userId).AvatarReference = avatarCloudBlob.Uri.ToString();
            _context.SaveChanges();
            return new SimpleAnswerDto {Result = avatarCloudBlob.Uri.ToString()};
        }

        private CloudBlockBlob InitAzureBlob(int id)
        {
            CloudStorageAccount account =
                new CloudStorageAccount(
                    new StorageCredentials("birdystorage",
                        Configurations.BlobStorageApiKey), true);

            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference($"user{id}");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Container);

            return container.GetBlockBlobReference("avatar.png");
        }
    }
}
