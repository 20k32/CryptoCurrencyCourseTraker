using CryptoBot.ExternalStorageDataAccessors.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBot.ExternalStorageDataAccessors.Database
{
    internal class CryptoCurrenciesDb
    {
        private const string DOCUMENT_NAME = "CryptoCurrencyBot";
        private const string COLLECTION_NAME = "CryptoCurrencies";
        private IMongoCollection<CurrencyListItemModel> Database = null!;

        public CryptoCurrenciesDb()
        {
            Database = MongoDBExtensions
                .GetDatabaseForDocument(DOCUMENT_NAME)
                .GetCollection<CurrencyListItemModel>(COLLECTION_NAME);
        }

        public Task UpdateEntry(CurrencyListItemModel model)
        {
            var filterDefinition = Builders<CurrencyListItemModel>
                .Filter
                .Eq(elementInDb => elementInDb.Name, model.Name);

            var updateDefinition = Builders<CurrencyListItemModel>
                .Update
                .Set(elementInDb => elementInDb.Name, model.Name)
                .Set(elementInDb => elementInDb.FullName, model.FullName)
                .Set(elementInDb => elementInDb.Price, model.Price)
                .Set(elementInDb => elementInDb.MarketCap, model.MarketCap);

            return Database.FindOneAndUpdateAsync(filterDefinition, updateDefinition);
        }

        public Task InsertOne(CurrencyListItemModel model) =>
            Database.InsertOneAsync(model);

        public Task InsertRange(IEnumerable<CurrencyListItemModel> models) =>
            Database.InsertManyAsync(models, new InsertManyOptions() { IsOrdered = true });

        public async Task<CurrencyListModel?> GetManyByNameOrFullName(string name)
        {
            var collectionByShortName = await GetManyByShortName(name) ?? new(Array.Empty<CurrencyListItemModel>());
            var collectonByFullName = await GetManyByFullName(name) ?? new(Array.Empty<CurrencyListItemModel>());

            var resultCollection = collectionByShortName
                .CurrencyList
                .Concat(collectonByFullName.CurrencyList);

            resultCollection = resultCollection.Distinct();


            return new CurrencyListModel(resultCollection.ToArray());
        }

        public Task<CurrencyListModel?> GetManyByShortName(string name)
        {
            var filter = new BsonDocument("Name", new BsonDocument() { { "$regex", $".*{name.ToUpper()}.*" } });

            var result = Database.FindSync(filter);

            while (result.Current is null
                    & !result.MoveNext())
            { }

            CurrencyListModel currencyListModel = null!;

            if(result.Current is not null)
            {
                currencyListModel = new(result.Current!.ToArray());
            }

            return Task.FromResult(currencyListModel)!;
        }


        private string FormatFullName(string name) => 
            $"{char.ToUpper(name[0])}{name[1..].ToLower()}";

        public Task<CurrencyListModel> GetManyByFullName(string name)
        {
            var filter = new BsonDocument("FullName", new BsonDocument() { { "$regex", $"{FormatFullName(name)}.*" } });

            var result = Database.FindSync(filter);

            while (result.Current is null
                    & !result.MoveNext())
            { }

            CurrencyListModel currencyListModel = null!;

            if (result.Current is not null)
            {
                currencyListModel = new(result.Current!.ToArray());
            }

            return Task.FromResult(currencyListModel)!;
        }

        public Task<CurrencyListModel?> GetManyInRange(int from, int to)
        {
            var filter = Builders<CurrencyListItemModel>
                .Filter
                .Empty;

            var findFluent = Database
                .Find(filter)
                .Skip(from)
                .Limit(to);

            var result = findFluent.ToCursor();

            while (result.Current is null
                   & !result.MoveNext())
            { }

            CurrencyListModel currencyListModel = null!;

            if (result.Current is not null)
            {
                currencyListModel = new(result.Current!.ToArray());
            }

            return Task.FromResult(currencyListModel);

        }
    }
}
