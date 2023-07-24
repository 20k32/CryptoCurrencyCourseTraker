using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBot.ExternalStorageDataAccessors.Models
{
    internal class CurrencyListModel
    {
        public CurrencyListItemModel[] CurrencyList = null!;

        public CurrencyListModel(CurrencyListItemModel[] currencyList) =>
            CurrencyList = currencyList;

        public IEnumerator<CurrencyListItemModel> GetEnumerator() =>
            CurrencyList.AsEnumerable().GetEnumerator();
    }
}
