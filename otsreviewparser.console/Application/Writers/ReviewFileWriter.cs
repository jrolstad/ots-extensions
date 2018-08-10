using System.Collections.Generic;
using CsvHelper;
using otsreviewparser.console.Application.Models;

namespace otsreviewparser.console.Application.Writers
{
    public class ReviewFileWriter
    {
        public void Write(string outputFile, IEnumerable<Review> reviews)
        {
            using (var fileWriter = System.IO.File.CreateText(outputFile))
            {
                var csvWriter = new CsvWriter(fileWriter);
                csvWriter.WriteRecords(reviews);

                fileWriter.Close();
            }

        }
    }
}