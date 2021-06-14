using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBooks.MyBooks.ActionResults;
using MyBooks.MyBooks.Data.Models;
using MyBooks.MyBooks.Data.Services;
using MyBooks.MyBooks.Data.ViewModels;

namespace MyBooks.MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly PublisherService publisherService;
        private readonly ILogger<PublisherController> logger;

        public PublisherController(PublisherService publisherService,ILogger<PublisherController> logger)
        {
            this.publisherService = publisherService;
            this.logger = logger;
        }
        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string sortBy, string searchString, int? pageNumber)
        {
           // throw new System.Exception("Test Error");
            try
            {
                logger.LogInformation("This is just a log in GetAllPublishers()");

                var result = publisherService.GetAllPublishers(sortBy, searchString, pageNumber);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return BadRequest("Sorry,could not load publishers");
            }
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var response = publisherService.GetPublisherById(id);
            if (response == null)
            {
                return NotFound();
                // return null;
            }
            else
            {
                return Ok(response);
                // return response;
            }
        }
        //[HttpGet("get-publisher-by-id/{id}")]
        //public ActionResult<Publisher> GetPublisherById(int id)
        //{
        //    var response = publisherService.GetPublisherById(id);
        //    if (response == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return response;
        //    }
        //}

        //[HttpGet("get-publisher-by-id/{id}")]
        //public CustomActionResult GetPublisherById(int id)
        //{
        //    var response = publisherService.GetPublisherById(id);
        //    if (response == null)
        //    {
        //        var responseObj = new CustomActionResultVM()
        //        {
        //            Exception = new System.Exception("This is comming from Publisher controller")
        //        };
        //        return new CustomActionResult(responseObj);
        //    }
        //    else
        //    {
        //        var responseObj = new CustomActionResultVM()
        //        {
        //            Publisher=response
        //        };
        //        return new CustomActionResult(responseObj);
        //    }
        //}

        [HttpGet("get-publisher-books-authors-by-id/{id}")]
        public IActionResult GetPublisherData(int id)
        {
            var response = publisherService.GetPublisherData(id);
            return Ok(response);
        }
        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            publisherService.AddPublisher(publisher);
            return Ok();
        }
        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            publisherService.DeletePublisherById(id);
            return Ok();
        }
    }
}
