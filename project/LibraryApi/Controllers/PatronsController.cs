using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/patrons")]

    public class PatronController : ControllerBase
    {
        private readonly Library _library;

        public PatronController(Library library)
        {
            _library = library;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Patron>> GetAllPatrons()
        {
            var patrons = _library.GetAllPatrons();
            return Ok(patrons);
        }

        [HttpGet("{id}")]

        public ActionResult<Patron> GetPatronById(int id)
        {
            var patron = _library.FindPatronById(id);
            if (patron == null)
            {
                return NotFound();
            }
            return Ok(patron);
        }
    }
}