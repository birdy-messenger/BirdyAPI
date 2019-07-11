namespace BirdyAPI.Dto
{
    public class RegistrationDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string UniqueTag { get; set; }

    }
}
