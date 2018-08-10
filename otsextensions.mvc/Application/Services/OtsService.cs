using System.Net;
using System.Net.Http;

namespace otsextensions.mvc.Application.Services
{
    public class OtsService
    {
        public string GetPageContent(string loginUrl, string pageUrl, string userName, string password)
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookies };

            using (var client = new HttpClient(handler))
            {
                // Authorize
                var authDetails = new AuthDetail { UserName = userName, Password = password };
                var authResult = client.PostAsJsonAsync(loginUrl, authDetails).Result;
                authResult.EnsureSuccessStatusCode();

                // Get content
                var response = client.GetStringAsync(pageUrl).Result;

                return response;

            }
        }
        public class AuthDetail
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}