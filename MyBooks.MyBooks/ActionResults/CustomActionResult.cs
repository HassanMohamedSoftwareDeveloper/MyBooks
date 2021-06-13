using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBooks.MyBooks.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooks.MyBooks.ActionResults
{
    public class CustomActionResult : IActionResult
    {
        private readonly CustomActionResultVM result;
        public CustomActionResult(CustomActionResultVM result)
        {
            this.result = result;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(result.Exception ?? result.Publisher as object)
            {
                StatusCode = result.Exception != null ? StatusCodes.Status500InternalServerError : StatusCodes.Status200OK
            };
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
