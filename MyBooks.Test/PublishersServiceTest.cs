using Microsoft.EntityFrameworkCore;
using MyBooks.MyBooks.Data;
using MyBooks.MyBooks.Data.Models;
using MyBooks.MyBooks.Data.Services;
using MyBooks.MyBooks.Data.ViewModels;
using MyBooks.MyBooks.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBooks.Test
{
    public class PublishersServiceTest
    {
        private static readonly DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BookDB").Options;
        private static AppDbContext context;

        PublisherService publisherService;
        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeddData();
            publisherService = new PublisherService(context);
        }


        [Test,Order(1)]
        public void GetPublisher_WithNosortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            //Actual
            var result = publisherService.GetAllPublishers("", "", null);
            //Expected
            int expected = 5;
            //Assert
            Assert.That(result.Count, Is.EqualTo(expected));
        }

        [Test, Order(2)]
        public void GetPublisher_WithNosortBy_WithNoSearchString_WithPageNumber_Test()
        {
            //Actual
            var result = publisherService.GetAllPublishers("", "", 1);
            //Expected
            int expected = 5;
            //Assert
            Assert.That(result.Count, Is.EqualTo(expected));
        }
        [Test, Order(3)]
        public void GetPublisher_WithNosortBy_WithSearchString_WithNoPageNumber_Test()
        {
            //Actual
            var result = publisherService.GetAllPublishers("", "publisher 1", null);
            //Expected
            int expected = 2;
            //Assert
            Assert.That(result.Count, Is.EqualTo(expected));
            var r = result.FirstOrDefault();
            Assert.That(r.Id,Is.EqualTo(1));
        }
        [Test, Order(4)]
        public void GetPublisher_WithsortBy_WithNoSearchString_WithNoPageNumber_Test()
        {
            //Actual
            var result = publisherService.GetAllPublishers("name desc", "", null);
            //Expected
            int expected = 5;
            //Assert
            Assert.That(result.Count, Is.EqualTo(expected));
        }

        [Test, Order(5)]
        public void GetPublisherById_WithResponse_Test()
        {
            //Actual
            var result = publisherService.GetPublisherById(1);
            //Expected

            //Assert
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Publisher 1"));
        }
        [Test, Order(6)]
        public void GetPublisherById_WithoutResponse_Test()
        {
            //Actual
            var result = publisherService.GetPublisherById(99);
            //Expected

            //Assert
            Assert.That(result, Is.Null);
        }
        [Test, Order(7)]
        public void AddPublisher_WithException_Test()
        {
            var newPublisher = new PublisherVM { Name = "1Publisher 11" };

            Assert.That(() => publisherService.AddPublisher(newPublisher),
                Throws.Exception.TypeOf<PublisherNameException>()
                .With.Message.EqualTo("Name starts with number"));

        }
        [Test, Order(8)]
        public void AddPublisher_WithoutException_Test()
        {
            var newPublisher = new PublisherVM { Name = "Publisher 11" };
            var result = publisherService.AddPublisher(newPublisher);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Does.StartWith("Publisher"));
            Assert.That(result.Id, Is.Not.EqualTo(0));

        }
        [Test,Order(9)]
        public void GetPublisherData_Test()
        {
            var result = publisherService.GetPublisherData(1);
            Assert.That(result.Name, Is.EqualTo("Publisher 1"));
            Assert.That(result.BookAuthors,Is.Not.Empty);
            Assert.That(result.BookAuthors.Count,Is.GreaterThan(0));

            var firstBookName = result.BookAuthors.OrderBy(n => n.BookName).FirstOrDefault().BookName;
            Assert.That(firstBookName,Is.EqualTo("Book 1 Title"));

        }
        [Test,Order(10)]
        public void DeletePublisherById_PublisherExists_Test()
        {
            int publisherId = 2;

            var publisherBefore = publisherService.GetPublisherById(publisherId);
            Assert.That(publisherBefore, Is.Not.Null);
            Assert.That(publisherBefore.Name, Is.EqualTo("Publisher 2"));

            publisherService.DeletePublisherById(publisherId);

            var publisherAfter = publisherService.GetPublisherById(publisherId);
            Assert.That(publisherAfter, Is.Null);

        }
        [Test, Order(11)]
        public void DeletePublisherById_PublisherNotExists_Test()
        {
            int publisherId = 20;
            Assert.That(() => publisherService.DeletePublisherById(publisherId), Throws.Exception.TypeOf<Exception>()
                .With.Message
                .EqualTo($"The publisher with id: {publisherId} does not exist"));
        }
        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private static void SeddData()
        {
            var publishers = new List<Publisher>()
            {
                new Publisher{Id=1, Name="Publisher 1" },
                new Publisher{Id=2,Name="Publisher 2" },
                new Publisher{Id=3,Name="Publisher 3" },
                new Publisher{Id=4,Name="Publisher 4" },
                new Publisher{Id=5,Name="Publisher 5" },
                new Publisher{Id=6,Name="Publisher 6" },
                new Publisher{Id=7,Name="Publisher 7" },
                new Publisher{Id=8,Name="Publisher 8" },
                new Publisher{Id=9,Name="Publisher 9" },
                new Publisher{Id=10,Name="Publisher 10" },
            };
            context.Publishers.AddRange(publishers);
            var authors = new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    FullName = "Author 1"
                },
                new Author()
                {
                    Id = 2,
                    FullName = "Author 2"
                }
            };
            context.Authors.AddRange(authors);


            var books = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Book 1 Title",
                    Description = "Book 1 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                },
                new Book()
                {
                    Id = 2,
                    Title = "Book 2 Title",
                    Description = "Book 2 Description",
                    IsRead = false,
                    Genre = "Genre",
                    CoverUrl = "https://...",
                    DateAdded = DateTime.Now.AddDays(-10),
                    PublisherId = 1
                }
            };
            context.Books.AddRange(books);

            var books_authors = new List<Book_Author>()
            {
                new Book_Author()
                {
                    Id = 1,
                    BookId = 1,
                    AuthorId = 1
                },
                new Book_Author()
                {
                    Id = 2,
                    BookId = 1,
                    AuthorId = 2
                },
                new Book_Author()
                {
                    Id = 3,
                    BookId = 2,
                    AuthorId = 2
                },
            };
            context.Book_Authors.AddRange(books_authors);

            context.SaveChanges();
        }
    }
}