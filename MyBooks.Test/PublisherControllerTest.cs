using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MyBooks.MyBooks.Controllers;
using MyBooks.MyBooks.Data;
using MyBooks.MyBooks.Data.Models;
using MyBooks.MyBooks.Data.Services;
using MyBooks.MyBooks.Data.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBooks.Test
{
    public class PublisherControllerTest
    {
        private static readonly DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase("BookDB").Options;
        private static AppDbContext context;

        PublisherService publisherService;
        PublisherController publisherController;
        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeddData();
            publisherService = new PublisherService(context);
            publisherController = new PublisherController(publisherService, new NullLogger<PublisherController>());
        }
        [Test,Order(1)]
        public void HTTPGET_GetAllPublishers_WithsortBy_WithSearchString_WithPageNumber_ReturnOk_Test()
        {
            IActionResult actionResult = publisherController.GetAllPublishers("name desc","Publisher 1",1);

            Assert.That(actionResult,Is.TypeOf<OkObjectResult>());

            var data = (actionResult as OkObjectResult).Value as List<Publisher>;
            Assert.That(data.Count,Is.GreaterThan(0));

            Assert.That(data.FirstOrDefault().Name,Is.EqualTo("Publisher 10"));
            Assert.That(data.FirstOrDefault().Id,Is.EqualTo(10));
        }
        [Test,Order(2)]
        public void HTTPGET_GetPublisherById_ReturnsOk_Test()
        {
            int publisherId = 1;
            IActionResult actionResult = publisherController.GetPublisherById(publisherId);

            Assert.That(actionResult,Is.TypeOf<OkObjectResult>());
            var data = (actionResult as OkObjectResult).Value as Publisher;

            Assert.That(data,Is.Not.Null);
            Assert.That(data.Id,Is.EqualTo(1));
            Assert.That(data.Name,Is.EqualTo("Publisher 1").IgnoreCase);

        }
        [Test, Order(3)]
        public void HTTPGET_GetPublisherById_ReturnsNotFound_Test()
        {
            int publisherId = 100;
            IActionResult actionResult = publisherController.GetPublisherById(publisherId);

            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());

        }
        [Test, Order(4)]
        public void HTTPPOST_AddPublisher_ReturnCreated_Test()
        {
            var publisher = new PublisherVM
            {
                Name="Publisher 11"
            };
            IActionResult actionResult = publisherController.AddPublisher(publisher);
            Assert.That(actionResult, Is.TypeOf<CreatedResult>());

            var data = (actionResult as CreatedResult).Value as Publisher;

            Assert.That(data,Is.Not.Null);
            Assert.That(data.Id,Is.Not.EqualTo(0));
            Assert.That(data.Id,Is.EqualTo(11));
            Assert.That(data.Name,Is.EqualTo("publisher 11").IgnoreCase);

            var location = (actionResult as CreatedResult).Location;
            Assert.That(location, Is.Not.Null);
            Assert.That(location, Is.EqualTo("AddPublisher").IgnoreCase);
        }

        [Test, Order(5)]
        public void HTTPPOST_AddPublisher_ReturnBadRequest_Test()
        {
            var publisher = new PublisherVM
            {
                Name = "123 Publisher 11"
            };
            IActionResult actionResult = publisherController.AddPublisher(publisher);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());

            var data = (actionResult as BadRequestObjectResult).Value as string;

            Assert.That(data, Is.Not.Null);
            Assert.That(data, Is.EqualTo($"Name starts with number, Publisher name: 123 Publisher 11").IgnoreCase);
        }
        [Test,Order(6)]
        public void HTTPDELETE_DeletePublisherById_ReturnOk_Test()
        {
            IActionResult actionResult = publisherController.DeletePublisherById(3);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public void HTTPDELETE_DeletePublisherById_ReturnBadRequest_Test()
        {
            IActionResult actionResult = publisherController.DeletePublisherById(99);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
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
