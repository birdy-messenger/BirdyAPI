namespace BirdyAPI.Dto
{
    public class ExceptionDto
    {
        public ExceptionDto(string message)
        {
            ErrorMessage = message;
        }
        public string ErrorMessage { get; set; }
    }
}
