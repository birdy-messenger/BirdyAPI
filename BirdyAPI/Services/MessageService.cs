namespace BirdyAPI.Services
{
    public class MessageService
    {
        private readonly BirdyContext _context;
        public MessageService(BirdyContext context)
        {
            _context = context;
        }
    }
}