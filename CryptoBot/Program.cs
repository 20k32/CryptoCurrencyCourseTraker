using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using CryptoBot.ExternalStorageDataAccessors;
using CryptoBot.ExternalStorageDataAccessors.Database;
using CryptoBot.ExternalStorageDataAccessors.Models;
using CryptoBot.MediatR.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.InlineQueryResults;
using ZstdSharp.Unsafe;

namespace CryptoBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appconfig.json")
                .Build();

            IServiceCollection services = new ServiceCollection();

            services.Configure<BotApiSettings>(instance =>
                configuration.GetSection(nameof(BotApiSettings)).Bind(instance));

            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddSingleton<CommandController>();

            services.AddSingleton<TelegramBotController>();

            services.AddSingleton<ExternalStorageParser>();

            services.AddSingleton<CryptoCurrenciesDb>();

            services.AddSingleton<DataFetcher>();

            IServiceProvider provider = services.BuildServiceProvider();

            var botController = provider.GetService<TelegramBotController>();

            botController!.ServerCommandFromAdminHandler = Console.In.ReadLineAsync!;
            botController!.LogToAdminUI = Console.Out.WriteLineAsync;

            await botController.ConfigureBot();

            await botController!.StartBot();


            /*var botApiOptions = provider.GetService<IOptions<BotApiSettings>>()!.Value;

            var fetcher = new DataFetcher(botApiOptions);

            var result = await fetcher.GetEntriesRange(0, 20);

            var resultStr = string.Join<CurrencyListItemModel>("\n\n", result.CurrencyList);

            Console.WriteLine(resultStr);

            await Console.In.ReadLineAsync();

            var database = new CryptoCurrenciesDb();

            var result = await database.GetManyInRange(0, 50);

            foreach (var item in result.CurrencyList)
            {
                await Console.Out.WriteLineAsync(item.FullName + " " + item.Price);
            }*/

        }
    }
}