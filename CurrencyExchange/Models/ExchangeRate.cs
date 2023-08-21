namespace CurrencyExchange.Models
{
    public record ExchangeRate
    {
        public CurrencyPair CurrencyPair { get; }
        public decimal Rate { get; }
        public ExchangeRate(CurrencyPair currencyPair, decimal rate)
        {
            CurrencyPair = currencyPair;
            Rate = rate;
        }
    }
}
