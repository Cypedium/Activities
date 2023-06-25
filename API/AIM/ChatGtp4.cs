/* using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MyApp
{
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_OPENAI_API_KEY");

        var content = new StringContent("{ \"model\": \"gpt-4\", \"messages\": [ {\"role\": \"user\", \"content\": \"Say this is a test!\"}], \"temperature\": 0.7 }", Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JObject.Parse(responseString);
        Console.WriteLine(responseObject["choices"][0]["message"]["content"]);
    }
} */