namespace CurrencyExchange.Models
{
    public record CurrencyPair
    {
        public string MainCurrency { get;}
        public string IncomingCurrency { get; }
        public CurrencyPair(string mainCurrency, string incomingCurrency)
        {
            MainCurrency = mainCurrency;
            IncomingCurrency = incomingCurrency;
        }
    }
}
