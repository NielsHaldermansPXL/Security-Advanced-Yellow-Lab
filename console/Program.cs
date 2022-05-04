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

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var stringTask = client.GetStringAsync($"http://localhost:5000/api/seatholders/{input}");

            var msg = await stringTask;
            Console.Write(msg);

            Console.ReadLine();
        }

        static async Task Main(string[] args)
        {
            await GetSeatHolders();
            //await getToken();
        }

        static async Task<string> getToken()
        {
            string url = "https://dev-z6sboel7.eu.auth0.com/oauth/token";
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            var data = new[]
            {
                new KeyValuePair<string, string>("client_id", "J9qJ1TitU7pCjp4QyRP2OERTOLYFhHtn"),
                new KeyValuePair<string, string>("client_secret", "e41SL9_Mjb1pMpl-EdwNzrIW8q6iuDR8Guz8xbMQiABM-I_UZn4vNOiTEPZfoNLU"),
                new KeyValuePair<string, string>("audience", "https://pesecadv/api"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var response = await client.PostAsync(url, new FormUrlEncodedContent(data));

            var responsJSON = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            var accessToken = responsJSON["access_token"].ToString();

            //Console.WriteLine(accessToken);
            //Console.ReadLine();

            return accessToken;
        }

    }
}
