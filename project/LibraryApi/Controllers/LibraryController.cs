using Microsoft.AspNetCore.Mvc;
using LibraryApi.Models;


namespace LibraryApi.Controllers
{
    public class CheckoutRequest
    {
        public int PatronId { get; set; }
        public string ISBN { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/library")]
    public class LibraryController : ControllerBase
    {
        private readonly Library _library;

        public LibraryController(Library library)
        {
            _library = library;
        }

        [HttpPost("checkout")]
        public ActionResult CheckOutBook([FromBody] CheckoutRequest request)
        {
            var result = _library.CheckOutBook(request.PatronId, request.ISBN);

            if (result.Contains("not found") || result.Contains("already"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        [HttpPost("return")]
        public ActionResult ReturnBook([FromBody] CheckoutRequest request)
        {
            var result = _library.ReturnBook(request.PatronId, request.ISBN);


            if (result.Contains("not found") || result.Contains("No active"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

    }
}
