using Microsoft.AspNetCore.Mvc;
using MyBooks.MyBooks.Data.Services;
using MyBooks.MyBooks.Data.ViewModels;

namespace MyBooks.MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService authorService;

        public AuthorController(AuthorService authorService)
        {
            this.authorService = authorService;
        }
        [HttpGet("get-author-with-books-by-id/{id}")]
        public IActionResult GetAuthorWithBooks(int id)
        {
            var authorWithBooks = authorService.GetAuthorWithBooks(id);
            return Ok(authorWithBooks);
        }
        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody]AuthorVM author)
        {
            authorService.AddAuthor(author);
            return Ok();
        }
    }
}
