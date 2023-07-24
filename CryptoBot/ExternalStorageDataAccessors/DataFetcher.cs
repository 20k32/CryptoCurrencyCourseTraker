using CryptoBot.ExternalStorageDataAccessors.Database;
using CryptoBot.ExternalStorageDataAccessors.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace CryptoBot.ExternalStorageDataAccessors
{
    internal class DataFetcher
    {

        private const int MINUTES_TO_UPDATE_RATING = 110;
        private DateTime[] Updated;

        private CryptoCurrenciesDb Database;
        private ExternalStorageParser Parser;

        public DataFetcher(ExternalStorageParser parser, CryptoCurrenciesDb database)
        {
            Database = database;
            Parser = parser;

            var CacheAccess = DateTime.Now;

            Updated = Enumerable.Range(0, 12)
                .Select(x => CacheAccess.AddMinutes(x * 10))
                .ToArray();
        }

        public async Task<CurrencyListModel?> GetEntriesByNameOrFullName(string name)
        {
            var task = EnsureCacheContainsValidValues()
                   .ContinueWith(AddToParser);

            await task;

            var result = await Database.GetManyByNameOrFullName(name);

            return result;
        }

        public async Task<CurrencyListModel?> GetEntriesRange(int from, int to)
        {
            var task = EnsureCacheContainsValidValues()
                   .ContinueWith(AddToParser);

            await task;

            var result = await Database.GetManyInRange(from, to);

            return result;
        }

        private async Task AddToParser(object _)
        {
            if (Parser.CurrencyBag is not null
               && Parser.CurrencyBag.Count != 0)
            {
                foreach (var item in Parser.CurrencyBag)
                {
                    await Database.UpdateEntry(item);
                }
            }
        }

        //мне нужно добавлять время к обновленной части
        private Task EnsureCacheContainsValidValues()
        {
            var currentDateTime = DateTime.Now;
            int index = -1;

            for (int i = 0; i < Updated.Length; i++)
            {
                if(Updated[i] < currentDateTime)
                {
                    Updated[i] = currentDateTime.AddMinutes(MINUTES_TO_UPDATE_RATING + (i * 10));
                    index = i + 1;
                    break;
                }
            }

            return (index) switch
            {
                -1 => Task.CompletedTask,
                < 12 => Parser.UpdateNthPart(index),
                _ => Parser.UpdateLastPart()
            };
        }
    }
}
