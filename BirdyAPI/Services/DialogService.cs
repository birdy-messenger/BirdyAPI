namespace BirdyAPI.Services
{
    public class DialogService
    {
        private readonly BirdyContext _context;

        public DialogService(BirdyContext context)
        {
            _context = context;
        }
    }
}
