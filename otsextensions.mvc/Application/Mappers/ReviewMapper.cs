using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;
using otsextensions.mvc.Application.Models;

namespace otsextensions.mvc.Application.Mappers
{
    public class ReviewMapper
    {
        public byte[] MapToBytes(IEnumerable<Review> reviews)
        {
            var stringValue =  MapToString(reviews);

            var bytes = Encoding.ASCII.GetBytes(stringValue);

            return bytes;
        }

        private static string MapToString(IEnumerable<Review> reviews)
        {
            using (var fileWriter = new StringWriter())
            {
                var csvWriter = new CsvWriter(fileWriter);
                csvWriter.WriteRecords(reviews);

                fileWriter.Close();

                return fileWriter.ToString();
            }
        }
    }
}