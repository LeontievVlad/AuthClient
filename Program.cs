using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AuthClient.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthClient
{
    public class Program
    {
        private static readonly HttpClientHandler httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
        };

        private static readonly HttpClient client = new HttpClient(httpClientHandler)
        {
            BaseAddress = new Uri("https://localhost:7208/api/")
        };

        public static async Task Main(string[] args)
        {
            while (true)
            {
                await Login();

                Console.WriteLine("Do you want to try again? (yes/no)");
                var answer = Console.ReadLine()?.ToLower();
                if (answer != "yes")
                {
                    break;
                }
            }
        }
    
        private static async Task Login()
        {
            Console.WriteLine("Enter your username:");
            var username = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            var password = Console.ReadLine();

            Console.WriteLine("Logining in...");

            var loginRequest = new LoginRequest { Username = username, Password = password };

            try
            {
                var response = await client.PostAsJsonAsync("auth/login", loginRequest);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    Console.WriteLine($"Welcome, {result.User.Username}! Email: {result.User.Email}");
                }
                else
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    Console.WriteLine($"Error: {error.Message}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
        }
    }

    
}
