namespace Sport.Web.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Sport.Web.Models;

    public class ErrorController : Controller
    {
        public IActionResult Index(string code)
        {
            var errorMessage = string.Empty;

            if (code.Equals("404"))
            {
                errorMessage = "Page cannot be found!";
            }

            return View(new ErrorViewModel
            {
                StatusCode = int.Parse(code),
                Url = HttpContext.Features.Get<IHttpRequestFeature>().RawTarget,
                Message = errorMessage
            });
            ;
        }
    }
}