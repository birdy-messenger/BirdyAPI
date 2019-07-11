namespace BirdyAPI.Models
{
    public class UserFriend
    {
        public UserFriend() { }

        public UserFriend(int id, string firstName, string avatar)
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
