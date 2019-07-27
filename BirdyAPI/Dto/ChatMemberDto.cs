using BirdyAPI.Types;

namespace BirdyAPI.Dto
{
    public class ChatMemberDto
    {
        public string UserUniqueTag { get; set; }
        public ChatStatus UserStatus { get; set; }
    }
}
