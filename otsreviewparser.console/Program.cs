using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using HtmlAgilityPack;

namespace otsreviewparser.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\jorolsta\Downloads\Responses for CFE OTS SWE2.html";
            var outputFile = $"c:\\temp\\reviews-{DateTime.Now.Ticks}.csv";

            var reviews = ParseReviewData(path)
                .OrderBy(r => r.ReviewStatus)
                .ThenByDescending(r => r.CompletionDate)
                .ToList();

            WriteToFile(outputFile, reviews);

            Console.WriteLine($"Reviews written to {outputFile}");
            Console.ReadLine();
        }

        private static void WriteToFile(string outputFile, IEnumerable<Review> reviews)
        {
            using (var fileWriter = System.IO.File.CreateText(outputFile))
            {
                var csvWriter = new CsvWriter(fileWriter);
                csvWriter.WriteRecords(reviews);

                fileWriter.Close();
            }
              
        }

        private static IEnumerable<Review> ParseReviewData(string path)
        {
            var doc = new HtmlDocument();
            doc.Load(path);

            var reviewRows = doc.DocumentNode
                .SelectNodes("//table[@id='ResponsesTable']//tbody//tr");

            var reviews = new List<Review>();
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

            return new Review
            {
                Email = data[1].InnerText,
                Name = data[2].InnerText,
                CompletionDate = completionDate,
                Time = data[4].InnerText,
                ScreenStatus = data[5].InnerText,
                ReviewStatus = data[6].InnerText,
                AssignedReviewer = reviewer
            };
        }

        private static string GetAssignedReviewer(IReadOnlyList<HtmlNode> data)
        {
            return data[7]
                .ChildNodes
                .Where(n => n.Name == "select")
                .SelectMany(n=>n.ChildNodes)
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

    public class Review
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Time { get; set; }
        public string ScreenStatus { get; set; }
        public string ReviewStatus { get; set; }
        public string AssignedReviewer { get; set; }
    }
}
