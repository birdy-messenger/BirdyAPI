using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BirdyAPI.Test.Factories
{
    public static class RandomValuesFactory
    {
        public static readonly Random Random;

        static RandomValuesFactory()
        {
            Random = new Random();
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

        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max);
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
