namespace CurrencyConverter.Infrastructure
{
    public class RateValue
    {
        public int RateValueId { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public string TargetCurrency { get; set; }
    }
}