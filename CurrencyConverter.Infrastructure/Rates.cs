using CurrencyConverter.Domain;
using System;
using System.Linq;

namespace CurrencyConverter.Infrastructure
{
    public class Rates : IRates
    {
        public decimal GetRateOf(Currency currency, Currency targetCurrency)
        {
            using (var db = new CurrencyConverterContext())
            {
                var rateValue = db.Rates.FirstOrDefault(r
                    => currency.Is(r.Currency)
                    && targetCurrency.Is(r.TargetCurrency));
                return rateValue == null ? 0 : rateValue.Value;
            }
        }
    }
}
