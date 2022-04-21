// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting application...");

IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
