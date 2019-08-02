using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BirdyAPI.Test
{
    public class TestFactory
    {
        private static readonly Random Random;
        static TestFactory()
        {
            Random = new Random();
        }
        public static BirdyContext GetContext()
        {
            var options = new DbContextOptionsBuilder<BirdyContext>()
                .UseSqlite(
                    "Data Source=BirdyTestDB.db")
                .Options;

            return new BirdyContext(options);
        }
        public static string GetRandomString()
        {
            int length = Random.Next(5, 9);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static int GetRandomInt()
        {
            return Random.Next();
        }

        public static Guid GetRandomGuid()
        {
            return Guid.NewGuid();
        }

        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}