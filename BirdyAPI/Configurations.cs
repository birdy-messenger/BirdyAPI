using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI
{
    public static class Configurations
    {
        public static string DataBaseString = "Server=tcp:birdytest.database.windows.net,1433;Initial Catalog=BirdyDB;" +
                                       "Persist Security Info=False;" +
                                       "User ID=lol67;" +
                                       "Password=Lolilop67;" +
                                       "MultipleActiveResultSets=False;" +
                                       "Encrypt=True;" +
                                       "TrustServerCertificate=False;" +
                                       "Connection Timeout=30;";
        public static string LocalDataBaseString = "Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
