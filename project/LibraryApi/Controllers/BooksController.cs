using Microsoft.AspNetCore.Mvc;
using LibraryApi.Models;
using System.Collections.Generic;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly Library _library;

        public BooksController(Library library)
        {
            _library = library;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAllBooks()
        {
            var books = _library.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{isbn}")]

        public ActionResult<Book> GetBookById(string isbn)
        {
            var book = _library.FindBookById(isbn);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
    }
}
