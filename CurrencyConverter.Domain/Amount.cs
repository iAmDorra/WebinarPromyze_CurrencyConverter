namespace CurrencyConverter.Domain
{
    public class Amount
    {
        private decimal v;

        public Amount(decimal v)
        {
            this.v = v;
        }

        internal bool IsNegative()
        {
            return v < 0;
        }

        internal Amount MultiplyBy(decimal conversionRate)
        {
            return new Amount(v * conversionRate);
        }

        public override bool Equals(object obj)
        {
            var amount = obj as Amount;
            return amount != null && v.Equals(amount.v);
        }

        public override int GetHashCode()
        {
            return v.GetHashCode();
        }
    }
}