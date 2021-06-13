using Microsoft.AspNetCore.Mvc;
using MyBooks.MyBooks.Data.Services;
using MyBooks.MyBooks.Data.ViewModels;

namespace MyBooks.MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService bookService;

        public BooksController(BookService bookService)
        {
            this.bookService = bookService;
        }
        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks()
        {
            var books = bookService.GetAllBooks();
            return Ok(books);
        }
        [HttpGet("get-book-by-id/{id}")]
        public IActionResult GetBookById(int id)
        {
            var books = bookService.GetBookById(id);
            return Ok(books);
        }
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody]BookVM book)
        {
            bookService.AddBookWithAuthors(book);
            return Ok();
        }
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] BookVM book)
        {
            var _book = bookService.UpdateBookById(id, book);
            return Ok(_book);
        }
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            bookService.DeleteBookById(id);
            return Ok();
        }
    }
}
