using MyBooks.MyBooks.Data.Models;
using MyBooks.MyBooks.Data.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyBooks.MyBooks.Data.Services
{
    public class PublisherService
    {
        private readonly AppDbContext _appDbContext;

        public PublisherService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public void AddPublisher(PublisherVM publisher)
        {
            var _publisher = new Publisher
            {
                Name = publisher.Name
            };
            _appDbContext.Publishers.Add(_publisher);
            _appDbContext.SaveChanges();
        }
        public List<Publisher> GetAllPublishers(string sortBy,string searchString,int pageNumber)
        {
            var query = _appDbContext.Publishers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString,System.StringComparison.CurrentCultureIgnoreCase));
            }
            switch (sortBy)
            {
                case "name desc": query.OrderByDescending(x => x.Name); break;
                default: query.OrderBy(x => x.Name); break;
            }

            return query.ToList();
        }
        public Publisher GetPublisherById(int publisherId) => _appDbContext.Publishers.FirstOrDefault(x => x.Id == publisherId);
        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            var publisherData = _appDbContext.Publishers.Where(x => x.Id == publisherId)
                 .Select(p => new PublisherWithBooksAndAuthorsVM
                 {
                     Name = p.Name,
                     BookAuthors = p.Books.Select(book => new BookAuthorVM
                     {
                         BookName = book.Title,
                         BookAuthors = book.Book_Authors.Select(x => x.Author.FullName).ToList()
                     }).ToList()
                 }).FirstOrDefault();
            return publisherData;
        }
        public Publisher UpdatePublisherById(int publisherId, PublisherVM publisher)
        {
            var _publisher = _appDbContext.Publishers.FirstOrDefault(x => x.Id == publisherId);
            if (_publisher != null)
            {
                _publisher.Name = publisher.Name;
                _appDbContext.SaveChanges();
            }
            return _publisher;
        }
        public void DeletePublisherById(int publisherId)
        {
            var _publisher = _appDbContext.Publishers.FirstOrDefault(x => x.Id == publisherId);
            if (_publisher != null)
            {
                _appDbContext.Publishers.Remove(_publisher);
                _appDbContext.SaveChanges();
            }
        }
    }
}
