using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using otsextensions.mvc.Application.Managers;
using otsextensions.mvc.Application.Mappers;
using otsextensions.mvc.Models;

namespace otsextensions.mvc.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewManager _manager;
        private readonly ReviewMapper _mapper;

        public ReviewController(ReviewManager manager, ReviewMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View(new ReviewViewModel());
        }

        [HttpPost]
        public FileResult Summary(IFormCollection data)
        {
            var userName = data["UserName"];
            var password = data["Password"];

            var reviews = _manager.GetCFEReviews(userName, password);

            var fileData = _mapper.MapToBytes(reviews);

            var result = File(fileData, "text/csv");
            result.FileDownloadName = "ots-summary.csv";

            return result;

        }
    }
}