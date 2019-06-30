namespace BirdyAPI.Dto
{
    public class UserSessionDto
    {
        public UserSessionDto(int id, int token)
        {
            Id = id;
            Token = token;  
        }
        public int Id { get; set; }
        public int Token { get; set; }
    }
}
