using Microsoft.AspNetCore.Mvc;

namespace RealtorsPortal.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page you requested could not be found.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry, something went wrong on the server.";
                    break;
                default:
                    ViewBag.ErrorMessage = "Sorry, an error occurred.";
                    break;
            }

            ViewBag.StatusCode = statusCode;
            return View("Error");
        }
    }
}