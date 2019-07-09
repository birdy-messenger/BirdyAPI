using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace BirdyAPI.Services
{
    public class UserService
    {
        private readonly BirdyContext _context;
        private readonly IConfiguration _configuration;

        public UserService(BirdyContext context, IConfiguration configuration)   
        {
            _context = context;
            _configuration = configuration;
        }

        public UserAccountDto SearchUserInfo(UserSessions userSession)
        {
            User user = _context.Users.FirstOrDefault(k => k.Id == userSession.UserId);
            if (user == null)
                throw new ArgumentException("User Not Found");
            else
            {
                return new UserAccountDto(user.FirstName, user.AvatarReference);
            }
        }
        

        public SimpleAnswerDto SetProfileAvatar(int id, byte[] imageBytes)
        {
            CloudBlockBlob blob = InitAzureBlob(id);

            blob.UploadFromByteArray(imageBytes, 0, imageBytes.Length);
            CloudBlob avatarCloudBlob = blob.Container.GetBlobReference("avatar.png");
            _context.Users.Find(id).AvatarReference = avatarCloudBlob.Uri.ToString();
            _context.SaveChanges();
            return new SimpleAnswerDto(avatarCloudBlob.Uri.ToString());
        }

        private CloudBlockBlob InitAzureBlob(int id)
        {
            CloudStorageAccount account =
                new CloudStorageAccount(
                    new StorageCredentials("birdystorage",
                        _configuration.GetConnectionString("BlobStorage")), true);

            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference($"user{id}");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Container);

           return container.GetBlockBlobReference("avatar.png");
        }
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
