using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBot.ExternalStorageDataAccessors.Database
{
    internal static class MongoDBExtensions
    {
        private const string CONNECTION_STRING = "mongodb://localhost:27017/";

        public static IMongoDatabase GetDatabaseForDocument(string documentName)
        {
            var fullConnectionString = CONNECTION_STRING + documentName;

            var mongoClient = new MongoClient(fullConnectionString);

            return mongoClient.GetDatabase(documentName);
        }
    }
}
