using BirdyAPI.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    public class ExtendedController : Controller
    {
        protected ObjectResult InternalServerError(ExceptionDto exception)
        {
            return StatusCode(500, exception);
        }

        protected ObjectResult PartialContent(SimpleAnswerDto answer)
        {
            return StatusCode(206, answer);
        }
    }
}
