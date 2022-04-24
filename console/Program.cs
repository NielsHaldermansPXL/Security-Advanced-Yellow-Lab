using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace console
{
    class Program
    {

        private static readonly HttpClient client = new HttpClient();

        static async Task GetSeatHolders()
        {
            Console.WriteLine("YELLOW LAB - HACS (HIGHLY ADVANCED CLIENT SYSTEM)");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Welcome to our 2022 interface!");
            Console.WriteLine("You can now query our web api directly from the command line!");
            Console.WriteLine("To get started, type out the name, or the areacode of the city you would like to know more about");
            Console.WriteLine("To quit, type exit");
            
            String input = Console.ReadLine();

            var accessToken = getToken().GetAwaiter().GetResult();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var stringTask = client.GetStringAsync($"http://localhost:5000/api/seatholders/{input}");

            var msg = await stringTask;
            Console.Write(msg);

            Console.ReadLine();
        }

        static async Task Main(string[] args)
        {
            await GetSeatHolders();
           
        }

        static async Task<string> getToken()
        {
            string url = "http://localhost:5002/connect/token";
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            var data = new[]
            {
                new KeyValuePair<string, string>("client_id", "console"),
                new KeyValuePair<string, string>("client_secret", "sec-console"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var response = await client.PostAsync(url, new FormUrlEncodedContent(data));
            var responsJSON = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            var accessToken = responsJSON["access_token"].ToString();


            return accessToken;
        }

    }
}
