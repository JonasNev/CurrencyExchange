using CurrencyExchange.Models;

namespace CurrencyExchange.Interfaces;

public interface IExchangeRateRepository
{
    ExchangeRate GetExchangeRateForCurrency(string currencyCode);

    IEnumerable<string> GetSupportedCurrencies();
}