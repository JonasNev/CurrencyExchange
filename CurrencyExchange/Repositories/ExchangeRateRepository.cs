using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;

namespace CurrencyExchange.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly IEnumerable<ExchangeRate> _exchangeRates = new List<ExchangeRate>
    {
        new(new CurrencyPair("DKK", "DKK"), 1.0M),
        new(new CurrencyPair("DKK", "EUR"), 1 / 7.43944M),
        new(new CurrencyPair("DKK", "USD"), 1 / 6.6311M),
        new(new CurrencyPair("DKK", "GBP"), 1 / 8.5285M),
        new(new CurrencyPair("DKK", "SEK"), 1 / 0.7610M),
        new(new CurrencyPair("DKK", "NOK"), 1 / 0.7840M),
        new(new CurrencyPair("DKK", "CHF"), 1 / 6.8358M),
        new(new CurrencyPair("DKK", "JPY"), 1 / 0.05974M)
    };

    public ExchangeRate GetExchangeRateForCurrency(string currencyCode)
    {
        return _exchangeRates.FirstOrDefault(x =>
            string.Equals(x.CurrencyPair.IncomingCurrency, currencyCode, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<string> GetSupportedCurrencies()
    {
        return _exchangeRates
            .Select(x => x.CurrencyPair.IncomingCurrency)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x);
    }
}