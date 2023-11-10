using System.Text.RegularExpressions;

namespace ClearAppleMailSuggestions
{
    public class Program
    {
        private static readonly Regex idPattern = new(@"(MR|GP)_[A-z0-9]*");

        public static async Task Main()
        {
            // Check for required files
            if (!File.Exists("url.txt") || !File.Exists("cookies.txt") || !File.Exists("recipients.txt"))
            {
                Console.WriteLine("Required files are missing! Please see the readme.");
                Environment.Exit(1);
            }

            // Get URL & replace included MR_XXX or GP_XXX with TEMPID for easier manipulation
            string baseUrl = await File.ReadAllTextAsync("url.txt");
            baseUrl = baseUrl.Replace(idPattern.Match(baseUrl).ToString(), "TEMPID");

            // Parse recentIds from JSON response data
            string recipients = await File.ReadAllTextAsync("recipients.txt");
            var recentIds = idPattern.Matches(recipients).OfType<Match>().Select(match => match.Value).ToList();

            // Loop through parsed IDs
            foreach (var id in recentIds)
            {
                // Sub ID into URL as query param
                Uri uri = new(baseUrl.Replace("TEMPID", id));

                // Read cookies
                var cookies = await File.ReadAllTextAsync("cookies.txt");

                // Compose HTTP DELETE request
                HttpRequestMessage message = new()
                {
                    Method = HttpMethod.Delete,
                    Headers =
                    {
                        Host = new Regex(@"p[0-9]*-mcc\.icloud\.com").Match(baseUrl).ToString(),
                        Referrer = new Uri("https://www.icloud.com/")
                    }
                };

                // Add headers & auth to request
                using var handler = new HttpClientHandler();
                using var client = new HttpClient(handler) { BaseAddress = uri };
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Origin", "https://www.icloud.com");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Cookie", cookies);

                // Send request, print status to console
                var response = await client.SendAsync(message);
                Console.WriteLine($"{response.StatusCode}: {id}");
            }
        }
    }
}