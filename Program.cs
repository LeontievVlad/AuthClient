using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AuthClient.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient
{
    class Program
    {
        static readonly string LoginUri = "api/auth/login";
        static readonly string RegisterUri = "api/auth/register";
        static readonly string CheckNotificationUri = "api/auth/checkNotification";
        static async Task Main(string[] args)
        {
            try
            {
                var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/notificationHub")
                .WithAutomaticReconnect(new[] { TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20) })
                .Build();

                connection.Closed += async (error) =>
                {
                    Console.WriteLine("Connection closed. Reconnecting...");
                    await Task.Delay(1000);
                    await connection.StartAsync();
                };

                try
                {
                    connection.On<string, string>("ReceiveNotification", (user, message) =>
                    {
                        Console.WriteLine($"Notification for {user}: {message}");
                    });

                    await connection.StartAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"connection failed");
                }

                await ShowStartMenu();

                await connection.StopAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task LoginAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            while (true)
            {
                Console.WriteLine("Enter your username:");
                var username = Console.ReadLine();

                Console.WriteLine("Enter your password:");
                var password = Console.ReadLine();

                var loginRequest = new LoginRequest()
                {
                    Username = username,
                    Password = password
                };

                var response = await client.PostAsJsonAsync(LoginUri, loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                    Console.WriteLine(content);

                    var user = new LoginResponse(content).User;
                    await ShowMenu(user);

                    break;
                }
                else
                {
                    Console.WriteLine("Login failed, try again.");
                }
            }
        }

        public static async Task Logout()
        {
            await ShowStartMenu();
        }
        public static async Task Register()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            while (true)
            {
                Console.WriteLine("Enter your username:");
                var username = Console.ReadLine();

                Console.WriteLine("Enter your email:");
                var email = Console.ReadLine();

                Console.WriteLine("Enter your password:");
                var password = Console.ReadLine();

                var registerRequest = new User()
                {
                    Username = username,
                    Email = email,
                    Password = password
                };

                var response = await client.PostAsJsonAsync(RegisterUri, registerRequest);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(content);

                    var user = new LoginResponse(content).User;
                    await ShowMenu(user);

                    break;
                }
                else
                {
                    Console.WriteLine("Register failed, try again.");
                }
            }
        }
        public static async Task CheckNotification(int userId)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            var response = await client.PostAsJsonAsync(CheckNotificationUri, userId);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine(content.Replace("_@", Environment.NewLine));

                var user = new LoginResponse(content).User;
                await ShowMenu(user);
            }
            else
            {
                Console.WriteLine("Failed, try again.");
                Console.WriteLine("");
            }
        }
        public static async Task ShowMenu(User user)
        {
            while (true)
            {
                Console.WriteLine($"Hello {user.Username}");
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Check Notifications");
                Console.WriteLine("2. Logout");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Check Notifications selected \n");
                        await CheckNotification(user.Id);
                        break;
                    case "2":
                        Console.WriteLine("Logout selected  \n");
                        await Logout();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.  \n");
                        break;
                }
            }
        }

        public static async Task ShowStartMenu()
        {
            while (true)
            {
                Console.WriteLine("Start Menu:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Login selected  \n");
                        await LoginAsync();
                        break;
                    case "2":
                        Console.WriteLine("Register selected  \n");
                        await Register();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        return; 
                    default:
                        Console.WriteLine("Invalid choice, try again.  \n");
                        break;
                }
            }
        }
    }
}
