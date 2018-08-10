using System;

namespace otsreviewparser.console.Application.Models
{
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