namespace BirdyAPI.Dto
{
    public class FriendRequestAnswerDto
    {
        public FriendRequestAnswerDto(string answer)
        {
            FriendRequestResult = answer;
        }
        public string FriendRequestResult { get; set; }
    }
}
