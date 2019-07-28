namespace BirdyAPI
{
    public static class Configurations
    {
        public static string SendGridApiKey;

        public static string BlobStorageApiKey;
        public static string AzureDatabaseConnectionString;

        public static string OurEmailAddress = "birdy-noreply@birdy.com";

        public static string EmailConfirmMessage =
            "Hi! <br>You need to confirm your email to finish registration in Birdy messenger." +
            " Click on this link : ";
    }
}
