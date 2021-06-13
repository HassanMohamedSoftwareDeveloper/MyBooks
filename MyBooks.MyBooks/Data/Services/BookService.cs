using MyBooks.MyBooks.Data.Models;
using MyBooks.MyBooks.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBooks.MyBooks.Data.Services
{
    public class BookService
    {
        private readonly AppDbContext _appDbContext;

        public BookService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public void AddBookWithAuthors(BookVM book)
        {
            var _book = new Book
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId=book.PublisherId
            };
            _appDbContext.Books.Add(_book);
            _appDbContext.SaveChanges();
            foreach (var id in book.AuthorIds)
            {
                var book_Author = new Book_Author
                {
                    AuthorId=id,
                    BookId=_book.Id
                };
                _appDbContext.Book_Authors.Add(book_Author);
                _appDbContext.SaveChanges();
            }
           
        }
        public List<Book> GetAllBooks() => _appDbContext.Books.ToList();
        public BookWithAuthorsVM GetBookById(int bookId)
        {
            var bookWithauthor = _appDbContext.Books.Where(x=>x.Id==bookId).Select(book => new BookWithAuthorsVM
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(x => x.Author.FullName).ToList()
            }).FirstOrDefault();
            return bookWithauthor;
        }
        public Book UpdateBookById(int bookId,BookVM book)
        {
            var _book = _appDbContext.Books.FirstOrDefault(x => x.Id == bookId);
            if (_book!=null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;
                _appDbContext.SaveChanges();
            }
            return _book;
        }
        public void DeleteBookById(int bookId)
        {
            var _book = _appDbContext.Books.FirstOrDefault(x => x.Id == bookId);
            if (_book!=null)
            {
                _appDbContext.Books.Remove(_book);
                _appDbContext.SaveChanges();
            }
        }
    }
}
