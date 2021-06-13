using MyBooks.MyBooks.Data;
using MyBooks.MyBooks.Data.Models;
using MyBooks.MyBooks.Data.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyBooks.MyBooks.Data.Services
{
    public class AuthorService
    {
        private readonly AppDbContext _appDbContext;
        public AuthorService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public void AddAuthor(AuthorVM author)
        {
            var _author = new Author
            {
                FullName = author.FullName
            };
            _appDbContext.Authors.Add(_author);
            _appDbContext.SaveChanges();
        }
        public List<Author> GetAllAuthors() => _appDbContext.Authors.ToList();
        public AuthorWithBooksVM GetAuthorWithBooks(int authorId)
        {
           var authorWithBooks= _appDbContext.Authors.Where(x=>x.Id==authorId).Select(author=>new AuthorWithBooksVM
           {
               FullName=author.FullName,
               BookTitles=author.Book_Authors.Select(x=>x.Book.Title).ToList()
           }).FirstOrDefault();
            return authorWithBooks;
        }
        public Author UpdateAuthorById(int authorId, AuthorVM author)
        {
            var _author = _appDbContext.Authors.FirstOrDefault(x => x.Id == authorId);
            if (_author != null)
            {
                _author.FullName = author.FullName;
                _appDbContext.SaveChanges();
            }
            return _author;
        }
        public void DeleteAuthorById(int authorId)
        {
            var _author = _appDbContext.Authors.FirstOrDefault(x => x.Id == authorId);
            if (_author != null)
            {
                _appDbContext.Authors.Remove(_author);
                _appDbContext.SaveChanges();
            }
        }
    }
}
