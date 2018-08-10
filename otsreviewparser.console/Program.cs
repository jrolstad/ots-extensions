using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using HtmlAgilityPack;
using otsreviewparser.console.Application.Models;
using otsreviewparser.console.Application.Parsers;
using otsreviewparser.console.Application.Services;
using otsreviewparser.console.Application.Writers;

namespace otsreviewparser.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\jorolsta\Downloads\Responses for CFE OTS SWE2.html";
            var outputFile = $"c:\\temp\\reviews-{DateTime.Now.Ticks}.csv";

            var service = new OtsService();
            //var fileContent = File.ReadAllText(path);
            var fileContent = service.GetPageContent(
                "https://msots.com/Account/Login",
                "https://msots.com/Review/Responses?screenId=8414&class=btn%20btn-info",
                "username",
                "pwd");
            var parser = new ReviewParser();
            
            var reviews = parser.Parse(fileContent)
                .OrderBy(r => r.ReviewStatus)
                .ThenByDescending(r => r.CompletionDate)
                .ToList();

            var writer = new ReviewFileWriter();
            writer.Write(outputFile, reviews);

            Console.WriteLine($"Reviews written to {outputFile}");
            Console.ReadLine();
        }

    }

}
