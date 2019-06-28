using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI
{
    public static class Configurations
    {
        //TODO:1 Use json config for connection strings
        //TODO:2 Don't commit connection string to CVS
        public static string DataBaseString = "Server=tcp:birdytest.database.windows.net,1433;Initial Catalog=BirdyDB;" +
                                       "Persist Security Info=False;" +
                                       "User ID=lol67;" +
                                       "Password=Lolilop67;" +
                                       "MultipleActiveResultSets=False;" +
                                       "Encrypt=True;" +
                                       "TrustServerCertificate=False;" +
                                       "Connection Timeout=30;";
        public static string LocalDataBaseString = "Server=(localdb)\\mssqllocaldb;" +
                                                   "Database=usersdbstore;" +
                                                   "Trusted_Connection=True;" +
                                                   "MultipleActiveResultSets=true";

        public static string SendGridAPIKey = "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";
        public static string OurEmailAddress = "birdy-noreply@birdy.com";

        public static string EmailConfirmMessage =
            "Hi! <br>You need to confirm your email to finish registration in Birdy messenger." +
            " Click on this link : ";
    }
}
