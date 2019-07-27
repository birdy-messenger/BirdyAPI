using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Tools.Extensions
{
    public static class ListExtensions
    {
        public static ChatUser RandomItem(this List<ChatUser> chatUsers)
        {
            return chatUsers[new Random().Next(0, chatUsers.Count)];
        }
    }
}
