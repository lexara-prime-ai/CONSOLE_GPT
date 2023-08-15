using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatGPTClient.Utilities;

public class ConsoleGPT
{
    class GPTRoles
    {
        public string role { get; set; }
        public string content { get; set; }

    }
    public async Task sendRequest()
    {

        while (true)
        {
            Console.WriteLine("Ask a question...");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            };

            List<GPTRoles> gptRoles = new List<GPTRoles>();



            gptRoles.Add(new GPTRoles() { role = "system", content = "You're an experienced C# developer" });

            gptRoles.Add(new GPTRoles() { role = "user", content = input });

            var API_KEY = "YOUR_API_KEY";
            var _URL = "https://api.openai.com/v1/chat/completions";


            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

            var body = new
            {
                model = "gpt-3.5-turbo",
                messages = gptRoles
            };

            var bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_URL, bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                var parsedResponse = JObject.Parse(responseBody);

                var content = parsedResponse["choices"][0]["message"]["content"].ToString();

                Console.WriteLine(content);

                gptRoles.Add(new GPTRoles() { role = "assistant", content = content });
            }
            else
            {
                Console.WriteLine($"An error occured: ");
            }
        }
    }
}
