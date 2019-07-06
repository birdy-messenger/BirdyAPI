namespace BirdyAPI.Dto
{
    public class FriendInfoDto
    {
        public FriendInfoDto(int id, string firstName, string avatar)
        {
            Id = id;
            FirstName = firstName;
            Avatar = avatar;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Avatar { get; set; }
    }
}
