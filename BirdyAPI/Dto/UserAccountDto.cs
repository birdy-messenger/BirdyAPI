namespace BirdyAPI.Dto
{
    public class UserAccountDto
    {
        public UserAccountDto(string firstName, string avatarReference)
        {
            FirstName = firstName;
            AvatarReference = avatarReference;
        }
        public string FirstName { get; set; }
        public string AvatarReference { get; set; }
    }
}
