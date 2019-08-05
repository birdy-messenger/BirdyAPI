using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Tools.Extensions
{
    public static class ListExtensions
    {
        public static ChatUser RandomItem(this List<ChatUser> chatUsers, int oldAdminId)
        {
            ChatUser oldAdmin = chatUsers.Find(k => k.UserInChatID == oldAdminId);
            chatUsers.Remove(oldAdmin);
            if (chatUsers.Count == 0)
                return oldAdmin;

            return chatUsers[new Random().Next(0, chatUsers.Count)];
        }
    }
}
