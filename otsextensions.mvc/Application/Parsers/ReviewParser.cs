using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using otsextensions.mvc.Application.Models;

namespace otsextensions.mvc.Application.Parsers
{
    public class ReviewParser
    {
        public IEnumerable<Review> Parse(string reviewPageContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(reviewPageContent);

            var reviews = ParseReviewData(doc);

            return reviews;
        }

        private static IEnumerable<Review> ParseReviewData(HtmlDocument doc)
        {
            var reviewRows = doc.DocumentNode
                .SelectNodes("//table[@id='ResponsesTable']//tbody//tr");

            var reviews = new List<Review>();
            if (reviewRows == null)
                return reviews;

            foreach (var row in reviewRows)
            {
                var data = row
                    .ChildNodes
                    .Where(n => n.Name == "td")
                    .ToList();
                var review = MapToReview(data);
                reviews.Add(review);
            }
            return reviews;
        }

        private static Review MapToReview(List<HtmlNode> data)
        {
            var reviewer = GetAssignedReviewer(data);
            var completionDate = GetCompletionDate(data);
            var screenStatus = data[5].InnerText;
            var reviewStatus = GetReviewStatus(data,screenStatus, completionDate);
            return new Review
            {
                Email = data[1].InnerText,
                Name = data[2].InnerText,
                CompletionDate = completionDate,
                Time = data[4].InnerText,
                ScreenStatus = screenStatus,
                ReviewStatus = reviewStatus,
                AssignedReviewer = reviewer
            };
        }

        private static string GetReviewStatus(List<HtmlNode> data, string screenStatus, DateTime? completionDate)
        {
            if (completionDate < new DateTime(2018, 6, 1) && screenStatus == "Completed")
            {
                return "Reviewed";
            }

            return data[6].InnerText;
        }

        private static string GetAssignedReviewer(IReadOnlyList<HtmlNode> data)
        {
            return data[7]
                .ChildNodes
                .Where(n => n.Name == "select")
                .SelectMany(n => n.ChildNodes)
                .Where(n => n.GetAttributeValue("selected", "") == "selected")
                .Select(n => n.InnerText)
                .FirstOrDefault();
        }

        private static DateTime? GetCompletionDate(IReadOnlyList<HtmlNode> data)
        {
            var completionDateText = data[3].InnerText;
            if (string.IsNullOrWhiteSpace(completionDateText))
                return null;

            return DateTime.Parse(completionDateText);
        }
    }
}