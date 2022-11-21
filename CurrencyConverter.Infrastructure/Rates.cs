using CurrencyConverter.Domain;
using System;
using System.Linq;

namespace CurrencyConverter.Infrastructure
{
    public class Rates : IRates
    {
        public decimal GetRateOf(string currency, string targetCurrency)
        {
            using (var db = new CurrencyConverterContext())
            {
                var rateValue = db.Rates.FirstOrDefault(r
                    => currency.Equals(r.Currency)
                    && targetCurrency.Equals(r.TargetCurrency));
                return rateValue == null ? 0 : rateValue.Value;
            }
        }
    }
}
