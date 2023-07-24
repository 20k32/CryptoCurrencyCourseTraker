using AngleSharp.Html;
using MediatR.NotificationPublishers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBot.ExternalStorageDataAccessors.Models
{
    internal class CurrencyListItemModel
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Price { get; set; }
        public string? Change { get; set; }
        public string? MarketCap { get; set; }

        public CurrencyListItemModel()
        { }

        public CurrencyListItemModel(string name = null!, string fullName = null!, string price = null!, string change = null!, string marketCap = null!) =>
            (Name, FullName, Price, Change, MarketCap) = (name, fullName, price, change, marketCap);

        public override string ToString()
        {
            return $"Name: {Name ?? "*"}\nFullName: {FullName ?? "*"}\nPrice: {Price ?? "*"}\nChange: {Change ?? "*"}\nMarketCap: {MarketCap ?? "*"}";
        }

        public string ToShortStringInTopCurrencies()
        {
            return $"{Name} ({FullName})\n{Price}\nИзменение за 24ч: {Change}\n";
        }

        public string ToStringInPagination()
        {
            return $"{Name} ({FullName})\n{Price}\nИзменение за 24ч: {Change}\nКапитализация: {MarketCap}";
        }

        public override bool Equals(object? obj)
        {
            if(obj is CurrencyListItemModel value)
            {
                return Name == value.Name
                    && FullName == value.FullName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Name + Price!.ToString()).GetHashCode();
        }
    }
}
