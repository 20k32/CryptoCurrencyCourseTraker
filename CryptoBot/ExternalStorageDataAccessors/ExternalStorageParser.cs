using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using CryptoBot.ExternalStorageDataAccessors.Database;
using CryptoBot.ExternalStorageDataAccessors.Models;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Collections.Concurrent;

namespace CryptoBot.ExternalStorageDataAccessors
{

    // всего страниц 364
    // будем обновлять по 125 страниц в 10 минут (обновление занимает 10 секунд + перепись данных в mongoDb)
    // перед загрузкой бота можно ОДИН РАЗ спарсить все данные по-порядку и занести их в базу данных mongodb
    // после того, как 125 страниц получено в коллекцию cconcurrent queue занести их ПО ПОРЯДКУ в базу данных mongodb
    // пользователь может использовать слепок базы данных в любое время

    // от этого класса нужно только получение в неотсортированном виде значений
    // из страниц выбранного диапазона ( от 1 до 125 и т. д. до 364 )

    internal class ExternalStorageParser : IDisposable
    {
        // site has 364 pages, per 10 minutes program will update only 32 pages
        private const int PAGES_TO_UPDATE_PER_TIME = 32;

        private BotApiSettings Settings = null!;
        private Task[] AsyncParsers = null!;

        public ConcurrentQueue<CurrencyListItemModel> CurrencyBag = null!;

        private IBrowsingContext Context;
        private IConfiguration AngleConfiguration;

        public ExternalStorageParser(IOptions<BotApiSettings> botApiSettings)
        {
            Settings = botApiSettings.Value;

            var requester = new DefaultHttpRequester(Settings.UserAgent);

            AngleConfiguration = Configuration
               .Default
               .With(requester)
               .WithDefaultLoader();

            Context = BrowsingContext.New(AngleConfiguration);
            CurrencyBag = new();
            AsyncParsers = ArrayPool<Task>.Shared.Rent(PAGES_TO_UPDATE_PER_TIME);
        }

        public async Task GetCurrencyRatesFromPage(int from, int pagesToUpdatePerTime)
        {
            CurrencyBag.Clear();

            for (int i = 0; i < pagesToUpdatePerTime; i++)
            {
                AsyncParsers[i] = UnitOfExecution(from++);
            }

            await Task.WhenAll(AsyncParsers[..pagesToUpdatePerTime]);
        }

        public Task UpdateNthPart(int n) =>
            GetCurrencyRatesFromPage(((n - 1) * PAGES_TO_UPDATE_PER_TIME) + 1, PAGES_TO_UPDATE_PER_TIME);
        public Task UpdateLastPart() =>
            GetCurrencyRatesFromPage(353, 12);

        public async Task UnitOfExecution(int pageNumber)
        {
            var siteUrl = Settings.ExternalStorage + pageNumber.ToString();

            IDocument document = await Context.OpenAsync(siteUrl);

            var elements = document.QuerySelectorAll("tbody tr")
                .Select(element => new CurrencyListItemModel(
                    name: element.QuerySelector("span.chakra-text.css-1jj7b1a")!.TextContent,
                    fullName: element.QuerySelector("p.chakra-text.css-rkws3")!.TextContent,
                    price: element.QuerySelector("p.chakra-text.css-13hqrwd")?.TextContent
                           ?? element.QuerySelector("p.chakra-text.css-1n32sox")?.TextContent
                           ?? element.QuerySelector("p.chakra-text.css-8gdmdj")!.TextContent,
                    change: element.QuerySelector("p.chakra-text.css-yyku61")?.TextContent
                            ?? element.QuerySelector("p.chakra-text.css-dg4gux")?.TextContent
                            ?? "*",
                    marketCap: element.QuerySelector("td.css-15lyn3l")!.TextContent));

            foreach (var item in elements)
            {
                CurrencyBag.Enqueue(item);
            }
        }

        public void Dispose()
        {
            ArrayPool<Task>.Shared.Return(AsyncParsers);
            Context.Dispose();
        }
    }
}
