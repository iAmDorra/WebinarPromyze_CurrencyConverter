using CurrencyConverter.Domain;
using System.Linq;

namespace CurrencyConverter.Infrastructure
{
    public class CurrencyVerifier : ICurrencyVerifier
    {
        public bool Verify(Currency currency)
        {
            using (var db = new CurrencyConverterContext())
            {
                var rateValue = db.Rates.FirstOrDefault(r
                    => currency.Is(r.Currency)
                    || currency.Is(r.TargetCurrency));
                return rateValue != null;
            }
        }
    }
}
