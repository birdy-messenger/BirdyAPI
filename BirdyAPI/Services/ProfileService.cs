using System.Configuration;
using BirdyAPI.Dto;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace BirdyAPI.Services
{
    public class ProfileService
    {
        private readonly BirdyContext _context;
        private readonly IConfiguration _configuration;

        public ProfileService(BirdyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public SimpleAnswerDto SetProfileAvatar(int userId, byte[] imageBytes)
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
                        ConfigurationManager.AppSettings["BlobStorage"]), true);

            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference($"user{id}");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Container);

            return container.GetBlockBlobReference("avatar.png");
        }
    }
}
