using CurrencyConverter.Domain;
using System.Linq;

namespace CurrencyConverter.Infrastructure
{
    public class CurrencyVerifier : ICurrencyVerifier
    {
        public bool Verify(string currency)
        {
            using (var db = new CurrencyConverterContext())
            {
                var rateValue = db.Rates.FirstOrDefault(r
                    => currency.Equals(r.Currency)
                    || currency.Equals(r.TargetCurrency));
                return rateValue != null;
            }
        }
    }
}
