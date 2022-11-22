using System;

namespace CurrencyConverter.Domain
{
    public class Currency
    {
        private string currencyLabel;

        public Currency(string currencyLabel)
        {
            this.currencyLabel = currencyLabel;
        }

        public bool Is(string currency)
        {
            return currencyLabel.Equals(currency);
        }
    }
}