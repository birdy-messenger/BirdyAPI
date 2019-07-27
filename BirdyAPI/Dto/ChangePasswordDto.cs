namespace BirdyAPI.Dto
{
    public class ChangePasswordDto
    {
        public string OldPasswordHash { get; set; }
        public string NewPasswordHash { get; set; }
    }
}
