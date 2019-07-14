namespace BirdyAPI.Dto
{
    public class ChangePasswordDto
    {
        public string OldPassorwdHash { get; set; }
        public string NewPasswordHash { get; set; }
    }
}
