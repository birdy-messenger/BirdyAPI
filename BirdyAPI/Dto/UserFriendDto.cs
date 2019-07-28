namespace BirdyAPI.Dto
{
    public class UserFriendDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Avatar { get; set; }

        public UserFriendDto() { }

        public UserFriendDto(int id, string firstName, string avatar)
        {
            Id = id;
            FirstName = firstName;
            Avatar = avatar;
        }
    }
}
