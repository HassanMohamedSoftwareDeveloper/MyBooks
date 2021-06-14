using MyBooks.MyBooks.Data.Models;
using MyBooks.MyBooks.Data.Paging;
using MyBooks.MyBooks.Data.ViewModels;
using MyBooks.MyBooks.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyBooks.MyBooks.Data.Services
{
    public class PublisherService
    {
        private readonly AppDbContext _appDbContext;

        public PublisherService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public Publisher AddPublisher(PublisherVM publisher)
        {
            if (StringStartWithNumber(publisher.Name)) 
                throw new PublisherNameException("Name starts with number", publisher.Name);

            var _publisher = new Publisher
            {
                Name = publisher.Name
            };
            _appDbContext.Publishers.Add(_publisher);
            _appDbContext.SaveChanges();
            return _publisher;
        }
        public List<Publisher> GetAllPublishers(string sortBy,string searchString,int? pageNumber)
        {
            var query = _appDbContext.Publishers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString, System.StringComparison.CurrentCultureIgnoreCase));
            }
            query = sortBy switch
            {
                "name desc" => query.OrderByDescending(x => x.Name),
                _ => query.OrderBy(x => x.Name),
            };

            //Paging 
            int pageSize = 5;
            return PaginatedList<Publisher>.Create(query, pageNumber??1, pageSize);
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
            if (_publisher == null)
                throw new System.Exception($"The publisher with id: {publisherId} does not exist");

            _appDbContext.Publishers.Remove(_publisher);
            _appDbContext.SaveChanges();

        }

        private static bool StringStartWithNumber(string name) => Regex.IsMatch(name, @"^\d");
    }
}
