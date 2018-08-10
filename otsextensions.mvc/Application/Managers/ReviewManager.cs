using System.Collections;
using System.Collections.Generic;
using System.Linq;
using otsextensions.mvc.Application.Models;
using otsextensions.mvc.Application.Parsers;
using otsextensions.mvc.Application.Services;

namespace otsextensions.mvc.Application.Managers
{
    public class ReviewManager
    {
        private readonly OtsService _otsService;
        private readonly ReviewParser _reviewParser;

        public ReviewManager(OtsService otsService, ReviewParser reviewParser)
        {
            _otsService = otsService;
            _reviewParser = reviewParser;
        }

        public IEnumerable<Review> GetCFEReviews(string userName, string password)
        {
            //var fileContent = File.ReadAllText(path);
            var pageContent = _otsService.GetPageContent(
                "https://msots.com/Account/Login",
                "https://msots.com/Review/Responses?screenId=8414&class=btn%20btn-info",
                userName,
                password);

            var reviews = _reviewParser.Parse(pageContent)
                .OrderBy(r => r.ReviewStatus)
                .ThenByDescending(r => r.CompletionDate)
                .ToList();

            return reviews;
        }
    }
}