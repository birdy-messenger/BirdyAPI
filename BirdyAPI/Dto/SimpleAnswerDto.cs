namespace BirdyAPI.Dto
{
    public class SimpleAnswerDto
    {
        public SimpleAnswerDto(string message)
        {
            Result = message;
        }
        public string Result { get; set; }
    }
}
