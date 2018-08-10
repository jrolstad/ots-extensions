using System.ComponentModel;

namespace otsextensions.mvc.Models
{
    public class ReviewViewModel
    {
        [Description("OTS User Name")]
        public string UserName { get; set; }
        [Description("Password")]
        public string Password { get; set; }
    }
}