namespace BirdyAPI.DataBaseModels
{
    public class Friend
    {
        public Friend() { }

        public Friend(int firstUserId, int secondUserId, bool requestAccepted)
        {
            FirstUserID = firstUserId;
            SecondUserID = secondUserId;
            RequestAccepted = requestAccepted;
        }
        public int FirstUserID { get; set; }
        public int SecondUserID { get; set; }
        public bool RequestAccepted { get; set; } 
    }
}
