using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Tools.Exceptions;
using BirdyAPI.Types;

namespace BirdyAPI.Services
{
    public class AccessService
    {
        private readonly BirdyContext _context;

        public AccessService(BirdyContext context)
        {
            _context = context;
        }

        public void CheckChatUserAccess(int userId, int chatNumber, ChatStatus statusToCheck)
        {
            ChatUser currentUserChat = _context.ChatUsers.SingleOrDefault(k => k.ChatNumber == chatNumber && k.UserInChatID == userId);
            if (currentUserChat == null || currentUserChat.Status != statusToCheck)
                throw new InsufficientRightsException();
        }

        public void CheckChatUserAccess(int userId, int chatNumber, ChatStatus statusToCheck,
            ChatStatus anotherStatusToCheck)
        {
            try
            {
                CheckChatUserAccess(userId, chatNumber, statusToCheck);
            }
            catch (InsufficientRightsException)
            {
                CheckChatUserAccess(userId, chatNumber, anotherStatusToCheck);
            }
        }
    }
}
