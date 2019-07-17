using BirdyAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/dialogs")]
    public class DialogController : ExtendedController
    {
        private readonly DialogService _dialogService;

        public DialogController(BirdyContext context)
        {
            _dialogService = new DialogService(context);
        }
    }
}
