namespace BirdyAPI.DataBaseModels
{
    public class Friend
    {
        public int FirstUserID { get; set; }
        public int SecondUserID { get; set; }
        public bool RequestAccepted { get; set; } 
        public Friend() { }

        private Friend(int firstUserId, int secondUserId, bool requestAccepted)
        {
            FirstUserID = firstUserId;
            SecondUserID = secondUserId;
            RequestAccepted = requestAccepted;
        }

        public static Friend Create(int firstUserId, int secondUserId)
        {
            return new Friend(firstUserId, secondUserId, false);
        }
    }
}
