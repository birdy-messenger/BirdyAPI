using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class DialogUser
    {
        [Key]
        public Guid DialogID { get; set; }
        public int FirstUserID { get; set; }
        public int SecondUserID { get; set; }
        public DialogUser() { }

        private DialogUser(Guid dialogId, int firstUserId, int secondUserId)
        {
            DialogID = dialogId;
            FirstUserID = firstUserId;
            SecondUserID = secondUserId;
        }

        public static DialogUser Create(int firstUserId, int secondUserId)
        {
            return new DialogUser(Guid.NewGuid(), firstUserId, secondUserId);
        }
    }
}
